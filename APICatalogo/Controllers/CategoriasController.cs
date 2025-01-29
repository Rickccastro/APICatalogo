using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDTO>> GetAll()
    {
        var categorias = _unitOfWork.CategoriaRepository.GetAll();

        var categoriasDto = categorias.ToCategoriaDTOList();

        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategorias([FromQuery] CategoriasParameters categoriaParameters)
    {
        var listaCategorias = _unitOfWork.CategoriaRepository.GetCategoria(categoriaParameters);

        return ObterCategorias(listaCategorias);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas([FromQuery] CategoriaFiltroNome categoriaFiltro)
    {
        var categoriasFiltradas = _unitOfWork.CategoriaRepository.GetCategoriaFiltroNome(categoriaFiltro);
            
       return ObterCategorias(categoriasFiltradas);
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categoriaFiltro)
    {
        var metaData = new
        {
            categoriaFiltro.TotalCount,
            categoriaFiltro.PageSize,
            categoriaFiltro.CurrentPage,
            categoriaFiltro.TotalPages,
            categoriaFiltro.HasNext,
            categoriaFiltro.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));
        var categoriasDTO = categoriaFiltro.ToCategoriaDTOList();

        return Ok(categoriasDTO);
    }




    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> GetById(int id)
    {
        var categoriaById = _unitOfWork.CategoriaRepository.Get( c=> c.CategoriaId == id);
        if (categoriaById is null)
            return NotFound("Categoria não encontrado");

         var categoriaDto = categoriaById.ToCategoriaDTO();

        return Ok(categoriaDto);
    }
    [HttpPost]
    public ActionResult<CategoriaDTO> CadastrarCategoria(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
            return BadRequest();

        var categoria = categoriaDto.ToCategoria();

        var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria!);

        var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

        _unitOfWork.Commit();

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto!.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> AtualizarPutCategoria(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
            return BadRequest();

        var categoria = categoriaDto.ToCategoria();

        var categoriaAtualizada = _unitOfWork.CategoriaRepository.Update(categoria!);
        _unitOfWork.Commit();

        var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

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

        var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaExcluidaDto);
    }
}