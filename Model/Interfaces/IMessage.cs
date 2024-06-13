namespace Model.Interfaces;

public interface IMessage
{
    int Id { get; init; }
    int DemandeId { get; init; }
    DateTime EnvoiDate { get; init; }
    string ConnectionId { get; init; }
}