using System.Linq.Expressions;

namespace APICatalogo.Repositories.interfaces.GenericInterface;

public interface IRepository<T>
{
    public Task <IEnumerable<T>> GetAllAsync();
    public Task <T?> GetAsync(Expression<Func<T, bool>> predicate);
    public T Create(T entity);
    public T Update(T entity);
    public T Delete(T entity);
}
