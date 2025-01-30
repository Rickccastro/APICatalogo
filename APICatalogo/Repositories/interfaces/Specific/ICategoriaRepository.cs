using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories.interfaces.GenericInterface;
using X.PagedList;

namespace APICatalogo.Repositories.interfaces.SpecificInterface;

public interface ICategoriaRepository: IRepository<Categoria>
{
    Task<IPagedList<Categoria>> GetCategoriaAsync(CategoriasParameters categoriasParams);
    Task<IPagedList<Categoria>> GetCategoriaFiltroNomeAsync(CategoriaFiltroNome categoriasParams);
    
}
