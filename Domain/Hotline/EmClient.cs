using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("EmClient")]
public class EmClient
{
	public EmClient() {}

	[Key]
	public int ClientId { get; set; }
	public required string LastName { get; set; }
	public required string FirstName { get; set; }
	public string? MiddleInitial { get; set; }

}
