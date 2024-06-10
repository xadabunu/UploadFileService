namespace FileSafetyService.Interfaces;

public interface IRepository<T>
{
    Task<T?> GetById(int id);
    Task<bool?> Update(T entity);
}