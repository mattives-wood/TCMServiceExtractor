using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Services
{
    [Table("Contacts")]
    public class Contacts
    {
        public Contacts() { }

        [Key]
        public int KeyId { get; set; }
        public int? ProgressNoteKey { get; set; }
        public int? IntakeKey { get; set; }
        public int ClientId { get; set; }
        public int? StaffId { get; set; }
        public DateTime? ServDate { get; set; }
        public int Mileage { get; set; }
        public decimal TimeSpent { get; set; }
        public int? EntryStaffId { get; set; }
        public required string ClientLast { get; set; }
        public required string ClientFirst { get; set; }
        public DateTime? ClientDOB { get; set; }
        public int? Activity { get; set; }
        public int? Location { get; set; }
        public int Program { get; set; }
        public int? CreateStaffId { get; set; }
        public string? CreateName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? ProviderLast { get; set; }
        public string? ProviderFirst { get; set; }
        public decimal TravelTime { get; set; }
        public int? ContactSigned { get; set; }
        public decimal FaceToFace { get; set; }
        public decimal OtherContactType { get; set; }
        public decimal Collateral { get; set; }
        public decimal RecordKeeping { get; set; }
        public decimal Support { get; set; }
        public decimal Travel { get; set; }
        public decimal BillableTime { get; set; }
        public int? SignedByStaffId { get; set; }
        public int? GroupNoteKey { get; set; }
        public DateTime? SignedByDate { get; set; }
        [ForeignKey("ClientId")]
        public required Client Client { get; set; }
        [ForeignKey("Program")]
        public ProgramLkp? ProgramLkp { get; set; }
        [ForeignKey("ProgressNoteKey")]
        public ProgressNotes? ProgressNotes { get; set; }
        [ForeignKey("GroupNoteKey")]
        public ProgressNotes? GroupProgressNotes { get; set; }
        [ForeignKey("IntakeKey")]
        public Intakes? Intake { get; set; }
        [ForeignKey("Location")]
        public LocationLkp? LocationLkp { get; set; }
        [ForeignKey("Activity")]
        public ServiceCodes? ServiceCode { get; set; }
        [ForeignKey("StaffId")]
        public Employees? StaffEmployee { get; set; }
        [ForeignKey("EntryStaffId")]
        public Employees? EntryStaffEmployee { get; set; }
        [ForeignKey("CreateStaffId")]
        public Employees? CreateStaffEmployee { get; set; }
        [ForeignKey("SignedByStaffId")]
        public Employees? SignedByStaffEmployee { get; set; }

    }
}
