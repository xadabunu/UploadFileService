namespace BlazorUI.Services;

public class DocumentService(IHttpClientFactory factory) : IDocumentService
{
    public async Task<int> Create(Document document, IBrowserFile file, string connectionId)
    {
        var client = factory.CreateClient("API");

        try
        {
            document = await WriteToDb(client, document, connectionId);

            if (await CopyFileToFolder(client, document, file, connectionId))
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

        var responseMessage = await client.DeleteAsync($"/document/{document.Id}/{connectionId}/{document.DemandeId}");

        return responseMessage.IsSuccessStatusCode;
    }
    
    public async Task<IEnumerable<Document>> GetDocumentsByType(int demandeId, string typeDocument)
    {
        var client = factory.CreateClient("API");

        return await client.GetFromJsonAsync<IEnumerable<Document>>($"{typeDocument}/{demandeId}");
    }

    private static async Task<Document?> WriteToDb(HttpClient client, Document document, string connectionId)
    {
        var response = await client.PostAsJsonAsync($"/document/{connectionId}", document);

        return await response.Content.ReadFromJsonAsync<Document>();
    }

    private static async Task<bool> CopyFileToFolder(HttpClient client, Document document, IBrowserFile file, string connectionId)
    {
        var content = new MultipartFormDataContent();
        
        var fileContent = new StreamContent(file.OpenReadStream());
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        content.Add(fileContent, "file", file.Name);

        // var connectionContent = new StringContent(connectionId);
        // content.Add(connectionContent);

        var response = await client.PostAsync($"/file/{document.Id}?connectionId={connectionId}", content);

        return response.IsSuccessStatusCode;
    }
}