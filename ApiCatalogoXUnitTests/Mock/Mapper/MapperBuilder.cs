using APICatalogo.DTOs.Mappings;
using AutoMapper;

namespace ApiCatalogoXUnitTests.Mock.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new ProdutoDTOMappingProfile());
        });

        return mapper.CreateMapper();
    }
}
