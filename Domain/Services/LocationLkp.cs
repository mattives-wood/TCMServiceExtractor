using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Services
{
    [Table("LocationLkp")]
    public class LocationLkp
    {
        public LocationLkp() { }
        
        [Key]
        public int Code { get; set; }
        public string? Description { get; set; }
    }
}
