using Model.EnumerationClasses;

namespace BlazorUI.Application.Repositories;

public class DocumentRepository(IHttpClientFactory factory) : IDocumentRepository
{
    public async Task<bool> Create(Document document, IBrowserFile file)
    {
        var client = factory.CreateClient("API");
        
        try
        {
            document = await WriteToDb(client, document);
            
            return await CreateFile(client, document, file);
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

    public async Task<bool> Delete(int id)
    {
        var client = factory.CreateClient("API");

        var responseMessage = await client.DeleteAsync($"/document/{id}");

        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<Document>> GetProjet(int demandeId)
    {
        var client = factory.CreateClient("API");

        return await client.GetFromJsonAsync<IEnumerable<Document>>($"/projet/{demandeId}");
    }

    public async Task<IEnumerable<Document>> GetAnnexes(int demandeId)
    {
        var client = factory.CreateClient("API");

        return [];
    }

    public async Task<IEnumerable<Document>> GetAutresDocuments(int demandeId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Document>> GetDocuments(int demandeId, string typeDocument)
    {
        var client = factory.CreateClient("API");

        return await client.GetFromJsonAsync<IEnumerable<Document>>($"{typeDocument}/{demandeId}");
    }

    private static async Task<Document> WriteToDb(HttpClient client, Document document)
    {
        var response = await client.PostAsJsonAsync("/document", document);

        return await response.Content.ReadFromJsonAsync<Document>();
    }

    private static async Task<bool> CreateFile(HttpClient client, Document document, IBrowserFile file)
    {
        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(file.OpenReadStream());
        
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        content.Add(fileContent, "file", file.Name);
        
        var response = await client.PostAsync($"/file/{document.Id}", content);
        
        return response.IsSuccessStatusCode;
    }
}