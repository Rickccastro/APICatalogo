using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Repositories.interfaces.SpecificInterface;
using Moq;

namespace ApiCatalogoXUnitTests.Mock;

public class FakeRepositoryCategoria
{
    private readonly Mock<ICategoriaRepository> _repository;

    public FakeRepositoryCategoria()
    {
        _repository = new Mock<ICategoriaRepository>();
    }

    public ICategoriaRepository Build()
    {
        return _repository.Object;
    }

    public FakeRepositoryCategoria GetAll()
    {
        var categoriasFake = new List<Categoria>
        {
            new Categoria { CategoriaId = 1, Nome = "Categoria A" },
            new Categoria { CategoriaId = 2, Nome = "Categoria B" }
        };

        _repository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(categoriasFake);

        return this;
    }

}
