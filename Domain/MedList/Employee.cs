using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.MedList;
[Table("Employees")]
public class Employee
{
    public Employee() { }

    [Key]
    public int StaffId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
}