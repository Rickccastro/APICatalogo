using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
    {
        var listaProdutos = await _context.Produtos.Take(10).AsNoTracking().ToListAsync();
        if (listaProdutos is null)
            return NotFound();

        return Ok(listaProdutos);
    }
    [HttpGet("{id}", Name = "ObterProduto")]
    public async Task<ActionResult<Produto>> GetByIdAsync([BindRequired]int id)
    {
        var produtoById = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(produto => produto.ProdutoId == id);
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
