using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using Model.Entities;

namespace FileSafetyService;

public static class ChannelExtension
{
    public static void EPSetupConsumer(this IModel channel, IRepository<Document> repository, HttpClient client)
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
            var message = GetMessageFromArgs(eventArgs);

            if (message.DocumentId == 0)
            {
                return;
            }

            var document = await repository.GetById(message.DocumentId);

            var path = $"C:/Eprolex/id_{document.DemandeId}/{document.TypeCode}/{document.Nom}";

            var fileIsSafe = await FileIsSafe(path);

            if (fileIsSafe)
            {
                document.StatutCode = StatutDocument.Valide.Code;
            }
            else
            {
                // suppresion physique ?
                document.StatutCode = StatutDocument.Corrompu.Code;

                return;
            }

            // if (!CheckExtension(path))
            // {
            //     Console.WriteLine("Pas un pdf.");
            //
            //     // conversion
            // }

            await repository.Update(document);

            var msg = new ScanResultMessage(document.DemandeId, document.Id, document.TypeCode, document.StatutCode);

            await client.PostAsJsonAsync("/scanresult", msg);

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

    private static FileMessage GetMessageFromArgs(BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray();
        var messageStr = Encoding.UTF8.GetString(body);

        return JsonSerializer.Deserialize<FileMessage>(messageStr);
    }

    private static async Task<bool> FileIsSafe(string path)
    {
        const int safetyProbability = 99;

        await Task.Delay(TimeSpan.FromSeconds(5));

        return RandomNumberGenerator.GetInt32(100) < safetyProbability;
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