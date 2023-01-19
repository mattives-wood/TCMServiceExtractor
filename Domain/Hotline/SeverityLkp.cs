using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("SeverityLkp")]
public class SeverityLkp
{
    public SeverityLkp() { }

    [Key]
    public int Code { get; set; }
    public string Description { get; set; }
}
