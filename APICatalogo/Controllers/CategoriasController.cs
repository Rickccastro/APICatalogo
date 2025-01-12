using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;
    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("produtos")]
    []
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
    {
        var listacategorias = await _context.Categorias.Include(produtoCategoria=> produtoCategoria.Produtos).AsNoTracking().ToListAsync();
        if (listacategorias is null)
            return NotFound();

        return Ok(listacategorias);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> Get()
    {
        var listacategorias = await _context.Categorias.AsNoTracking().ToListAsync();
        if (listacategorias is null)
            return NotFound();

        return Ok(listacategorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetById(int id)
    {
        var categoriaById = _context.Categorias.AsNoTracking().FirstOrDefault(categoria => categoria.CategoriaId == id);
        if (categoriaById is null)
            return NotFound("Categoria não encontrado");

        return Ok(categoriaById);
    }
    [HttpPost]
    public ActionResult CadastrarCategoria(Categoria categoria)
    {
        if (categoria is null)
            return BadRequest();
        _context.Categorias!.Add(categoria);

        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult AtualizarPutCategoria(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
            return BadRequest();

        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteCategoria(int id)
    {
        var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == id);
        if (categoria is null)
            return NotFound("Categoria não encontrado");

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();

        return Ok(categoria);
    }
}