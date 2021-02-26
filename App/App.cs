using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;

using Data;

using Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using PdfLib;

namespace App
{
    public class App
    {
        private readonly IConfiguration _config;
        private readonly Context _context;
        
        public App(IConfiguration configuration)
        {
            _config = configuration;
            DbContextOptionsBuilder<Context> optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("Connection"));
            _context = new Context(optionsBuilder.Options);
        }

        public void Run()
        {
            // List<Contacts> contacts = _context.Contacts.Where(c => c.ClientId == 117315)
            // .Include(c => c.Client)
            // .Include(c => c.ProgramLkp)
            // .Include(c => c.ServiceCode)
            // .Include(c => c.LocationLkp)
            // .Include(c => c.CreateStaffEmployee)
            // .Include(c => c.StaffEmployee)
            // .Include(c => c.EntryStaffEmployee)
            // .Include(c => c.SignedByStaffEmployee)
            // .Include(c => c.ProgressNotes)
            // .Include(c => c.Intake)
            // .ToList();
            // Console.ReadKey();

            Client client = _context.Clients
            .Include(c => c.Contacts)
            .ThenInclude(c => c.StaffEmployee)
            .Include(c => c.Contacts)
            .ThenInclude(c => c.ServiceCode)
            .Include(c => c.Contacts.Where(co => co.ServDate >= new DateTime(2021, 1, 1)))
            .ThenInclude(c => c.ProgressNotes)
            .Where(c => c.ClientId == 117315).FirstOrDefault();
            if (client == null)
            {
                throw new Exception();
            }

            PDFDocument pdf = new PDFDocument();
            pdf.GeneratePdf(client);
        }
    }
}
