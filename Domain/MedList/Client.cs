using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.MedList;
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
    public List<Medication>? Medications { get; set; }
    public MedInfo? MedInfo { get; set; }
}
