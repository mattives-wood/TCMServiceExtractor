using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Services
{
    [Table("Employees")]
    public class Employees
    {
        public Employees() { }

        [Key]
        public int StaffId { get; set; }
        public required string LastName { get; set; }
        public required string FirstName { get; set; }
    }
}
