using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("EmCallTypeLkp")]
public class EmCallTypeLkp
{
    public EmCallTypeLkp() { }

    [Key]
    public int Code { get; set; }
    public required string Description { get; set; }
}
