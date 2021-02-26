using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("Employees")]
    public class Employees
    {
        public Employees() { }

        [Key]
        public int StaffId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
