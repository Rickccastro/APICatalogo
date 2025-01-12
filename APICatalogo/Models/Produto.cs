using APICatalogo.Validations;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Produto")]
public class Produto : IValidatableObject
{
    [Key]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage ="O nome é obrigatório")]
    [StringLength(80,ErrorMessage = "O nome deve ter no maximo {1} e no minimo {2}caracteres", MinimumLength =5)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300, ErrorMessage ="A descrição deve ter no máximo {1} caracteres")]
    public string? Descricao { get; set;}


    [Required]
    [Range(1,1000,ErrorMessage ="0 preço deve estar entre {1} e {2}")]
    public decimal Preco { get; set;}

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set;}
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    public int CategoriaId { get; set; }
    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(this.Nome))
        {
            var primeiraLetra = this.Nome.ToString()[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula", new[] { nameof(this.Nome) });
            }
        }
        
    }
}
