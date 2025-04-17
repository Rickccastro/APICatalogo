using APICatalogo.DTOs;
using APICatalogo.DTOs.Patch;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiConventionType(typeof(DefaultApiConventions))]

[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly CacheService _cache;
    private string cacheKey = "ListaProdutos";


    public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper, CacheService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    [HttpGet("produtos/{id}")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutoByCategoria(int id)
    {

        var listaProdutosByCategoria = await _unitOfWork.ProdutoRepository.GetProdutoByCategoriaAsync(id);
        if (listaProdutosByCategoria is null)
            return NotFound();
        var listaProdutosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutosByCategoria);

        return Ok(listaProdutosDTO);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos([FromQuery] ProdutosParameters produtosParameters)
    {
        var listaProdutos = await _unitOfWork.ProdutoRepository.GetProdutosAsync(produtosParameters);


        ObterProdutos(listaProdutos);

        var listaProdutosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutos);
        return Ok(listaProdutosDTO);
    }
    [HttpGet("filter/preco/pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco produtosFilterParams)
    {
        var listaProdutos = await _unitOfWork.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosFilterParams);

        ObterProdutos(listaProdutos);

        var listaProdutosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutos);
        return Ok(listaProdutosDTO);
    }

    private void ObterProdutos(IPagedList<Produto> listaProdutos)
    {
        var metaData = new
        {
            listaProdutos.Count,
            listaProdutos.PageSize,
            listaProdutos.PageCount,
            listaProdutos.TotalItemCount,
            listaProdutos.HasNextPage,
            listaProdutos.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public ActionResult<IEnumerable<ProdutoDTO>> GetAsync()
    {
        var listaProdutoseDTOByCache = _cache.TryGetValue<IEnumerable<ProdutoDTO>>(cacheKey);
        if (listaProdutoseDTOByCache != null)
            return Ok(listaProdutoseDTOByCache);

        var listaProdutos = _unitOfWork.ProdutoRepository.GetAllAsync();
        if (listaProdutos is null)
            return NotFound();

        var listaProdutosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutos);

        _cache.Set(cacheKey, listaProdutosDTO, TimeSpan.FromMinutes(5));

        return Ok(listaProdutosDTO);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> GetByIdAsync(int id)
    {
        string cacheKey = $"Produto_{id}";

        var produtoDTOByIdCache = _cache.TryGetValue<ProdutoDTO>(cacheKey);
        if (produtoDTOByIdCache != null)
            return Ok(produtoDTOByIdCache);

        var produtoById = _unitOfWork.ProdutoRepository.GetAsync(p => p.ProdutoId == id);
        if (produtoById is null)
            return NotFound("Produto não encontrado");

        var produtoDTOById = _mapper.Map<ProdutoDTO>(produtoById);
        _cache.Set(cacheKey, produtoDTOById, TimeSpan.FromMinutes(5));

        return Ok(produtoDTOById);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProdutoDTO>> CadastrarProduto([FromBody] ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDto);

        var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
        await _unitOfWork.CommitAsync();

        var novoProdutoDTO = _mapper.Map<ProdutoDTO>(novoProduto);

        _cache.Set($"Produto_{novoProdutoDTO.ProdutoId}", novoProdutoDTO, TimeSpan.FromMinutes(5));
        _cache.Remove("ListaProdutos");

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDTO.ProdutoId }, novoProdutoDTO);
    }


    [HttpPatch("{id:int}/UpdatePartial")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProdutoDTOUpdateResponse>> AtualizarPatchProduto(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if (patchProdutoDTO is null || id <= 0)
            return BadRequest();

        var produto = await _unitOfWork.ProdutoRepository.GetAsync(c => c.ProdutoId == id);
        if (produto == null)
            return NotFound();

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

        patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);

        if (!ModelState.IsValid || TryValidateModel(produtoUpdateRequest))
            return BadRequest(ModelState);

        _mapper.Map(produtoUpdateRequest, produto);

        _unitOfWork.ProdutoRepository.Update(produto);
        await _unitOfWork.CommitAsync();

        _cache.Set($"Produto_{id}", produto, TimeSpan.FromMinutes(5));
        _cache.Remove("ListaProdutos");

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProdutoDTO>> AtualizarPutProduto(int id, ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.ProdutoId)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDTO);

        var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
        await _unitOfWork.CommitAsync();

        var produtoAtualizadoDTO = _mapper.Map<ProdutoDTO>(produtoAtualizado);

        _cache.Set($"Produto_{id}", produtoAtualizadoDTO, TimeSpan.FromMinutes(5));
        _cache.Remove("ListaProdutos");

        return Ok(produtoAtualizadoDTO);
    }

    [Authorize(AuthenticationSchemes = "Bearer", Policy = "Admin-Geral")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> DeleteProduto(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository.GetAsync(p => p.ProdutoId == id);
        if (produto is null)
            return StatusCode(StatusCodes.Status404NotFound, ("Produto não encontrado"));

        var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
        await _unitOfWork.CommitAsync();

        var produtoDeletadoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);

        _cache.Remove($"Produto_{id}");
        _cache.Remove("ListaProdutos");

        return Ok(produtoDeletadoDTO);
    }
}
