using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.MedList;
[Table("Pharmacy")]
public class Pharmacy
{
    public Pharmacy() { }
    [Key]
    public int PharmacyId { get; set; }
    public string Name { get; set; }
}
