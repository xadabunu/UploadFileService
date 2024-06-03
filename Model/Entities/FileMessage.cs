using Model.Interfaces;

namespace Model.Entities;

public record FileMessage : IMessage
{
    public int Id { get; init; }
    public int DemandeId { get; init; }
    public int DocumentId { get; init; }
    public DateTime EnvoiDate { get; init; }
    public string Content { get; init; }
    
    public string Action { get; init; }
}