using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories.interfaces.SpecificInterface;
using APICatalogo.Repositories.methods;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>,IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) :base(context)   
    {
        
    }

    public IEnumerable<Produto> GetProdutoByCategoria(int id)
    {
      return GetAll().Where(c => c.CategoriaId == id);
    }

   
    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);      
        return produtosOrdenados;
    }

    public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFltroParams)
    {
        var produtos = GetAll().AsQueryable();
        if (produtosFltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFltroParams.PrecoCriterio))
        {
            if (produtosFltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco > produtosFltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            if (produtosFltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco < produtosFltroParams.Preco.Value).OrderBy(p => p.Preco);
            } 
            if (produtosFltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco == produtosFltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
        }

        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFltroParams.PageNumber, produtosFltroParams.PageSize);
        return produtosFiltrados;
    }
}
