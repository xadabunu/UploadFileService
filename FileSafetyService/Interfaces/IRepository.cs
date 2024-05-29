namespace FileSafetyService.Interfaces;

public interface IRepository<T>
{
    Task<T?> GetById(int id);
    Task<T?> Update(T entity);
}