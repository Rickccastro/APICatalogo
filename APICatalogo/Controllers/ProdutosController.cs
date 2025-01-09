using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var listaProdutos = _context.Produtos.ToList();
        if (listaProdutos is null)
            return NotFound();

        return Ok(listaProdutos);
    }
    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> GetById(int id)
    {
        var produtoById = _context.Produtos.FirstOrDefault(produto => produto.ProdutoId == id);
        if (produtoById is null)
            return NotFound("Produto não encontrado");

        return Ok(produtoById);
    }
    [HttpPost]
    public ActionResult CadastrarProduto(Produto produto)
    {
        if (produto is null)
            return BadRequest();
        _context.Produtos!.Add(produto);

        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult AtualizarPutProduto(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest();

        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduto(int id)
    {
       var produto = _context.Produtos.FirstOrDefault(produto => produto.ProdutoId ==id);
        if (produto is null)
            return NotFound("Produto não encontrado");

        _context.Produtos.Remove(produto);
        _context.SaveChanges(); 

        return Ok(produto);
    }
}
