using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Financials;
public class Payment
{
    public Payment() { }
    public DateTime PaymentDate { get; set; }
    public string? Description { get; set; }
    public decimal AllocatedAmount { get; set; }
    public int KeyId { get; set; }
    public int Batchkey { get; set; }
    public int ClientId { get; set; }
    public string? ReceiptNumber { get; set; }
    public int ContactKeyId { get; set; }
}
