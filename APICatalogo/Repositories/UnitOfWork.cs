using APICatalogo.Context;
using APICatalogo.Repositories.interfaces.SpecificInterface;

namespace APICatalogo.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IProdutoRepository? _produtoRepo;
    private ICategoriaRepository? _categoriaRepo;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IProdutoRepository ProdutoRepository
    {
        get
        {
            if (_produtoRepo == null)
            {
                _produtoRepo = new ProdutoRepository(_context);
            }
            return _produtoRepo;
        }
    }

    public ICategoriaRepository CategoriaRepository
    {
        get
        {
            if ( _categoriaRepo == null)
            {
                _categoriaRepo = new CategoriaRepository(_context);
            }
            return _categoriaRepo;
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }
    public void Dispose() 
    {
        _context.Dispose(); 
    }
}
