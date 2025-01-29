using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories.interfaces.GenericInterface;

namespace APICatalogo.Repositories.interfaces.SpecificInterface;

public interface IProdutoRepository : IRepository<Produto>
{
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);
    PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosParams);
    IEnumerable<Produto> GetProdutoByCategoria(int id);
}
