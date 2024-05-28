namespace FileSafetyService;

public class FileMessage
{
    public FileMessage(ReadOnlyMemory<byte> byteMessage)
    {
        Decode(byteMessage);
    }
    
    public int Id { get; set; }
    public int DemandeId { get; set; }
    public int DocumentId { get; set; }
    public DateTime EnvoiDate { get; set; }
    public string Content { get; set; }

    private void Decode(ReadOnlyMemory<byte> byteMessage)
    {
        using var memoryStream = new MemoryStream(byteMessage.ToArray());
        using var reader = new BinaryReader(memoryStream);

        DemandeId = reader.ReadInt32();
        DocumentId = reader.ReadInt32();
        EnvoiDate = new DateTime(reader.ReadInt64());
        Content = reader.ReadString();
    }

    public override string ToString()
    {
        return
            $"""
             demandeId: {DemandeId},
             documentId: {DocumentId},
             envoiDate: {EnvoiDate},
             content: {Content}
             """;
    }
}