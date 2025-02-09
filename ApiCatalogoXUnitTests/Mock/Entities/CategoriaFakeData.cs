using APICatalogo.Models;
using Bogus;

namespace ApiCatalogoXUnitTests.Mock.Entities;

public class CategoriaFakeData
{
    public Categoria BuildRandomCategoriaData(List<Produto>? listProdutos)
    {
        var fakeCategory = new Faker<Categoria>()
            .RuleFor(u => u.Nome, f => f.Name.Random.ToString())
            .RuleFor(u => u.CategoriaId, f => f.Random.Int())
            .RuleFor(u => u.ImageUrl, f => f.Image.ToString())
            .RuleFor(u => u.Produtos, f => listProdutos);

        return fakeCategory;
    }

    public List<Categoria> BuildRandomListCategoriaData(List<Produto>? listProdutos)
    {
        var fakeCategory = new Faker<Categoria>()
            .RuleFor(u => u.Nome, f => f.Name.Random.ToString())
            .RuleFor(u => u.CategoriaId, f => f.Random.Int())
            .RuleFor(u => u.ImageUrl, f => f.Image.ToString())
            .RuleFor(u => u.Produtos, f => listProdutos);

        return fakeCategory.Generate(5);
    }
}
