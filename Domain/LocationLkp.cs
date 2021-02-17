using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("LocationLkp")]
    public class LocationLkp
    {
        public LocationLkp() { }
        
        [Key]
        public int Code { get; set; }
        public string Description { get; set; }
        [ForeignKey("Location")]
        public List<Contacts> Contacts { get; set; }
    }
}
