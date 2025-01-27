using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.interfaces.GenericInterface;

namespace APICatalogo.Repositories.interfaces.SpecificInterface;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
    IEnumerable<Produto> GetProdutoByCategoria(int id);
}
