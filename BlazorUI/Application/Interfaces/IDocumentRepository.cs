namespace BlazorUI.Application.Interfaces;

public interface IDocumentRepository
{
    Task<int> Create(Document document, IBrowserFile file, string connectionId);
    Task<bool> Delete(int id);
    // Task<IEnumerable<Document>> GetProjet(int demandeId);
    // Task<IEnumerable<Document>> GetAnnexes(int demandeId);
    // Task<IEnumerable<Document>> GetAutresDocuments(int demandeId);
    Task<IEnumerable<Document>> GetDocumentsByType(int demandeId, string typeDocument);
}