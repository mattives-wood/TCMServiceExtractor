using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Financials;
public class ServiceLineBalance
{
    public ServiceLineBalance() { }
    public int KeyId { get; set; }
    public decimal CurrentBalance { get; set; }
    public int ClientId { get; set; }
}
