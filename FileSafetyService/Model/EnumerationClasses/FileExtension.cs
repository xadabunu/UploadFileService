namespace FileSafetyService.EnumerationClasses;

public class FileExtension
{
    public string Nom { get; }
    public string MagicNumber { get; }

    public static readonly FileExtension Pdf = new("pdf", "%PDF");

    private FileExtension(string nom, string magicNumber)
    {
        Nom = nom;
        MagicNumber = magicNumber;
    }

    public static string? GetNomFromMagicNumber(string magicNumber)
    {
        return 
            (from extension in GetAll()
             where extension.MagicNumber == magicNumber
             select extension.Nom)
            .FirstOrDefault();
    }
    
    public static IEnumerable<FileExtension> GetAll()
    {
        return
        [
            Pdf
        ];
    }
}