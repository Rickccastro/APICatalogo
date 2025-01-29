using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories.interfaces.GenericInterface;

namespace APICatalogo.Repositories.interfaces.SpecificInterface;

public interface ICategoriaRepository: IRepository<Categoria>
{
    PagedList<Categoria> GetCategoria(CategoriasParameters categoriasParams);
    PagedList<Categoria> GetCategoriaFiltroNome(CategoriaFiltroNome categoriasParams);
    
}
