using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Financials;

    public class ResponsibleParty
{
    public ResponsibleParty() { }

    public int RespPartyId { get; set; }

    public int ClientId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleInitial { get; set; }

    public string? BillingAddress { get; set; }

    public string? BillingCity { get; set; }

    public string? BillingState { get; set; }

    public string? BillingZip { get; set; }

    public string? BillingZip2 { get; set; }

    public string? BillingPhone1 { get; set; }

    public string? BillingPhone2 { get; set; }

    public string? Comment { get; set; }

    public int RelationshipCode { get; set; }

    public string? Relationship { get; set; }

    public string? PartyLastName { get; set; }

    public string? PartyFirstName { get; set; }

    public string? PartyMiddleInitial { get; set; }

    public string? PartyAddress { get; set; }

    public string? PartyCity { get; set; }

    public string? PartyState { get; set; }

    public string? PartyZip { get; set; }

    public string? PartyZip2 { get; set; }

    public string? PartyPhone { get; set; }

    public string? PartyPhone2 { get; set; }

    public string? PartySSN { get; set; }

    public string? PartyBillingLastName { get; set; }

    public string? PartyBillingFirstName { get; set; }

    public string? PartyBillingAddress { get; set; }

    public string? PartyBillingCity { get; set; }

    public string? PartyBillingState { get; set; }

    public string? PartyBillingZip { get; set; }

    public string? PartyBillingZip2 { get; set; }

    public string? PartyBillingComment { get; set; }

    public string? PartyOrganization { get; set; }
}
