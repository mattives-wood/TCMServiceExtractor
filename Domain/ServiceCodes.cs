using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("ServiceCodes")]
    public class ServiceCodes
    {
        public ServiceCodes() { }

        [Key]
        public int Code { get; set; }
        public string Description { get; set; }
        [ForeignKey("Activity")]
        public List<Contacts> Contacts { get; set; }
    }
}
