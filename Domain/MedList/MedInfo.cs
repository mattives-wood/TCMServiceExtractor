using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.MedList;

[Table("MedInfo")]
public class MedInfo
{
    public MedInfo() { }
    [Key]
    public int ClientId { get; set; }

    public string Physician { get; set; }

    public string Allergies { get; set; }

    public int PharmacyId { get; set; }

    public Pharmacy Pharmacy { get; set; }

    public string OtherMedOrdersText { get; set; }

    public string MedAlerts { get; set; }

    public int PhysicianCode { get; set; }

    [ForeignKey("PhysicianCode")]
    public Employee PhysicianCodeEmployee { get; set; }
}