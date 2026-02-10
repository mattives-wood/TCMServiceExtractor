using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Services;

[Table("EmLocationLkp")]
public class EmLocationLkp
{
    public EmLocationLkp() { }

    [Key]
    public int Code { get; set; }
    public required string Description { get; set; }
}
