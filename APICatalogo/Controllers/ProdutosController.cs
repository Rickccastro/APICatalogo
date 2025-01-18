using APICatalogo.Models;
using APICatalogo.Repositories.interfaces.GenericInterface;
using APICatalogo.Repositories.interfaces.SpecificInterface;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;
    public ProdutosController(IProdutoRepository produtoRepository)

    {
        _repository = produtoRepository;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutoByCategoria(int id)
    {
        var listaProdutosByCategoria = _repository.GetProdutoByCategoria(id);
        if (listaProdutosByCategoria is null)
            return NotFound();

        return Ok(listaProdutosByCategoria);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetAsync()
    {
        var listaProdutos = _repository.GetAll().ToList();
        if (listaProdutos is null)
            return NotFound();

        return Ok(listaProdutos);
    }
    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> GetByIdAsync(int id)
    {
        var produtoById = _repository.Get(p => p.ProdutoId == id);
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
        var produtoAtualizado = _repository.Update(produto);

        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduto(int id)
    {
        var produto = _repository.Get(p => p.ProdutoId == id);
        if (produto is null )
            return Ok($"Produto de id = {id} foi excluído");

        var produtoDeletado = _repository.Delete(produto);
        return Ok(produtoDeletado); 
    }
}
