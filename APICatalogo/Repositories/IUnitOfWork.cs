using APICatalogo.Repositories.interfaces.SpecificInterface;

namespace APICatalogo.Repositories;

public interface IUnitOfWork
{
    public IProdutoRepository ProdutoRepository { get;}
    public ICategoriaRepository CategoriaRepository { get;}
    public Task CommitAsync();
}
