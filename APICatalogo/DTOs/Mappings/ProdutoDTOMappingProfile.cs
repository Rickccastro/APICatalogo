﻿using APICatalogo.DTOs.Patch;
using APICatalogo.Models;
using AutoMapper;
using System.Collections.Generic;

namespace APICatalogo.DTOs.Mappings;

public class ProdutoDTOMappingProfile : Profile
{
	public ProdutoDTOMappingProfile()
	{
			CreateMap<Produto,ProdutoDTO>().ReverseMap();
			CreateMap<Produto,ProdutoDTOUpdateRequest>().ReverseMap();
			CreateMap<Produto,ProdutoDTOUpdateResponse>().ReverseMap();
			CreateMap<Categoria,CategoriaDTO>().ReverseMap();
	}
}

