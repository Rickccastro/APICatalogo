using APICatalogo.DTOs;
using APICatalogo.DTOs.Patch;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
    public async Task <ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos([FromQuery] ProdutosParameters produtosParameters)
    {
        var listaProdutos = await _unitOfWork.ProdutoRepository.GetProdutosAsync(produtosParameters);

 
        ObterProdutos(listaProdutos);

        var listaProdutosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutos);
        return Ok(listaProdutosDTO);
    }  
    [HttpGet("filter/preco/pagination")]
    public async Task <ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco produtosFilterParams)
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

    [Authorize]
    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> GetAsync()
    {
        var listaProdutos = _unitOfWork.ProdutoRepository.GetAllAsync();
        if (listaProdutos is null)
            return NotFound();
        var listaProdutosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutos);

        return Ok(listaProdutosDTO);
    }
    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> GetByIdAsync(int id)
    {
        var produtoById = _unitOfWork.ProdutoRepository.GetAsync(p => p.ProdutoId == id);
        if (produtoById is null)
            return NotFound("Produto não encontrado");

        var produtoDTOById = _mapper.Map<ProdutoDTO>(produtoById);
        return Ok(produtoDTOById);
    }
    [HttpPost]
    public async Task <ActionResult<ProdutoDTO>> CadastrarProduto([FromBody] ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDto);

        var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
        await _unitOfWork.CommitAsync();

        var novoProdutoDTO = _mapper.Map<ProdutoDTO>(novoProduto);

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDTO.ProdutoId }, novoProdutoDTO);
    }


    [HttpPatch("{id:int}/UpdatePartial")]
    public async Task <ActionResult<ProdutoDTOUpdateResponse>> AtualizarPatchProduto(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if (patchProdutoDTO is null || id <= 0)
            return BadRequest();

        var produto = await _unitOfWork.ProdutoRepository.GetAsync(c => c.ProdutoId == id);
        if (produto == null)
          return NotFound();

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
        
        patchProdutoDTO.ApplyTo(produtoUpdateRequest,ModelState);

        if (!ModelState.IsValid || TryValidateModel(produtoUpdateRequest))
            return BadRequest(ModelState);

        _mapper.Map(produtoUpdateRequest,produto);

        _unitOfWork.ProdutoRepository.Update(produto);
        await _unitOfWork.CommitAsync();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));    
    }

    [HttpPut("{id:int}")]
    public async Task <ActionResult<ProdutoDTO>> AtualizarPutProduto(int id, ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.ProdutoId)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDTO);

        var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
        await _unitOfWork.CommitAsync();

        var produtoAtualizadoDTO = _mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(produtoAtualizadoDTO);
    }

    [HttpDelete("{id:int}")]
    public async Task <ActionResult<ProdutoDTO>> DeleteProduto(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository.GetAsync(p => p.ProdutoId == id);
        if (produto is null)
            return Ok($"Produto de id = {id} foi excluído");

        var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
        await _unitOfWork.CommitAsync();

        var produtoDeletadoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);
        return Ok(produtoDeletadoDTO);
    }
}
