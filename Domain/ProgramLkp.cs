using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("ProgramLkp")]
    public class ProgramLkp
    {
        public ProgramLkp() { }

        [Key]
        public int Code { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
    }
}
