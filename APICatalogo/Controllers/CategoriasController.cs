using APICatalogo.DTOs;
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
    public ActionResult<IEnumerable<CategoriaDTO>> GetAll()
    {
        var categorias = _unitOfWork.CategoriaRepository.GetAll();
       
        var listaCategoriasDto = new List<CategoriaDTO>();

        foreach(var categoria in categorias)
        {
            var categoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                ImageUrl = categoria.ImageUrl,
                Nome = categoria.Nome,
            };
            listaCategoriasDto.Add(categoriaDto);
        }
        
        
        return Ok(listaCategoriasDto);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> GetById(int id)
    {
        var categoriaById = _unitOfWork.CategoriaRepository.Get( c=> c.CategoriaId == id);
        if (categoriaById is null)
            return NotFound("Categoria não encontrado");
        var categoriaDto = new CategoriaDTO()
        {
            CategoriaId = categoriaById.CategoriaId,
            ImageUrl = categoriaById.ImageUrl,
            Nome = categoriaById.Nome,
        };
        return Ok(categoriaById);
    }
    [HttpPost]
    public ActionResult<CategoriaDTO> CadastrarCategoria(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
            return BadRequest();

        var categoria = new Categoria()
        {
            CategoriaId = categoriaDto.CategoriaId,
            ImageUrl= categoriaDto.ImageUrl,
            Nome= categoriaDto.Nome,      
        };
        var novaCategoriaDto = new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            ImageUrl = categoria.ImageUrl,
            Nome = categoria.Nome,
        };

        var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
        _unitOfWork.Commit();

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> AtualizarPutCategoria(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
            return BadRequest();

        var categoria = new Categoria()
        {
            CategoriaId = categoriaDto.CategoriaId,
            ImageUrl = categoriaDto.ImageUrl,
            Nome = categoriaDto.Nome,
        };

        var categoriaAtualizada = _unitOfWork.CategoriaRepository.Update(categoria);
        _unitOfWork.Commit();

        var categoriaAtualizadaDto = new CategoriaDTO()
        {
            Nome = categoriaAtualizada.Nome,
            ImageUrl= categoriaAtualizada.ImageUrl,
            CategoriaId = categoriaAtualizada.CategoriaId 
        };

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> DeleteCategoria(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
        if (categoria is null)
            return NotFound("Categoria não encontrado");

        var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
        _unitOfWork.Commit();

        var categoriaExcluidaDto = new CategoriaDTO()
        {
            Nome = categoriaExcluida.Nome,
            ImageUrl = categoriaExcluida.ImageUrl,
            CategoriaId = categoriaExcluida.CategoriaId
        };

        return Ok(categoriaExcluidaDto);
    }
}