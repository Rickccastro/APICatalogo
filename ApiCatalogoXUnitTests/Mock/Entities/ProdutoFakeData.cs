using APICatalogo.Models;
using Bogus;

namespace ApiCatalogoXUnitTests.Mock.Entities;

public class ProdutoFakeData
{

    public Produto BuildRandomProdutosData(Categoria? categoria)
    {
        var fakeProducts = new Faker<Produto>()
            .RuleFor(u => u.Nome, f => f.Commerce.ProductName())
            .RuleFor(u => u.Preco, f => f.Random.Decimal(1, 1000))
            .RuleFor(u => u.Estoque, f => f.Random.Float(1, 1000))
            .RuleFor(u => u.Descricao, f => f.Commerce.ProductDescription())
            .RuleFor(u => u.ImageUrl, f => f.Image.PicsumUrl())
            .RuleFor(u => u.CategoriaId, f => f.Random.Int(1, 1000))
            .RuleFor(u => u.Categoria, f => categoria);

        return fakeProducts.Generate();
    }

    public List<Produto> BuildRandomListProdutosData(Categoria? categoria)
    {
        var fakeProducts = new Faker<Produto>()
            .RuleFor(u => u.ProdutoId, f => f.Random.Int(1, 1000))
            .RuleFor(u => u.Nome, f => f.Commerce.ProductName())
            .RuleFor(u => u.Preco, f => f.Random.Decimal(1, 1000))
            .RuleFor(u => u.Estoque, f => f.Random.Float(1, 1000))
            .RuleFor(u => u.Descricao, f => f.Commerce.ProductDescription())
            .RuleFor(u => u.ImageUrl, f => f.Image.PicsumUrl())
            .RuleFor(u => u.CategoriaId, f => f.Random.Int(1, 1000))
            .RuleFor(u => u.Categoria, f => categoria);

        return fakeProducts.Generate(5);
    }
}
