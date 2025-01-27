using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs.Patch;

public class ProdutoDTOUpdateRequest : IValidatableObject
{
    [Range(1,9999,ErrorMessage ="Estoque deve estar entre 1-9999")]
    public float Estoque { get; set; }

    [Range(1, 9999, ErrorMessage = "Estoque deve estar entre 1-9999")]
    public DateTime DataCadastro { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataCadastro.Date <= DateTime.Now.Date)
        {
            // yield serve para retornar a  iteração da lista de erros da validação,
            // que nesse caso só retorna 1 mas poderia retornar mais.
            yield return new ValidationResult("A data deve ser maior que a data atual", new[] {nameof(this.DataCadastro) });
        }
    }
}
