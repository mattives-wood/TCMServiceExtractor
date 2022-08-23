using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Meta;

[Table("Metadata")]
public class Metadata
{
    [Key]
    public int LegacyDocumentId { get; set; }
    public int LegacyDocumentCodeId { get; set; }    
    public string LegacyDocumentName { get; set; }
    public int LegacyClientId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public string LegacyDocumentCategory { get; set; }
    public string PathToPdfFile { get; set; }
}
