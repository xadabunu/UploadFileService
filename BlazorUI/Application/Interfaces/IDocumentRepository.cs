namespace BlazorUI.Application.Interfaces;

public interface IDocumentRepository
{
    Task<bool> Create(Document document, IBrowserFile file);
    Task<bool> Delete(int id);
    Task<IEnumerable<Document>> GetProjet(int demandeId);
    Task<IEnumerable<Document>> GetAnnexes(int demandeId);
    Task<IEnumerable<Document>> GetAutresDocuments(int demandeId);
    Task<IEnumerable<Document>> GetDocuments(int demandeId, string typeDocument);
}