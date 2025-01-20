using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoriasController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<Categoria>> GetAll()
    {
        var categorias = _unitOfWork.CategoriaRepository.GetAll();
        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetById(int id)
    {
        var categoriaById = _unitOfWork.CategoriaRepository.Get( c=> c.CategoriaId == id);
        if (categoriaById is null)
            return NotFound("Categoria não encontrado");

        return Ok(categoriaById);
    }
    [HttpPost]
    public ActionResult CadastrarCategoria(Categoria categoria)
    {
        if (categoria is null)
            return BadRequest();
        var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
        _unitOfWork.Commit();

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
    }

    [HttpPut("{id:int}")]
    public ActionResult AtualizarPutCategoria(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
            return BadRequest();
        _unitOfWork.CategoriaRepository.Update(categoria);
        _unitOfWork.Commit();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteCategoria(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
        if (categoria is null)
            return NotFound("Categoria não encontrado");

        var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
        _unitOfWork.Commit();
        return Ok(categoriaExcluida);
    }
}