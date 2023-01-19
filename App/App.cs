using Data;

using Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using PdfLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace App
{
    public class App
    {
        private readonly IConfiguration _config;
        private readonly Context _context;
        private readonly MetaContext _metaContext;
        private readonly HotLineContext _hotLineContext;
        private readonly string _outputPath;
        private readonly List<int> _programs;
        
        public App(IConfiguration configuration)
        {
            _config = configuration;
            DbContextOptionsBuilder<Context> optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("TCMConnection"));
            _context = new Context(optionsBuilder.Options);
            DbContextOptionsBuilder<HotLineContext> HLOptionsBuilder = new DbContextOptionsBuilder<HotLineContext>();
            HLOptionsBuilder.UseSqlServer(_config.GetConnectionString("TCMConnection"));
            _hotLineContext = new HotLineContext(HLOptionsBuilder.Options);
            DbContextOptionsBuilder<MetaContext> metaOptionsBuilder = new DbContextOptionsBuilder<MetaContext>();
            metaOptionsBuilder.UseSqlServer(_config.GetConnectionString("MetadataConnection"));
            _metaContext = new MetaContext(metaOptionsBuilder.Options);
            _outputPath = _config.GetValue<string>("OutputPath");
            _programs = new List<int>();
            _config.GetSection("Parameters").GetSection("Programs").Bind(_programs);
        }

        public void Run()
        {
            //Get list of ClientIDs to process
            List<Domain.Meta.Client> clients = _metaContext.Clients.ToList();
            int left = clients.Count;

            foreach (Domain.Meta.Client metaClient in clients)
            {
                Process(metaClient);
                left--;
                Console.WriteLine(left);
            }
        }

        public void Process(Domain.Meta.Client metaClient)
        {
            DateTime yearlyStartDate = new DateTime(2000, 1, 1);
            DateTime yearlyEndDate = new DateTime(2021, 9, 1).AddMilliseconds(-1);
            DateTime monthlyStartDate = new DateTime(2021, 9, 1);
            DateTime monthlyEndDate = new DateTime(2022, 3, 1).AddMilliseconds(-1);
            //DateTime dailyStartDate = new DateTime(2021, 3, 1);
            //DateTime dailyEndDate = new DateTime(2022, 1, 1).AddMilliseconds(-1);
            //DateTime singleStartDate = new DateTime(2022, 1, 1);
            //DateTime singleEndDate = new DateTime(2022, 3, 1).AddMilliseconds(-1);
            bool processed = false;

            Client client = _context.Clients.Where(c => c.ClientId == metaClient.ClientId).SingleOrDefault();

            if (client != null)
            {
                List<Contacts> contacts = GetContactsQuery(metaClient.ClientId, yearlyStartDate, yearlyEndDate).ToList();

                if (contacts.Any())
                {
                    PDFDocument doc = new PDFDocument(_outputPath, _metaContext);
                    doc.RenderYearly(client, contacts);
                    processed = true;
                }

                contacts = GetContactsQuery(metaClient.ClientId, monthlyStartDate, monthlyEndDate).ToList();

                if (contacts.Any())
                {
                    PDFDocument doc = new PDFDocument(_outputPath, _metaContext);
                    doc.RenderMonthly(client, contacts);
                    processed = true;
                }

                //contacts = GetContactsQuery(metaClient.ClientId, dailyStartDate, dailyEndDate).ToList();

                //if (contacts.Any())
                //{
                //    PDFDocument doc = new PDFDocument(_outputPath, _metaContext);
                //    doc.RenderDaily(client, contacts);
                //    processed = true;
                //}

                //contacts = GetContactsQuery(metaClient.ClientId, singleStartDate, singleEndDate).ToList();

                //if (contacts.Any())
                //{
                //    PDFDocument doc = new PDFDocument(_outputPath, _metaContext);
                //    doc.RenderSingle(client, contacts);
                //    processed = true;
                //}
            }

            metaClient.Processed = processed;

            _metaContext.SaveChanges();
        }

        private IQueryable<Contacts> GetContactsQuery(int clientId, DateTime startDate, DateTime endDate)
        {
            return _context.Contacts
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
                                    && c.ServDate <= endDate
                                    && _programs.Contains(c.Program))
                            .OrderBy(c => c.ServDate);
        }
    }
}
