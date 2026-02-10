using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Meta;

[Table("BH")]
public class BH : IMetadata
{
    public BH() { }

    [Key]
    public int ClientId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public DateTime? DOB { get; set; }

    public DateTime? BHActivityDate { get; set; }

    public string? BHActivityType { get; set; }

    public string? BHProgram { get; set; }

    public string? KeepBH { get; set; }

    [Column("BH")]
    public bool BHyn { get; set; }

    [NotMapped]
    public bool FSyn { get { return false; } set { } }

    public bool ProcessedNotes { get; set; }

    public bool ProcessedHotline { get; set; }

    public bool ProcessedBiometrics { get; set; }

    public bool ProcessedMedList { get; set; }

    public bool ProcessedFinancials { get; set; }
}