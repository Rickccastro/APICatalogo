using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories.interfaces.SpecificInterface;
using APICatalogo.Repositories.methods;
using X.PagedList;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>,IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) :base(context)   
    {
        
    }

    public async Task<IEnumerable<Produto>> GetProdutoByCategoriaAsync(int id)
    {
        var produtos = await GetAllAsync();

        return produtos.Where(c => c.CategoriaId == id);
    }


    public async Task <IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
    {
        var produtos = await GetAllAsync();
        var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();
        var resultado = await produtosOrdenados.ToPagedListAsync(produtosParams.PageNumber, produtosParams.PageSize);      
        return resultado;
    }

    public async Task <IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFltroParams)
    {
        var produtos = await GetAllAsync();

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

        var produtosFiltrados = await produtos.ToPagedListAsync(produtosFltroParams.PageNumber, produtosFltroParams.PageSize);
        return produtosFiltrados;
    }
}
