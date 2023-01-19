using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

[Table("EmLocationLkp")]
public class EmLocationLkp
{
    public EmLocationLkp() { }

    [Key]
    public int Code { get; set; }
    public string Description { get; set; }
}
