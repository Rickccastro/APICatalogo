using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public ProdutosController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutoByCategoria(int id)
    {
        var listaProdutosByCategoria = _unitOfWork.ProdutoRepository.GetProdutoByCategoria(id);
        if (listaProdutosByCategoria is null)
            return NotFound();

        return Ok(listaProdutosByCategoria);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetAsync()
    {
        var listaProdutos = _unitOfWork.CategoriaRepository.GetAll().ToList();
        if (listaProdutos is null)
            return NotFound();

        return Ok(listaProdutos);
    }
    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> GetByIdAsync(int id)
    {
        var produtoById = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);
        if (produtoById is null)
            return NotFound("Produto não encontrado");

        return Ok(produtoById);
    }
    [HttpPost]
    public ActionResult CadastrarProduto([FromBody] Produto produto)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
        _unitOfWork.Commit();

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult AtualizarPutProduto(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest();
        var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduto(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);
        if (produto is null )
            return Ok($"Produto de id = {id} foi excluído");

        var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
        _unitOfWork.Commit();
        return Ok(produtoDeletado); 
    }
}
