using System.Security.Cryptography;
using System.Text.Json;

namespace FileSafetyService;

public static class ChannelExtension
{
    public static void EPSetupConsumer(this IModel channel, IRepository<Document> repository)
    {
        channel.QueueDeclare(
            queue: "file-checking-queue",
            durable: false,
            exclusive: false,
            autoDelete: false
        );

        channel.BasicQos(
            prefetchSize: 0,
            prefetchCount: 12,
            global: false
        );

        var consumer = new AsyncEventingBasicConsumer(channel);
        
        consumer.Received += async (_, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();

            var messageStr = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<FileMessage>(messageStr);

            Console.WriteLine(message);
            
            var document = await repository.GetById(message.DocumentId);

            var path = $"C:/Eprolex/id_{document.DemandeId}/{document.TypeCode}/{document.Nom}";

            if (!await ScanFile(path))
            {
                Console.WriteLine("Fichier corrompu");
                // suppresion physique ?
                document.StatutCode = StatutDocument.Corrompu.Code;
                
                return;
            }

            document.StatutCode = StatutDocument.Valide.Code;
            
            if (!CheckExtension(path))
            {
                Console.WriteLine("Pas un pdf.");
            
                // conversion
            }
            
            await repository.Update(document);

            channel.BasicAck(
                deliveryTag: eventArgs.DeliveryTag,
                multiple: false
            );
        };
        
        channel.BasicConsume(
            queue: "file-checking-queue",
            autoAck: false,
            consumer: consumer
        );
    }

    private static async Task<bool> ScanFile(string path)
    {
        const int probability = 1;
        
        await Task.Delay(TimeSpan.FromSeconds(5));
        
        return RandomNumberGenerator.GetInt32(100) < probability;
    }

    private static bool CheckExtension(string path)
    {
        var fileExtension = Path.GetExtension(path).ToLower();
        var fileMagicNumber = ReadFirstBytes(path);

        var fileType = FileExtension.GetNomFromMagicNumber(fileMagicNumber);
        
        return fileType != null && fileType == fileExtension;
    }

    private static string ReadFirstBytes(string path)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var binaryReader = new BinaryReader(fileStream);
        var bytes = binaryReader.ReadBytes(4);

        return BitConverter.ToString(bytes).Replace("-", string.Empty);
    }
}