using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.MedList;
[Table("MedExplanationLkp")]
public class MedExplanationLkp
{
    public MedExplanationLkp() { }
    [Key]
    public int Code { get; set; }
    public string Description { get; set; }
}
