using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Services
{
    [Table("Client")]
    public class Client
    {
        public Client() { }

        [Key]
        public int ClientId { get; set; }
        public required string LastName { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleInitial { get; set; }
        public string? LastFirstName { get; set; }
    }
}
