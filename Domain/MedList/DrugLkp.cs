using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.MedList;
[Table("DrugLkp")]
public class DrugLkp
{
    public DrugLkp() { }
    [Key]
    public int Code { get; set; }
    public string? Generic {  get; set; }
    public string? Brand { get; set; }
    public string? Combined { get; set; }
}
