using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Financials;

public class StatementPayment
{
    public StatementPayment() { }

    public int PaymentAllocId { get; set; }

    public int ContactKeyId { get; set; }

    public decimal AllocatedAmount { get; set; }

    public int StatementId { get; set; }  //Match on this for each statement


    //Discount?
}
