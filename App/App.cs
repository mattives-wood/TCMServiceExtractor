using Data;

using Domain.Financials;
using Domain.Hotline;
using Domain.MedList;
using Domain.Meta;
using Domain.Services;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using PdfLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata;

namespace App
{
    public class App
    {
        private readonly IConfiguration _config;
        private readonly ServicesContext _servicesContext;
        private readonly MetadataContext _metadataContext;
        private readonly HotLineContext _hotLineContext;
        private readonly MedListContext _medListContext;
        private readonly FinancialsContext _financialsContext;
        private readonly string _outputPath;
        private readonly List<int> _programs;
        
        public App(IConfiguration configuration)
        {
            _config = configuration;

            DbContextOptionsBuilder<MetadataContext> metadataOptionsBuilder = new DbContextOptionsBuilder<MetadataContext>();
            metadataOptionsBuilder.UseSqlServer(_config.GetConnectionString("DocMigrationConnection"), x => x.UseCompatibilityLevel(120));
            _metadataContext = new MetadataContext(metadataOptionsBuilder.Options);

            DbContextOptionsBuilder<ServicesContext> servicesOptionsBuilder = new DbContextOptionsBuilder<ServicesContext>();
            servicesOptionsBuilder.UseSqlServer(_config.GetConnectionString("TCMConnection"), x => x.UseCompatibilityLevel(120));
            _servicesContext = new ServicesContext(servicesOptionsBuilder.Options);

            DbContextOptionsBuilder<MedListContext> medListOptionsBuilder = new DbContextOptionsBuilder<MedListContext>();
            medListOptionsBuilder.UseSqlServer(_config.GetConnectionString("TCMConnection"), x => x.UseCompatibilityLevel(120));
            _medListContext = new MedListContext(medListOptionsBuilder.Options);

            DbContextOptionsBuilder<HotLineContext> hlOptionsBuilder = new DbContextOptionsBuilder<HotLineContext>();
            hlOptionsBuilder.UseSqlServer(_config.GetConnectionString("TCMConnection"), x => x.UseCompatibilityLevel(120));
            _hotLineContext = new HotLineContext(hlOptionsBuilder.Options);

            DbContextOptionsBuilder<FinancialsContext> financialsOptionsBuilder = new DbContextOptionsBuilder<FinancialsContext>();
            financialsOptionsBuilder.UseSqlServer(_config.GetConnectionString("TCMConnection"), x => x.UseCompatibilityLevel(120));
            _financialsContext = new FinancialsContext(financialsOptionsBuilder.Options);

            _outputPath = _config.GetValue<string>("OutputPath");
            _programs = new List<int>();
            _config.GetSection("Parameters").GetSection("BHPrograms").Bind(_programs);
        }

        public void Run()
        {
            List<IMetadata> clients = new List<IMetadata>();
            clients.AddRange(_metadataContext.BH.Where(b => b.BHyn).ToList<IMetadata>());
            clients.AddRange(_metadataContext.BHFS.Where(b => b.BHyn || b.FSyn).ToList<IMetadata>());

            int left = clients.Count;

            foreach (IMetadata client in clients)
            {
                Process(client);
                left--;

                // Console.WriteLine(left);
            }
        }

        public void Process(IMetadata metadata)
        {
            ProcessNotes(metadata);

            ProcessHotline(metadata);

            ProcessMeds(metadata);
        }

        private void ProcessNotes(IMetadata metadata)
        {
            DateTime yearlyStartDate = new DateTime(1980, 1, 1);
            DateTime yearlyEndDate = new DateTime(2022, 1, 1).AddMilliseconds(-1);
            DateTime monthlyStartDate = new DateTime(2022, 1, 1);
            DateTime monthlyEndDate = new DateTime(2022, 4, 1).AddMilliseconds(-1);

            Domain.Services.Client client = _servicesContext.Clients
                                                            .Where(c => c.ClientId == metadata.ClientId)
                                                            .SingleOrDefault();

            if (client != null)
            {
                List<Contacts> contacts = GetContactsQuery(metadata.ClientId, yearlyStartDate, yearlyEndDate).ToList();

                if (contacts.Any())
                {
                    ClientNotesPdfDocument doc = new ClientNotesPdfDocument(_outputPath, _metadataContext);
                    doc.RenderYearly(client, contacts);
                }

                contacts = GetContactsQuery(metadata.ClientId, monthlyStartDate, monthlyEndDate).ToList();

                if (contacts.Any())
                {
                    ClientNotesPdfDocument doc = new ClientNotesPdfDocument(_outputPath, _metadataContext);
                    doc.RenderMonthly(client, contacts);
                }
            }

            metadata.ProcessedNotes = true;
            _metadataContext.SaveChanges();            
        }

        private IQueryable<Contacts> GetContactsQuery(int clientId, DateTime startDate, DateTime endDate)
        {
            return _servicesContext.Contacts
                            .Include(c => c.StaffEmployee)
                            .Include(c => c.ServiceCode)
                            .Include(c => c.ProgramLkp)
                            .Include(c => c.LocationLkp)
                            .Include(c => c.EntryStaffEmployee)
                            .Include(c => c.SignedByStaffEmployee)
                            .Include(c => c.ProgressNotes)
                            .Include(c => c.GroupProgressNotes)
                            .Where(c => c.ClientId == clientId
                                    && c.ServDate >= startDate
                                    && c.ServDate <= endDate)
                                    //&& _programs.Contains(c.Program))
                            .OrderBy(c => c.ServDate);
        }

        private void ProcessHotline(IMetadata metadata)
        {
            DateTime yearlyStartDate = new DateTime(1980, 1, 1);
            DateTime yearlyEndDate = new DateTime(2022, 1, 1).AddMilliseconds(-1);
            DateTime monthlyStartDate = new DateTime(2022, 1, 1);
            DateTime monthlyEndDate = new DateTime(2022, 4, 1).AddMilliseconds(-1);

            Domain.Services.Client client = _hotLineContext.clients
                                                            .Where(c => c.ClientId == metadata.ClientId)
                                                            .SingleOrDefault();

            List<HotLineHist> yearCalls = GetCallsQuery(metadata.ClientId, yearlyStartDate, yearlyEndDate).ToList();

            if (yearCalls.Any())
            {
                HotlinePdfDocument doc = new HotlinePdfDocument(_outputPath, _metadataContext);
                doc.RenderYearly(client, yearCalls);
            }

            List<HotLineHist> monthCalls = GetCallsQuery(metadata.ClientId, monthlyStartDate, monthlyEndDate).ToList();

            if (monthCalls.Any())
            {
                HotlinePdfDocument doc = new HotlinePdfDocument(_outputPath, _metadataContext);
                doc.RenderMonthly(client, monthCalls);
            }

            metadata.ProcessedHotline = true;
            _metadataContext.SaveChanges();
        }

        private IQueryable<HotLineHist> GetCallsQuery(int clientId, DateTime startDate, DateTime endDate)
        {
            return _hotLineContext.hotLineHists
                                  .Include(h => h.CallerRelationship)
                                  .Include(h => h.ClientAlert)
                                  .Include(h => h.ClientCaller)
                                  .Include(h => h.Comment)
                                  .Include(h => h.EmClientCaller)
                                  .Include(h => h.HLClientCaller)
                                  .Include(h => h.InsertStaff)
                                  .Include(h => h.Location)
                                  .Include(h => h.ProgramLkp)
                                  .Include(h => h.ProgressNotes)
                                  .Include(h => h.RespCounty)
                                  .Include(h => h.SignedByStaff)
                                  .Include(h => h.CallType)
                                  .Where(h => h.ClientId == clientId
                                  && h.CallDateTime >= startDate
                                    && h.CallDateTime <= endDate
                                    && _programs.Contains(h.Program.Value))
                                  .OrderByDescending(h => h.CallDateTime);
        }

        private void ProcessMeds(IMetadata metadata)
        {
            List<Medication> meds = _medListContext.Medications
                .Include(m => m.Pharmacy)
                .Include(m => m.Drug)
                .Include(m => m.MedApplication)
                .Include(m => m.MedExplanation)
                .Where(m => m.ClientId == metadata.ClientId)
                .OrderByDescending(h => h.StartDate)
                .ToList();

            if (meds.Any())
            {
                Domain.MedList.Client client = _medListContext.Clients
                                                              .Where(c => c.ClientId == metadata.ClientId)
                                                              .SingleOrDefault();

                MedInfo medInfo = _medListContext.MedInfos.Where(m => m.ClientId == metadata.ClientId).FirstOrDefault();

                MedListPdfDocument doc = new MedListPdfDocument(_outputPath, _metadataContext);
                doc.Render(client, meds, medInfo);

                metadata.ProcessedMedList = true;
                _metadataContext.SaveChanges();
            }

        }

    }
}
