using APICatalogo.Models;
using APICatalogo.Repositories.interfaces.GenericInterface;

namespace APICatalogo.Repositories.interfaces.SpecificInterface;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutoByCategoria(int id);
}
