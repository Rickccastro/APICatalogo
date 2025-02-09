using APICatalogo.Models;
using APICatalogo.Repositories;
using ApiCatalogoXUnitTests.Mock.Entities;
using ApiCatalogoXUnitTests.Mock.Mapper;
using AutoMapper;

namespace ApiCatalogoXUnitTests.UnitTests;

public class ProdutosUnitTestController
{
    public IUnitOfWork repository;
    public IMapper mapper;

    private List<Categoria> _categorias;

    public List<Categoria> Categorias
    {
        get
        {
            if (_categorias == null)
            {
                _categorias = new CategoriaFakeData().BuildRandomListCategoriaData(null);
            }
            return _categorias;
        }
    }


    public ProdutosUnitTestController()
    {
        var mapper = MapperBuilder.Build();

        repository = new UnitOfWork();
    }


}
