using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Financials;

public class StatementLineItem
{
    public StatementLineItem() { }

    public int ServiceLineId { get; set; }

    public int ContactId { get; set; }

    public int ClientId {  get; set; }

    public int RespPartyId { get; set; }

    public int StatementId { get; set; }

    public DateTime ContactServDate { get; set; }

    public decimal Amount { get; set; }

    public decimal AtpOverride { get; set; }

    public int Activity {  get; set; }

    public decimal Cost { get; set; }

    public string? Description { get; set; }

    [NotMapped]
    public List<StatementAdjustment> StatementAdjustments { get; set; }

    [NotMapped]
    public List<StatementPayment> StatementPayments { get; set; }
}
