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

        [ForeignKey("CaseMgr")]
        public List<Intakes> IntakesCaseMgr { get;set; }
        [ForeignKey("StaffId")]
        public List<Contacts> StaffContacts { get; set; }
        [ForeignKey("EntryStaffId")]
        public List<Contacts> EntryStaffContacts { get; set; }
        [ForeignKey("CreateStaffId")]
        public List<Contacts> CreateStaffContacts { get; set; }
        [ForeignKey("SignedByStaffId")]
        public List<Contacts> SignedByStaffContacts { get; set; }
    }
}
