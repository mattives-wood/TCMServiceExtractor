namespace Domain.Meta;
public interface IMetadata
{
    public int ClientId { get; set; }
    public bool BHyn { get; set; }
    public bool FSyn { get; set; }
    public bool ProcessedNotes { get; set; }

    public bool ProcessedHotline { get; set; }

    public bool ProcessedBiometrics { get; set; }

    public bool ProcessedMedList { get; set; }

    public bool ProcessedFinancials { get; set; }

}
