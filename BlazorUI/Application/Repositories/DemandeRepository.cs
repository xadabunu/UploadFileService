namespace BlazorUI.Application.Repositories;

public class DemandeRepository(IHttpClientFactory factory) : IDemandeRepository
{
    public async Task<Demande?> GetById(int id)
    {
        var client = factory.CreateClient("API");

        var result = await client.GetAsync($"/demande/{id}");

        var demande = await result.Content.ReadFromJsonAsync<Demande>();

        return demande;
    }
}