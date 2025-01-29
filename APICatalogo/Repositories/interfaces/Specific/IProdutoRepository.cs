using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories.interfaces.GenericInterface;

namespace APICatalogo.Repositories.interfaces.SpecificInterface;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
    Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosParams);
    Task<IEnumerable<Produto>> GetProdutoByCategoriaAsync(int id);
}
