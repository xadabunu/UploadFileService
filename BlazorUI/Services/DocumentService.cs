namespace BlazorUI.Services;

public class DocumentService(IHttpClientFactory factory) : IDocumentService
{
    public async Task<int> Create(Document document, IBrowserFile file, string connectionId)
    {
        var client = factory.CreateClient("API");

        try
        {
            document = await client.WriteToDb(document, connectionId);

            if (await client.UploadFile(document, file, connectionId))
            {
                return document.Id;
            }

            return 0;
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"==> caught an error with message: {e.Message}");
            throw;
        }
    }

    public async Task<bool> Delete(Document document, string connectionId)
    {
        var client = factory.CreateClient("API");
        
        var directory = @$"C:\Eprolex\id_{document.DemandeId}\{document.TypeCode}";
        var filePath = Path.Combine(directory, document.Nom);
        
        File.Delete(filePath);

        var responseMessage = await client.DeleteAsync($"/document/{document.Id}/{connectionId}/{document.DemandeId}");

        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<Document>> GetDocumentsByType(int demandeId, string typeDocument)
    {
        var client = factory.CreateClient("API");

        return await client.GetFromJsonAsync<IEnumerable<Document>>($"{typeDocument}/{demandeId}");
    }
}