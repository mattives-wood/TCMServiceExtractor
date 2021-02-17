using System.Collections.Generic;
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

        public List<Intakes> IntakesCaseMgr { get;set; }
        public List<Contacts> StaffContacts { get; set; }
        public List<Contacts> EntryStaffContacts { get; set; }
        public List<Contacts> CreateStaffContacts { get; set; }
        public List<Contacts> SignedByStaffContacts { get; set; }
    }
}
