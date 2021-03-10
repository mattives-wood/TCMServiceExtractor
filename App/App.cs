using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.XPath;

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
            int? ClientId = _config.GetSection("Parameters").GetValue<int?>("ClientId");
            DateTime StartDate = _config.GetSection("Parameters").GetValue<DateTime>("StartDate");
            DateTime EndDate = _config.GetSection("Parameters").GetValue<DateTime>("EndDate").AddDays(1).AddMilliseconds(-1);
            string mode = _config.GetSection("Parameters").GetValue<string>("Mode");

            PDFDocument.Mode Mode = mode switch
            {
                "S" => PDFDocument.Mode.Single,
                "D" => PDFDocument.Mode.Daily,
                "M" => PDFDocument.Mode.Monthly,
                "Y" => PDFDocument.Mode.Yearly,
                _ => throw new ArgumentException("Mode is incorrectly specified")
            };

            // Func<Client, bool> clientFunc = (Expression.Lambda<Func<Client, bool>>(body: Expression.Constant(true), parameters: Expression.Parameter(typeof(Client), "x"))).Compile();

            // if (ClientId != null) 
            // {
            // ParameterExpression param = Expression.Parameter(typeof(Client), "x");
            // MemberExpression member = Expression.Property(param, "ClientId");
            // Type propertyType = ((PropertyInfo)member.Member).PropertyType;
            // ConstantExpression constant = Expression.Constant(ClientId.Value);
            // Expression expression = Expression.Equal(member, constant);
            // Expression<Func<Client, bool>> funcExpression = Expression.Lambda<Func<Client, bool>>(body: expression, parameters: param);
            // clientFunc = funcExpression.Compile();
            // }

            // Func<Contacts, bool> startDateFunc = (Expression.Lambda<Func<Contacts, bool>>(body: Expression.Constant(true), parameters: Expression.Parameter(typeof(Contacts), "x"))).Compile();
            // Func<Contacts, bool> endDateFunc = (Expression.Lambda<Func<Contacts, bool>>(body: Expression.Constant(true), parameters: Expression.Parameter(typeof(Contacts), "x"))).Compile();
            // Expression < Func<Contacts, bool> > funcExpression = null;
            // if (StartDate != null)
            // {
            // ParameterExpression param = Expression.Parameter(typeof(Contacts), "x");
            // MemberExpression member = Expression.Property(param, "ServDate");
            // Type propertyType = ((PropertyInfo)member.Member).PropertyType;
            // ConstantExpression constant = Expression.Constant(StartDate, typeof(DateTime?));
            // Expression expression = Expression.GreaterThanOrEqual(member, constant);
            // funcExpression = Expression.Lambda<Func<Contacts, bool>>(body: expression, parameters: param);
            // startDateFunc = funcExpression.Compile();
            // }

            // if (EndDate != null)
            // {
            // ParameterExpression param = Expression.Parameter(typeof(Contacts), "x");
            // MemberExpression member = Expression.Property(param, "ServDate");
            // Type propertyType = ((PropertyInfo)member.Member).PropertyType;
            // ConstantExpression constant = Expression.Constant(EndDate, typeof(DateTime?));
            // Expression expression = Expression.LessThanOrEqual(member, constant);
            // Expression<Func<Contacts, bool>> funcExpression = Expression.Lambda<Func<Contacts, bool>>(body: expression, parameters: param);
            // endDateFunc = funcExpression.Compile();
            // }

            IQueryable<Client> query = _context.Clients
                                .Include(c => c.Contacts)
                                .ThenInclude(c => c.StaffEmployee)
                                .Include(c => c.Contacts)
                                .ThenInclude(c => c.ServiceCode)
                                .Include(c => c.Contacts)
                                .ThenInclude(c => c.ProgramLkp)
                                .Include(c => c.Contacts)
                                .ThenInclude(c => c.LocationLkp)
                                .Include(c => c.Contacts)
                                .ThenInclude(c => c.EntryStaffEmployee);

            if (StartDate != null || EndDate != null)
            {
                if (EndDate == null)
                {
                    query = query.Include(
                        c => c.Contacts.Where(co => co.ServDate >= StartDate).OrderBy(co => co.ServDate)).ThenInclude(co => co.ProgressNotes);
                }

                if (StartDate == null)
                {
                    query = query.Include(
                        c => c.Contacts.Where(co => co.ServDate <= EndDate).OrderBy(co => co.ServDate)).ThenInclude(co => co.ProgressNotes);
                }

                query = query.Include(
                        c => c.Contacts.Where(co => co.ServDate >= StartDate && co.ServDate <= EndDate).OrderBy(co => co.ServDate)).ThenInclude(co => co.ProgressNotes);
            }
            else
            {
                query = query.Include(
                        c => c.Contacts.OrderBy(co => co.ServDate)).ThenInclude(co => co.ProgressNotes);
            }

            if (ClientId != null)
            {
                query = query.Where(c => c.ClientId == ClientId);
            }

            query = query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName);

            List<Client> clients = query.ToList();

            if (clients.Count == 0)
            {
                throw new Exception();
            }

            Console.WriteLine($"Client count: {clients.Count()}");

            foreach (Client c in clients)
            {
                PDFDocument pdf = new PDFDocument();
                pdf.GeneratePdf(c, Mode);
            }
        }
    }
}
