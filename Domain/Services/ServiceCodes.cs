using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Services
{
    [Table("ServiceCodes")]
    public class ServiceCodes
    {
        public ServiceCodes() { }

        [Key]
        public int Code { get; set; }
        public required string Description { get; set; }
    }
}
