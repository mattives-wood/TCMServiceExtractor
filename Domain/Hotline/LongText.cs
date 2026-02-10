using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("LongText")]
public class LongText
{
    public LongText() { }

    [Key]
    public int TextId { get; set; }
    public string? TextBlob { get; set; }
}
