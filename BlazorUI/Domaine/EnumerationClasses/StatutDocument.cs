namespace BlazorUI.Domaine.EnumerationClasses;

public class StatutDocument
{
    public string Code { get; }
    public string Libelle { get; }
    
    public static readonly StatutDocument EnCours = new("enCours", "en cours de vérification");
    public static readonly  StatutDocument Valide = new("valide", "valide");
    public static readonly  StatutDocument Corrompu = new("corrompu", "corrompu");

    private StatutDocument(string code, string libelle)
    {
        Code = code;
        Libelle = libelle;
    }
    
    public IEnumerable<StatutDocument> GetAll()
    {
        yield return EnCours;
        yield return Valide;
        yield return Corrompu;
    }
}