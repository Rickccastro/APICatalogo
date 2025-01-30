using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories.interfaces.GenericInterface;
using X.PagedList;

namespace APICatalogo.Repositories.interfaces.SpecificInterface;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
    Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosParams);
    Task<IEnumerable<Produto>> GetProdutoByCategoriaAsync(int id);
}
