using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories.interfaces.GenericInterface;

namespace APICatalogo.Repositories.interfaces.SpecificInterface;

public interface ICategoriaRepository: IRepository<Categoria>
{
    Task<PagedList<Categoria>> GetCategoriaAsync(CategoriasParameters categoriasParams);
    Task<PagedList<Categoria>> GetCategoriaFiltroNomeAsync(CategoriaFiltroNome categoriasParams);
    
}
