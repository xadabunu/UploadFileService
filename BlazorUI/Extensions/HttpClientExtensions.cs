using System.Text;

namespace BlazorUI.Extensions;

public static class HttpClientExtensions
{
    public static async Task<Document?> WriteToDb(this HttpClient client, Document document, string connectionId)
    {
        var response = await client.PostAsJsonAsync($"/document/{connectionId}", document);

        return await response.Content.ReadFromJsonAsync<Document>();
    }

    public static async Task<bool> UploadFile(this HttpClient client, Document document, IBrowserFile file,
        string connectionId)
    {
        // var content = new MultipartFormDataContent();
        // var fileContent = new StreamContent(file.OpenReadStream(long.MaxValue));
        //
        // fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        // content.Add(fileContent, "file", file.Name);
        //
        // // var connectionContent = new StringContent(connectionId);
        // // content.Add(connectionContent);
        //
        // var response = await client.PostAsync($"/file/{document.Id}?connectionId={connectionId}", content);
        //
        // Console.WriteLine($"===> ResponseStatusCode : {response.StatusCode}");
        // return response.IsSuccessStatusCode;

        /* using chunks | not working, file sent and received don't have same size*/

        // const int chunkSize = 10 * 1024 * 1024; // Taille du chunk : 10 MB
        // var fileStream = file.OpenReadStream(long.MaxValue);
        // var fileSize = file.Size;
        //
        // var totalChunks = (long)Math.Ceiling((double)fileSize / chunkSize);
        //
        // var checker = fileSize;
        //
        // for (var chunkIndex = 0; chunkIndex < totalChunks; chunkIndex++)
        // {
        //     var chunkContent = new MultipartFormDataContent();
        //
        //     var buffer = new byte[chunkSize];
        //     var bytesRead = await fileStream.ReadAsync(buffer, 0, chunkSize);
        //
        //     checker -= bytesRead;
        //
        //     Console.WriteLine($"Checker after reading chunk {chunkIndex}: {checker}");
        //
        //     if (bytesRead == 0)
        //     {
        //         break;
        //     }
        //
        //     var chunkStream = new MemoryStream(buffer, 0, bytesRead);
        //     var fileContent = new StreamContent(chunkStream);
        //     fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        //
        //     // Ajoutez des informations supplémentaires pour identifier le chunk
        //     chunkContent.Add(fileContent, "file", file.Name);
        //     chunkContent.Add(new StringContent(chunkIndex.ToString()), "chunkIndex");
        //     chunkContent.Add(new StringContent(totalChunks.ToString()), "totalChunks");
        //
        //     var response = await client.PostAsync($"/file2/{document.Id}?connectionId={connectionId}", chunkContent);
        //
        //     if (!response.IsSuccessStatusCode)
        //     {
        //         return false;
        //     }
        // }
        //
        // Console.WriteLine($"checker : {checker}");
        //
        // return true;

        /* stockage via blazor server */

        const int maxFileSize = 1024 * 1024 * 500;

        var directory = @$"C:\Eprolex\id_{document.DemandeId}\{document.TypeCode}";

        Directory.CreateDirectory(directory);

        var filePath = Path.Combine(directory, file.Name);

        await using var stream = file.OpenReadStream(maxFileSize);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await stream.CopyToAsync(fileStream);

        var content = new StringContent(connectionId, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"/file3/{document.Id}", content);

        return response.IsSuccessStatusCode;
    }
}