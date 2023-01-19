using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("EmClient")]
public class EmClient
{
	public EmClient() {}

	[Key]
	public int ClientId { get; set; }
	public string LastName { get; set; }
	public string FirstName { get; set; }
	public string MiddleInitial { get; set; }

}
