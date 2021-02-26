using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("Intakes")]
    public class Intakes
    {
        public Intakes() { }

        [Key]
        public int KeyId { get; set; }
        public int ClientId { get; set; }
        public int Program { get; set; }
        public int CaseMgr { get; set; }
        public DateTime? InitDate { get; set; }
        public DateTime? TermDate { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        [ForeignKey("Program")]
        public ProgramLkp ProgramLkp { get; set; }
        [ForeignKey("CaseMgr")]
        public Employees CaseMgrEmployee { get; set; }
        public List<Contacts> Contacts { get; set; }
    }
}
