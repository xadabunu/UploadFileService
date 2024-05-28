namespace BlazorUI.Application.Interfaces;

public interface IDemandeRepository
{
    Task<Demande> GetById(int id);
}