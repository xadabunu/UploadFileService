using Microsoft.AspNetCore.Mvc;
using Model.EnumerationClasses;

namespace API.Endpoints;

public static class FileEndpoints
{
    public static WebApplication MapFileEndpoints(this WebApplication app)
    {
        app.MapGet("/document/{id:int}", async (IRepository<Document> repository, int id)
            => await repository.GetById(id) is Document document ? Ok(document) : NotFound());

        app.MapGet("/document", async (IRepository<Document> repository)
            => await repository.GetAll());

        app.MapPost("/document/{connectionId}", async (string connectionId, IRepository<Document> repository,
            IHubContext<MyHub> hubContext, Document document) =>
        {
            var newDocument = await repository.Create(document);

            await hubContext.Clients
                .GroupExcept(document.DemandeId.ToString(), connectionId)
                .SendAsync(DocumentNotification.Notify);

            await hubContext.Clients
                .Client(connectionId)
                .SendAsync(DocumentNotification.Added, newDocument);

            return Created(new Uri($"http://localhost:5123/document/{newDocument.Id}"), newDocument);
        });

        app.MapPost("/file/{documentId:int}",
            async (int documentId, IFormFile file, IQueueService queueService, [FromQuery] string connectionId,
                IRepository<Document> documentRepository) =>
            {
                var document = await documentRepository.GetById(documentId);

                var target = $@"C:\Eprolex\id_{document.DemandeId}\{document.TypeCode}";

                Directory.CreateDirectory(target);

                await using var fileStream = new FileStream(Path.Combine(target, file.FileName), FileMode.Create);
                await file.OpenReadStream().CopyToAsync(fileStream);

                if (!System.IO.File.Exists(Path.Combine(target, file.FileName)))
                {
                    throw new UploadFileException($"Le fichier {file.FileName} n'a pas pu être créé.");
                }

                var message = new FileMessage
                {
                    DemandeId = document.DemandeId,
                    DocumentId = document.Id,
                    ConnectionId = connectionId,
                    EnvoiDate = DateTime.Now
                };

                queueService.Send(message);
            }).DisableAntiforgery();

        app.MapPut("/document", async (IRepository<Document> repository, Document document)
            => await repository.Update(document) is Document updatedDocument ? Ok(updatedDocument) : NotFound());

        app.MapDelete("/document/{documentId:int}/{connectionId}/{demandeId}",
            async (IRepository<Document> repository, IHubContext<MyHub> hubContext,
                int documentId, string connectionId, string demandeId) =>
            {
                await repository.Delete(documentId);

                await hubContext.Clients
                    .Client(connectionId)
                    .SendAsync(DocumentNotification.Deleted, documentId);

                await hubContext.Clients.GroupExcept(demandeId, connectionId).SendAsync(DocumentNotification.Notify);

                return NoContent();
            });

        app.MapGet("/projet/{demandeId:int}", async (IRepository<Document> repository, int demandeId)
            => await repository.GetProjetByDemandeId(demandeId));

        app.MapGet("/annexeProjet/{demandeId:int}", async (IRepository<Document> repository, int demandeId)
            => await repository.GetAnnexesByDemandeId(demandeId));

        app.MapGet("/autreDocument/{demandeId:int}", async (IRepository<Document> repository, int demandeId)
            => await repository.GetAutresDocumentsByDemandeId(demandeId));

        app.MapPost("/scanresult", async (IHubContext<MyHub> hubContext, ScanResultMessage message) =>
        {
            await hubContext.Clients
                .Client(message.ConnectionId)
                .SendAsync(DocumentNotification.ScanResult, message);

            await hubContext.Clients
                .GroupExcept(message.DemandeId.ToString(), message.ConnectionId)
                .SendAsync(DocumentNotification.Notify);

            return Ok();
        });

        return app;
    }
}