using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("RelationshipLkp")]
public class RelationshipLkp
{
    public RelationshipLkp() { }

    [Key]
    public int Code { get; set; }
    public string Description { get; set; }
}
