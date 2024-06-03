namespace FileSafetyService;

public record FileMessage
{
    public int Id { get; init; }
    public DateTime EnvoiDate { get; init; }
    
    public int DemandeId { get; init; }
    public int DocumentId { get; init; }
    
    public string Action { get; init; }
}