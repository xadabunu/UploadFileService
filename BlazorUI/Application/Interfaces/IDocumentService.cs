namespace BlazorUI.Application.Interfaces;

public interface IDocumentService
{
    Task<int> Create(Document document, IBrowserFile file, string connectionId);
    Task<bool> Delete(Document document, string connectionId);
    Task<IEnumerable<Document>> GetDocumentsByType(int demandeId, string typeDocument);
}