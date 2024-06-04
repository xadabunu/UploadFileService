namespace API.Repositories;

public interface IRepository<T>
{
    Task<T> Create(T entity);
    Task<T?> Update(T entity);
    Task Delete(int id);
    Task<T?> GetById(int id);
    Task<IEnumerable<T>> GetAll();

    Task<IEnumerable<T>> GetProjetByDemandeId(int demandeId);
    Task<IEnumerable<T>> GetAnnexesByDemandeId(int demandeId);
    Task<IEnumerable<T>> GetAutresDocumentsByDemandeId(int demandeId);
}