namespace FileSafetyService;

public class FileMessageConsumer(IRepository<Document> repository, IHttpClientFactory factory) : IConsumer<FileMessage>
{
    private readonly HttpClient client = factory.CreateClient("API");

    public async Task Consume(ConsumeContext<FileMessage> context)
    {
        var message = context.Message;

        var document = await repository.GetById(message.DocumentId);

        if (document is not null)
        {
            var scanResultMessage = await CheckDocument(document, message.ConnectionId);

            await client.PostAsJsonAsync("/scanresult", scanResultMessage);
        }
    }

    private async Task<ScanResultMessage> CheckDocument(Document document, string connectionId)
    {
        var path = $"C:/Eprolex/id_{document.DemandeId}/{document.TypeCode}/{document.Nom}";

        var fileIsSafe = await ScanFile(path);

        document.StatutCode = fileIsSafe ? StatutDocument.Valide.Code : StatutDocument.Corrompu.Code;

        if (!fileIsSafe)
        {
            File.Delete(path);
        }

        // if (!CheckExtension(path))
        // {
        //     Console.WriteLine("Pas un pdf.");
        //
        //     // conversion
        // }

        await repository.Update(document);

        return new ScanResultMessage(document.DemandeId, document.Id, document.StatutCode, connectionId);
    }

    private static async Task<bool> ScanFile(string path)
    {
        const int safetyProbability = 99;

        await Task.Delay(TimeSpan.FromSeconds(3));

        return RandomNumberGenerator.GetInt32(100) < safetyProbability;
    }
}