using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Services
{
    [Table("ProgressNotes")]
    public class ProgressNotes
    {
        public ProgressNotes() { }

        [Key]
        public int NoteKey { get; set; }
        public string ProgressNote { get; set; }
        public DateTime? DateSigned { get; set; }
    }
}
