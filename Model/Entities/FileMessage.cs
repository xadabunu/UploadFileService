namespace Model.Entities;

public record FileMessage
{
    public int DemandeId { get; init; }
    public int DocumentId { get; init; }
    public DateTime EnvoiDate { get; init; }
    public string ConnectionId { get; init; }
}