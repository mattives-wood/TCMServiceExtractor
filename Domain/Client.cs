using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("Client")]
    public class Client
    {
        public Client() { }

        [Key]
        public int ClientId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastFirstName { get; set; }

        public List<Intakes> Intakes { get; set; }
        public List<Contacts> Contacts { get; set; }
        public List<ProgressNotes> ProgressNotes { get; set; }
    }
}
