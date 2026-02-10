using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("ClientAlert")]
public class ClientAlert
{
    public ClientAlert() { }

    [Key]
    public int AlertKeyId { get; set; }
    public string? Comment { get; set; }
}
