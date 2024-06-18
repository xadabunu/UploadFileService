namespace Model.EnumerationClasses;

public class StatutDocument
{
    public string Code { get; }
    public string Libelle { get; }
    
    public static readonly StatutDocument EnCours = new("enCours", "En cours de vérification");
    public static readonly  StatutDocument Valide = new("valide", "Valide");
    public static readonly  StatutDocument Corrompu = new("corrompu", "Corrompu");

    private StatutDocument(string code, string libelle)
    {
        Code = code;
        Libelle = libelle;
    }
    
    public static IEnumerable<StatutDocument> GetAll()
    {
        yield return EnCours;
        yield return Valide;
        yield return Corrompu;
    }

    public static string GetLibelleByCode(string code)
    {
        return GetAll().FirstOrDefault(statut => statut.Code == code)?.Libelle ?? string.Empty;
    }
}