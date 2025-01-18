using APICatalogo.Models;
using APICatalogo.Repositories.interfaces.SpecificInterface;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;
    public ProdutosController(IProdutoRepository repository)
    {
        _repository = repository;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetAsync()
    {
        var listaProdutos = _repository.GetProdutos().ToList();
        if (listaProdutos is null)
            return NotFound();

        return Ok(listaProdutos);
    }
    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> GetByIdAsync(int id)
    {
        var produtoById = _repository.GetProduto(id);
        if (produtoById is null)
            return NotFound("Produto não encontrado");

        return Ok(produtoById);
    }
    [HttpPost]
    public ActionResult CadastrarProduto([FromBody] Produto produto)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var novoProduto = _repository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult AtualizarPutProduto(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest();
        var atualizado = _repository.Update(produto);

        if (atualizado)
            return Ok(atualizado);
        else
            return StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao atualizar produto com id={id}");

    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduto(int id)
    {
        var deletado = _repository.Delete(id);
        if (deletado)
            return Ok($"Produto de id = {id} foi excluído");
        else
            return StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao excluir produto com id={id}");

    }
}
