using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.MedList;
[Table("Employees")]
public class Employee
{
    public Employee() { }

    [Key]
    public int StaffId { get; set; }
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
}