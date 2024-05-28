namespace API;

public interface IMessage
{
    int Id { get; set; }
    int DemandeId { get; set; }
    DateTime EnvoiDate { get; set; }
    string Content { get; set; }

    ReadOnlyMemory<byte> Encode();
}