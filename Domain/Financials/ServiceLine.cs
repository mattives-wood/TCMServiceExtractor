using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Financials;
public class ServiceLine
{
    public ServiceLine() { }
    public int KeyId { get; set; }
    public int ClientId { get; set; }
    public DateTime ServDate { get; set; }
    public string LastFirstName { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public decimal CurrentBalance { get; set; }
    public DateTime StatementPostDate { get; set; }
    public string LastFirst {  get; set; }

}
