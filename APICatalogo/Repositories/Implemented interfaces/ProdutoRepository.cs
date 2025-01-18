using APICatalogo.Context;
using APICatalogo.Models;
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
}
