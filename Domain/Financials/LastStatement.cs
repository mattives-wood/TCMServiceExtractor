using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Financials;
public class LastStatement
{
    public LastStatement() { }
    public int ClientId { get; set; }
    public DateTime StatementDate { get; set; }
    public decimal TotalDue { get; set; }
}
