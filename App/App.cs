using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Linq.Dynamic.Core;

using Data;

using Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
            //ExpressionPredicateBuilder.Test test = new ExpressionPredicateBuilder.Test();
            //test.Run();

            int? ClientId = _config.GetSection("Parameters").GetValue<int?>("ClientId");
            DateTime StartDate = _config.GetSection("Parameters").GetValue<DateTime>("StartDate");
            DateTime EndDate = _config.GetSection("Parameters").GetValue<DateTime>("EndDate").AddDays(1).AddMilliseconds(-1);
            string mode = _config.GetSection("Parameters").GetValue<string>("Mode");
            string outputPath = _config.GetValue<string>("OutputPath");
            string metadataFile = _config.GetValue<string>("MetadataFile");
            List<int> programs = new List<int>();
            _config.GetSection("Parameters").GetSection("Programs").Bind(programs);

            PDFDocument.Mode Mode = mode switch
            {
                "S" => PDFDocument.Mode.Single,
                "D" => PDFDocument.Mode.Daily,
                "M" => PDFDocument.Mode.Monthly,
                "Y" => PDFDocument.Mode.Yearly,
                _ => throw new ArgumentException("Mode is incorrectly specified")
            };

            //IQueryable<Client> query = _context.Clients;
                                //.Include(c => c.Contacts)
                                //.ThenInclude(c => c.StaffEmployee)
                                //.Include(c => c.Contacts)
                                //.ThenInclude(c => c.ServiceCode)
                                //.Include(c => c.Contacts)
                                //.ThenInclude(c => c.ProgramLkp)
                                //.Include(c => c.Contacts)
                                //.ThenInclude(c => c.LocationLkp)
                                //.Include(c => c.Contacts)
                                //.ThenInclude(c => c.EntryStaffEmployee)
                                //.Include(c => c.Contacts)
                                //.ThenInclude(c => c.SignedByStaffEmployee);

            //MethodInfo methodInfo = typeof(List<int>).GetMethod("Contains", new Type[] { typeof(int) });
            //ConstantExpression progs = Expression.Constant(programs);
            ParameterExpression param = Expression.Parameter(typeof(Contacts), "x");
            //MemberExpression programMember = Expression.Property(param, "Program");
            MemberExpression member = Expression.Property(param, "ServDate");
            //Type propertyType = ((PropertyInfo)member.Member).PropertyType;
            ConstantExpression constant = Expression.Constant(StartDate, typeof(DateTime?));
            // Expression expression1 = Expression.GreaterThanOrEqual(member, constant);
            // constant = Expression.Constant((DateTime?)EndDate);
            // Expression expression2 = Expression.LessThanOrEqual(member, constant);
            // Expression expression3 = Expression.Call(progs, methodInfo, programMember);
            Expression expression3 = Expression.LessThanOrEqual(member, constant);
            Expression<Func<Contacts, bool>> funcExpression = Expression.Lambda<Func<Contacts, bool>>(expression3, param);
            var func = funcExpression.Compile();

            // query = query.Include(c => c.Contacts.Where(func));

            // if (StartDate != null || EndDate != null)
            // {
            // if (EndDate == null)
            // {
            // query = query.Include(
            // c => c.Contacts.Where(co => co.ServDate >= StartDate && programs.Contains(co.Program)).OrderBy(co => co.ServDate)).ThenInclude(co => co.ProgressNotes);
            // }

            // if (StartDate == null)
            // {
            // query = query.Include(
            // c => c.Contacts.Where(co => co.ServDate <= EndDate && programs.Contains(co.Program)).OrderBy(co => co.ServDate)).ThenInclude(co => co.ProgressNotes);
            // }

            // query = query.Include(
            // c => c.Contacts.Where(co => co.ServDate >= StartDate && co.ServDate <= EndDate && programs.Contains(co.Program)).OrderBy(co => co.ServDate)).ThenInclude(co => co.ProgressNotes);
            // }
            // else
            // {
            // query = query.Include(
            // c => c.Contacts.Where(co => programs.Contains(co.Program)).OrderBy(co => co.ServDate)).ThenInclude(co => co.ProgressNotes);
            // }

            // if (ClientId != null)
            // {
            // query = query.Where(c => c.ClientId == ClientId);
            // }

            // query = query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName);
            // var query = _context.Clients.Include(c => c.Contacts.Where(x => x.ServDate <= StartDate));
            var query = _context.Clients.Include(c => c.Contacts.AsQueryable<Contacts>().Where<Contacts>(co => co.ServDate >= StartDate));
            List<Client> clients = query.ToList();
            //List<Client> clients = query.ToList().Where(c => c.Contacts.Any()).ToList();

            //if (clients.Count == 0)
            //{
            //    throw new Exception();
            //}

            //Console.WriteLine($"Client count: {clients.Count()}");

            //foreach (Client c in clients)
            //{
            //    PDFDocument pdf = new PDFDocument(outputPath, metadataFile);
            //    pdf.GeneratePdf(c, Mode);
            //}
        }
    }
}
