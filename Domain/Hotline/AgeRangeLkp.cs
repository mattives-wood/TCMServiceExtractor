using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("AgeRangeLkp")]
public class AgeRangeLkp
{
    public AgeRangeLkp() { }

    [Key]
    public int Code { get; set; }
    public string? Description { get; set; }
}
