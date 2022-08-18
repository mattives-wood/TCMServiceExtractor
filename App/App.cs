using Data;

using Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using PdfLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace App
{
    public class App
    {
        private readonly IConfiguration _config;
        private readonly Context _context;
        private readonly MetaContext _metaContext;
        private readonly string _outputPath;
        
        public App(IConfiguration configuration)
        {
            _config = configuration;
            DbContextOptionsBuilder<Context> optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("TCMConnection"));
            _context = new Context(optionsBuilder.Options);
            DbContextOptionsBuilder<MetaContext> metaOptionsBuilder = new DbContextOptionsBuilder<MetaContext>();
            metaOptionsBuilder.UseSqlServer(_config.GetConnectionString("MetadataConnection"));
            _metaContext = new MetaContext(metaOptionsBuilder.Options);
            _outputPath = _config.GetValue<string>("OutputPath");
        }

        public void Run()
        {            
            string metadataFile = _config.GetValue<string>("MetadataFile");
            List<int> programs = new List<int>();
            _config.GetSection("Parameters").GetSection("Programs").Bind(programs);

            //Get list of ClientIDs to process
            List<Domain.Meta.Client> clients = _metaContext.Clients.ToList();
            //List<Domain.Meta.Client> clients = _metaContext.Clients.Where(c => c.ClientId == 101424).ToList();
            float count = clients.Count;
            float done = 0;
            float percent = -1;
            foreach (Domain.Meta.Client metaClient in clients)
            {
                done++;
                Process(metaClient);
                float tempPercent = done / count * 100;
                if (tempPercent != percent)
                {
                    percent = tempPercent;
                    Console.WriteLine($"{percent.ToString("0.00")}%");
                }
            }
        }

        public void Process(Domain.Meta.Client metaClient)
        {
            DateTime yearlyStartDate = new DateTime(2000, 1, 1);
            DateTime yearlyEndDate = new DateTime(2020, 1, 1).AddMilliseconds(-1);
            DateTime monthlyStartDate = new DateTime(2020, 1, 1);
            DateTime monthlyEndDate = new DateTime(2021, 3, 1).AddMilliseconds(-1);
            DateTime dailyStartDate = new DateTime(2021, 3, 1);
            DateTime dailyEndDate = new DateTime(2022, 1, 1).AddMilliseconds(-1);
            DateTime singleStartDate = new DateTime(2022, 1, 1);
            DateTime singleEndDate = new DateTime(2022, 3, 1).AddMilliseconds(-1);
            bool processed = false;

            Client client = _context.Clients.Where(c => c.ClientId == metaClient.ClientId).SingleOrDefault();

            if (client != null)
            {
                List<Contacts> contacts = GetContactsQuery(metaClient.ClientId, yearlyStartDate, yearlyEndDate).ToList();

                if (contacts.Any())
                {
                    PDFDocument doc = new PDFDocument(_outputPath);
                    doc.RenderYearly(client, contacts);
                    processed = true;
                }

                contacts = GetContactsQuery(metaClient.ClientId, monthlyStartDate, monthlyEndDate).ToList();

                if (contacts.Any())
                {
                    PDFDocument doc = new PDFDocument(_outputPath);
                    doc.RenderMonthly(client, contacts);
                    processed = true;
                }

                contacts = GetContactsQuery(metaClient.ClientId, dailyStartDate, dailyEndDate).ToList();

                if (contacts.Any())
                {
                    PDFDocument doc = new PDFDocument(_outputPath);
                    doc.RenderDaily(client, contacts);
                    processed = true;
                }

                contacts = GetContactsQuery(metaClient.ClientId, singleStartDate, singleEndDate).ToList();

                if (contacts.Any())
                {
                    PDFDocument doc = new PDFDocument(_outputPath);
                    doc.RenderSingle(client, contacts);
                    processed = true;
                }
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
                                    && c.ServDate <= endDate)
                            .OrderBy(c => c.ServDate);
        }
    }
}
