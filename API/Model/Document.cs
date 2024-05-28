namespace API.Model;

public class Document
{
    public int Id { get; set; }
    public string TypeCode { get; init; } = string.Empty;
    public string Nom { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string StatutCode { get; set; } = string.Empty;
    public int DemandeId { get; init; }
}