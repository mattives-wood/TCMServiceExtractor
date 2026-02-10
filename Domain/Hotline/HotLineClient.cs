using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("HotLineClient")]
public class HotLineClient
{
	public HotLineClient() { }

	[Key]
	public int HLClientId { get; set; }
	public string? LastName { get; set; }
	public string? FirstName { get; set; }
}
