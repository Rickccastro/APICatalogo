using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

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

    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAll()
    {
        var categorias = await _unitOfWork.CategoriaRepository.GetAllAsync();

        var categoriasDto = categorias.ToCategoriaDTOList();

        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public async Task <ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias([FromQuery] CategoriasParameters categoriaParameters)
    {
        var listaCategorias = await _unitOfWork.CategoriaRepository.GetCategoriaAsync(categoriaParameters);

        return ObterCategorias(listaCategorias);
    }

    [HttpGet("filter/nome/pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas([FromQuery] CategoriaFiltroNome categoriaFiltro)
    {
        var categoriasFiltradas = await _unitOfWork.CategoriaRepository.GetCategoriaFiltroNomeAsync(categoriaFiltro);
            
       return ObterCategorias(categoriasFiltradas);
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categoriaFiltro)
    {
        var metaData = new
        {
            categoriaFiltro.Count,
            categoriaFiltro.PageSize,
            categoriaFiltro.PageCount,
            categoriaFiltro.TotalItemCount,
            categoriaFiltro.HasNextPage,
            categoriaFiltro.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));
        var categoriasDTO = categoriaFiltro.ToCategoriaDTOList();

        return Ok(categoriasDTO);
    }




    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoriaDTO>> GetById(int id)
    {
        var categoriaById =  await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
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

        _unitOfWork.CommitAsync();

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto!.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> AtualizarPutCategoria(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
            return BadRequest();

        var categoria = categoriaDto.ToCategoria();

        var categoriaAtualizada = _unitOfWork.CategoriaRepository.Update(categoria!);
        _unitOfWork.CommitAsync();

        var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> DeleteCategoria(int id)
    {
        var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
       
        if (categoria is null)
            return NotFound("Categoria não encontrado");

        var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
       await _unitOfWork.CommitAsync();

        var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaExcluidaDto);
    }
}