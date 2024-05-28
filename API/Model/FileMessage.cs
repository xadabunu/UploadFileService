namespace API.Model;

public class FileMessage : IMessage
{
    public int Id { get; set; }
    public int DemandeId { get; set; }
    public int DocumentId { get; set; }
    public DateTime EnvoiDate { get; set; }
    public string Content { get; set; }
    public ReadOnlyMemory<byte> Encode()
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);
        
        writer.Write(DemandeId);
        writer.Write(DocumentId);
        writer.Write(EnvoiDate.Ticks);
        writer.Write(Content);
        
        return new ReadOnlyMemory<byte>(memoryStream.ToArray());
    }
}