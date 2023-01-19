using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Hotline;

[Table("HotLineHist")]
public class HotLineHist
{
    public HotLineHist() { }

    //Common
    [Key]
    public int KeyId { get; set; }
    public int ClientId { get; set; }
    [ForeignKey("ClientId")]
    public Client Client { get; set; }
    [ForeignKey("ClientId")]
    public EmClient EmClient { get; set; }
    [ForeignKey("ClientId")]
    public HotLineClient HotLineClient { get; set; }
    public string ClientTable { get; set; }
    public int CallerId { get; set; }
    [ForeignKey("CallerId")]
    public Client ClientCaller { get; set; }
    [ForeignKey("CallerId")]
    public EmClient EmClientCaller { get; set; }
    [ForeignKey("CallerId")]
    public Employees StaffCaller { get; set; }
    [ForeignKey("CallerId")]
    public HotLineClient HLClientCaller { get; set; }
    public string CallerTable { get; set; }
    public int StaffId { get; set; }
    [ForeignKey("StaffId")]
    public Employees Staff { get; set; }
    public int ReferralSource { get; set; }
    [ForeignKey("ReferralSource")]
    public ReferralLkp ReferralSrc { get; set; }
    public int CallTypeCode { get; set; }
    [ForeignKey("CallTypeCode")]
    public EmCallTypeLkp CallType { get; set; }
    public int Program { get; set; }
    [ForeignKey("Program")]
    public ProgramLkp ProgramLkp { get; set; }
    public int LocationCode { get; set; }
    [ForeignKey("LocationCode")]
    public EmLocationLkp Location { get; set; }
    public int CallerAgeRangeCode { get; set; }
    [ForeignKey("CallerAgeRangeCode")]
    public AgeRangeLkp CallerAgeRange { get; set; }
    public int SeverityCode { get; set; }
    [ForeignKey("SeverityCode")]
    public SeverityLkp Severity { get; set; }
    public int AnonymousCaller { get; set; }
    public DateTime CallDateTime { get; set; }
    public int ProgressNoteKey { get; set; }
    [ForeignKey("ProgressNoteKey")]
    public ProgressNotes ProgressNotes { get; set; }
    public int CallMinutes { get; set; }
    public int CommentsTextId { get; set; }
    [ForeignKey("CommentsTextId")]
    public LongText Comment { get; set; }
    public DateTime? SignedDate { get; set; }
    public int SignedByStaffId { get; set; }
    [ForeignKey("SignedByStaffId")]
    public Employees SignedByStaff { get; set; }
    public DateTime InsertDate { get; set; }
    public int InsertStaffId { get; set; }
    [ForeignKey("InsertStaffId")]
    public Employees InsertStaff { get; set; }
    public int ADRCVersion { get; set; }

    //Version 1
    public int ClientAlertKeyId { get; set; }
    [ForeignKey("ClientAlertKeyId")]
    public ClientAlert ClientAlert { get; set; }
    public int ConsultClientId { get; set; }
    [ForeignKey("ConsultClientId")]
    public Client ConsultClient { get; set; }


    //Version 2
    public string AnonymousCallerFirstName { get; set; }
    public string AnonymousClientFirstName { get; set; }
    public int CallerRelationshipCode { get; set; }
    [ForeignKey("CallerRelationshipCode")]
    public RelationshipLkp CallerRelationship { get; set; }
    public int RespCountyCode { get; set; }
    [ForeignKey("RespCountyCode")]
    public CountyLkp RespCounty { get; set; }
    public int ClientAgeRangeCode { get; set; }
    [ForeignKey("ClientAgeRangeCode")]
    public AgeRangeLkp ClientAgeRange { get; set; }
    public int AnonymousClient { get; set; }
    public decimal FaceToFace { get; set; }
    public decimal OtherContactType { get; set; }
    public decimal Collateral { get; set; }
    public decimal RecordKeeping { get; set; }
    public decimal Support { get; set; }
    public decimal Travel { get; set; }
    public decimal DistributedTotalTime { get; set; }
    public decimal DistributedBillableTime { get; set; }
    public string DistributedTimeUnitType { get; set; }
    public int RelationshipSelf { get; set; }
}
