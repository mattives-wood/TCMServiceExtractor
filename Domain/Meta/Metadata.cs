using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Meta;

[Table("Metadata")]
public class Metadata
{
    [Key]
    public Guid FileNameGuid { get; set; }
    public int ClientId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int? DocumentMonth {  get; set; }
    public int? DocumentYear { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ServiceDateTime { get; set; }
    public string? DocumentType { get; set; }
    public string? Program { get; set; }
}
