using APICatalogo.Models;
using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class ProdutoDTO
{
    [Key]
    public int ProdutoId { get; set; }

    [Required]
    [StringLength(80)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }


    [Required]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    public int CategoriaId { get; set; }
}
