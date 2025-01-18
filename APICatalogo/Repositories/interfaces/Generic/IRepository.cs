using System.Linq.Expressions;

namespace APICatalogo.Repositories.interfaces.GenericInterface;

public interface IRepository<T>
{
    public IEnumerable<T> GetAll();
    public T? Get(Expression<Func<T, bool>> predicate);
    public T Create(T entity);
    public T Update(T entity);
    public T Delete(T entity);
}
