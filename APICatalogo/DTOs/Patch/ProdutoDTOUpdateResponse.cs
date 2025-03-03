﻿using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs.Patch;

public class ProdutoDTOUpdateResponse
{
   
    public int ProdutoId { get; set; }

    public string? Nome { get; set; }

    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public string? ImageUrl { get; set; }
    public int CategoriaId { get; set; }
}
