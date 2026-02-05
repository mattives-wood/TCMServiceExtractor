using System;

namespace Domain.Financials;
public class Adjustment
{
    public Adjustment() { }

    public int ClientId { get; set; }
    public DateTime AdjustDate { get; set; }
    public decimal AdjustAmount { get; set; }
    public string Comment { get; set; }
    public int EntryStaffId { get; set; }
    public DateTime EntryDate { get; set; }
    public int KeyId { get; set; }
    public int AdjustCode { get; set; }
    public int ContactKeyId { get; set; }
    public int RespPartyId { get; set; }
    public DateTime PostDate { get; set; }
    public int PaymentAllocKey { get; set; }
    public int PaymentId { get; set; }
    public string Description { get; set; }
}
