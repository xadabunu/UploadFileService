namespace BlazorUI.Model;

public class Demande
{
    public int Id { get; set; }
    public int? NumeroRole { get; set; } = null;
    public int NumeroDemande { get; set; }
    public ICollection<Document> Documents { get; } = [];
}