using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository _repository;
    public CategoriasController(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        var categorias = _repository.GetCategorias();
        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetById(int id)
    {
        var categoriaById = _repository.GetCategoria(id);
        if (categoriaById is null)
            return NotFound("Categoria não encontrado");

        return Ok(categoriaById);
    }
    [HttpPost]
    public ActionResult CadastrarCategoria(Categoria categoria)
    {
        if (categoria is null)
            return BadRequest();
        var categoriaCriada = _repository.Create(categoria);


        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
    }

    [HttpPut("{id:int}")]
    public ActionResult AtualizarPutCategoria(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
            return BadRequest();
        _repository.Update(categoria);

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteCategoria(int id)
    {
        var categoria = _repository.GetCategoria(id);
        if (categoria is null)
            return NotFound("Categoria não encontrado");

        var categoriaExcluida = _repository.Delete(id);
        return Ok(categoriaExcluida);
    }
}