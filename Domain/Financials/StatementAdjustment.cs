using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Financials;

public class StatementAdjustment
{
    public StatementAdjustment() { }

    public int KeyId { get; set; }

    public int ContactKeyId { get; set; }

    public decimal AdjustAmount { get; set; }
}
