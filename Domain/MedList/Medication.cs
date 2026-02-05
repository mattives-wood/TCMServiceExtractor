using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.MedList;

[Table("Medications")]
public class Medication
{
    public Medication() { }
    [Key]
    public int KeyId { get; set; }

    public int ClientId { get; set; }

    public int? MedCode { get; set; }
    [ForeignKey("MedCode")]
    public DrugLkp? Drug {  get; set; }

    public string MedDosage { get; set; }

    public string Frequency { get; set; }

    public int Refills { get; set; }

    public string Comment { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Tablets { get; set; }

    public string Physician { get; set; }

    public string DispenseTime { get; set; }

    public string Application { get; set; }

    public string MedType { get; set; }

    public int? PrescribingPhysicianId { get; set; }

    public Employee? PrescribingPhysician { get; set; }

    public int MedCatDsplyType { get; set; }

    public int PharmacyId { get; set; }

    public Pharmacy Pharmacy { get; set; }

    public DateTime? OrderedDate { get; set; }

    public int? ExplanationCode { get; set; }

    [ForeignKey("ExplanationCode")]
    public MedExplanationLkp? MedExplanation { get; set; }

    public int? ApplicationCode { get; set; }

    [ForeignKey("ApplicationCode")]
    public MedApplicationLkp? MedApplication { get; set; }

    public DateTime? ConsentDate { get; set; }
}