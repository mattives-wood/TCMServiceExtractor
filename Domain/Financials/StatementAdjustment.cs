using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Financials;

[Table("ArActAdjustments")]
public class StatementAdjustment
{
    public StatementAdjustment() { }

    [Key]
    public int KeyId { get; set; }

    public int ContactKeyId { get; set; }

    public decimal AdjustAmount { get; set; }
}
