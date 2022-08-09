using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Meta;

[Table("Client")]
public class Client
{
    public Client() { }

    [Key]
    public int ClientId { get; set; }
    public bool Processed { get; set; }
}
