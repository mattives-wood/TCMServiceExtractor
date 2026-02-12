using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Financials;

[Table("ArStatements")]
public class Statement
{
    public Statement() { }

    [Key]
    public int StatementId { get; set; }

    public int RespPartyId { get; set; }

    public DateTime StatementDate { get; set; }

    public decimal BalForward { get; set; }

    public decimal Atp { get; set; }

    public decimal TotalDue { get; set; }

    public decimal TotalDue0To30 { get; set; }

    public decimal TotalDue31To60 { get; set; }

    public decimal TotalDue61To90 { get; set; }

    public decimal TotalDueOver90 { get; set; }

    public decimal PaymentPlanAmountDue { get; set; }

    [NotMapped]
    public ResponsibleParty? ResponsibleParty { get; set; }

    [NotMapped]
    public List<StatementLineItem> StatementLineItems { get; set; }
}