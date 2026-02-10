using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Services;

namespace Domain.Biometrics;

[Table("stVitalSigns")]
public class VitalSign
{
    public VitalSign() { }

    [Key]
    public int Seq {  get; set; }
    public int ClientId { get; set; }
    public required Client Client { get; set; }
    public int AssessorId { get; set; }
    public Employees? Assessor {  get; set; }
    public DateTime AsmtDate { get; set; }
    [Column("Q1")]
    public int BPStanding { get; set; }
    [Column("Q2")]
    public int BPSitting { get; set; }
    [Column("Q3")]
    public int BPLyingDown { get; set; }
    [Column("Q4")]
    public int Heartrate { get; set; }
    [Column("Q5")]
    public int Respiration { get; set; }
    [Column("Q6")]
    public int Temperature { get; set; }
    [Column("Q7")]
    public int Weight { get; set; }
}
