using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Financials;
public class Claim
{
    public Claim() { }
    public int ClientId { get; set; }
    public int? ClaimNumber { get; set; }
    public int? ClaimTypeCode { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? ClaimDate { get; set; }
    public string? Comment { get; set; }
    public DateTime? CreationDateTime { get; set; }
    public int? InsCode { get; set; }
    public int? Program { get; set; }
    public DateTime? ServDate { get; set; }
    public DateTime? ClaimCompleteDate { get; set; }
    public decimal? TotalCharge { get; set; }
    public decimal? AmountPaid { get; set; }
    public decimal ?BalanceDue { get; set; }
    public string? ClaimTypeDesc { get; set; }
    public string? InsDesc { get; set; }
    public int ContactKeyId { get; set; }
}
