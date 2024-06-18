namespace BlazorUI.Services;

public class DemandeService(IHttpClientFactory factory) : IDemandeRepository
{
    public async Task<Demande?> GetById(int id)
    {
        var client = factory.CreateClient("API");

        var result = await client.GetAsync($"/demande/{id}");

        var demande = await result.Content.ReadFromJsonAsync<Demande>();

        return demande;
    }
}