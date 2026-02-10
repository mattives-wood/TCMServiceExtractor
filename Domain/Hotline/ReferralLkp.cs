using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("ReferralLkp")]
public class ReferralLkp
{
    public ReferralLkp() { }

    [Key]
    public int Code { get; set; }
    public string? Description { get; set; }
}
