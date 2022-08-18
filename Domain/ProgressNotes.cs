using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("ProgressNotes")]
    public class ProgressNotes
    {
        public ProgressNotes() { }

        [Key]
        public int NoteKey { get; set; }
        //public int ClientId { get; set; }
        public string ProgressNote { get; set; }
        public DateTime? DateSigned { get; set; }
        //public int ContactKeyId { get; set; }
        //[ForeignKey("ClientId")]
        //public Client Client { get; set; }
        //[ForeignKey("ContactKeyId")]
        //public Contacts Contact { get; set; }
    }
}
