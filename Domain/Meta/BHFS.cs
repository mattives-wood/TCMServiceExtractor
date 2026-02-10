using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Meta;

[Table("BHFS")]
public class BHFS : IMetadata
{
    [Key]
    public int ClientId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public DateTime? DOB { get; set; }

    public DateTime? BHActivityDate { get; set; }

    public string? BHActivityType { get; set; }

    public string? BHProgram { get; set; }

    public string? KeepBH { get; set; }

    public DateTime? FSActivityDate { get; set; }

    public string? FSActivityType { get; set; }

    public string? FSProgram { get; set; }

    public string? KeepFS { get; set; }

    [Column("FS")]
    public bool FSyn { get; set; }

    [Column("BH")]
    public bool BHyn { get; set; }

    public bool ProcessedNotes { get; set; }

    public bool ProcessedHotline { get; set; }

    public bool ProcessedBiometrics { get; set; }

    public bool ProcessedMedList { get; set; }

    public bool ProcessedFinancials { get; set; }
}