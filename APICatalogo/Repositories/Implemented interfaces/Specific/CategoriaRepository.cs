using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories.interfaces.SpecificInterface;
using APICatalogo.Repositories.methods;
using System.Linq.Expressions;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }
}
