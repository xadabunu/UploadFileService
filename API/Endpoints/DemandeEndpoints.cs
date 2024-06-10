namespace API.Endpoints;

public static class DemandeEndpoints
{
    public static WebApplication MapDemandeEndpoints(this WebApplication app)
    {
        app.MapGet("/demande/{id:int}", async (IRepository<Demande> repository, int id)
            => await repository.GetById(id) is Demande demande ? Ok(demande) : NotFound());

        app.MapGet("/demande", async (IRepository<Demande> repository) => await repository.GetAll());
        
        app.MapPost("/demande", async (IRepository<Demande> repository) =>
        {
            var demande = await repository.Create(null);
            return Created(new Uri($"http://localhost:5123/demande/{demande.Id}"), demande);
        });
        
        return app;
    }
}