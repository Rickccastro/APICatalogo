using APICatalogo.Models;

namespace APICatalogo.Repositories;

public interface IProdutoRepository
{
    public IQueryable<Produto> GetProdutos();
    public Produto GetProduto(int id);
    public Produto Create(Produto categoria);
    public bool Update(Produto categoria);
    public bool Delete(int id);
}
