namespace API.Model.EnumerationClasses;

public class TypeDocument
{
    public string Code { get; }
    public string Libelle { get; }
    
    public static readonly TypeDocument Projet = new("projet", "projet");
    public static readonly TypeDocument AnnexeProjet = new("annexeProjet", "annexe au projet");
    public static readonly TypeDocument AutreDocument = new("autreDocument", "autre document");

    private TypeDocument(string code, string libelle)
    {
        Code = code;
        Libelle = libelle;
    }

    public IEnumerable<TypeDocument> GetAll()
    {
        yield return Projet;
        yield return AnnexeProjet;
        yield return AutreDocument;
    }
}