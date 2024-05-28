namespace API.Model;

public class Demande
{
    public int Id { get; set; }
    public int? NumeroRole { get; set; } = null;
    public int NumeroAttente { get; set; }
    public IEnumerable<Document> Documents { get; } = [];
}