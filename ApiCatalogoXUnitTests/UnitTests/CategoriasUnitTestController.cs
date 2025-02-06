using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Repositories.interfaces.SpecificInterface;
using ApiCatalogoXUnitTests.Mock;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using X.PagedList;

namespace ApiCatalogoXUnitTests.UnitTests;

public class CategoriasUnitTestController
{
    private readonly FakeRepositoryCategoria _fakeRepositoryCategoria;

    public CategoriasUnitTestController()
    {
        _fakeRepositoryCategoria = new FakeRepositoryCategoria();
    }

    [Fact]
    public async Task Test_GetAllAsync_ShouldReturnCategorias()
    {
        // Arrange
        _fakeRepositoryCategoria.GetAll();
        var repository = _fakeRepositoryCategoria.Build();

        // Act
        var results = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Count()); // Verifica se há 2 categorias mockadas
        Assert.Equal("Categoria A", results.First().Nome); // Verifica o nome da primeira categoria
    }
}