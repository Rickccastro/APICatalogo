using APICatalogo.Models;
using APICatalogo.Repositories.interfaces.GenericInterface;
using APICatalogo.Repositories.interfaces.SpecificInterface;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet()]
    public ActionResult<IEnumerable<Categoria>> GetAll()
    {
        var categorias = _repository.GetAll();
        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetById(int id)
    {
        var categoriaById = _repository.Get( c=> c.CategoriaId == id);
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
        var categoria = _repository.Get(c => c.CategoriaId == id);
        if (categoria is null)
            return NotFound("Categoria não encontrado");

        var categoriaExcluida = _repository.Delete(categoria);
        return Ok(categoriaExcluida);
    }
}