namespace API.Endpoints;

public static class FileEndpoints
{
    public static WebApplication MapFileEndpoints(this WebApplication app)
    {
        app.MapGet("/document/{id:int}", async (IRepository<Document> repository, int id)
            => await repository.GetById(id) is Document document ? Ok(document) : NotFound());

        app.MapGet("/document", async (IRepository<Document> repository)
            => await repository.GetAll());
        
        app.MapPost("/document", async (IRepository<Document> repository, Document document) =>
        {
            var newDocument = await repository.Create(document);
            return Created(new Uri($"http://localhost:5123/document/{newDocument.Id}"), newDocument);
        });

        app.MapPost("/file/{documentId:int}",
            async (int documentId, IFormFile file, IQueueService queueService, IRepository<Document> documentRepository) =>
            {
                var document = await documentRepository.GetById(documentId);
                
                var target = $@"C:\Eprolex\id_{document.DemandeId}\{document.TypeCode}";
                
                Directory.CreateDirectory(target);
                
                await using var fileStream = new FileStream(Path.Combine(target, file.FileName), FileMode.Create);
                await file.OpenReadStream().CopyToAsync(fileStream);
                
                if (!System.IO.File.Exists(Path.Combine(target, file.FileName)))
                {
                    // error
                }
                
                // envoie du message au worker service via rabbitMQ
                var message = new FileMessage()
                {
                    DemandeId = document.DemandeId,
                    DocumentId = document.Id,
                    Content = string.Empty,
                    EnvoiDate = DateTime.Now
                };
                queueService.Send(message);
                
            }).DisableAntiforgery();

        app.MapPut("/document", async (IRepository<Document> repository, Document document)
            => await repository.Update(document) is Document updatedDocument ? Ok(updatedDocument) : NotFound());

        app.MapDelete("/document/{id:int}", async (IRepository<Document> repository, int id) =>
        {
            await repository.Delete(id);
            return NoContent();
        });

        return app;
    }
}