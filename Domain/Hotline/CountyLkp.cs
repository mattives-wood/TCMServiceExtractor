using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("CountyLkp")]

public class CountyLkp
{
    public CountyLkp() { }

    [Key]
    public int Code { get; set; }
    public string Description { get; set; }
}
