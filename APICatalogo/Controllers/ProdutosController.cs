using APICatalogo.DTOs;
using APICatalogo.DTOs.Patch;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Pagination.Filters;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutoByCategoria(int id)
    {
        var listaProdutosByCategoria = _unitOfWork.ProdutoRepository.GetProdutoByCategoria(id);
        if (listaProdutosByCategoria is null)
            return NotFound();
        var listaProdutosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutosByCategoria);

        return Ok(listaProdutosDTO);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutos([FromQuery] ProdutosParameters produtosParameters)
    {
        var listaProdutos = _unitOfWork.ProdutoRepository.GetProdutos(produtosParameters);

 
        ObterProdutos(listaProdutos);

        var listaProdutosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutos);
        return Ok(listaProdutosDTO);
    }  
    [HttpGet("filter/preco/pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco produtosFilterParams)
    {
        var listaProdutos = _unitOfWork.ProdutoRepository.GetProdutosFiltroPreco(produtosFilterParams);

        ObterProdutos(listaProdutos);

        var listaProdutosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutos);
        return Ok(listaProdutosDTO);
    }

    private void ObterProdutos(PagedList<Produto> listaProdutos)
    {
        var metaData = new
        {
            listaProdutos.TotalCount,
            listaProdutos.PageSize,
            listaProdutos.CurrentPage,
            listaProdutos.TotalPages,
            listaProdutos.HasNext,
            listaProdutos.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> GetAsync()
    {
        var listaProdutos = _unitOfWork.ProdutoRepository.GetAll();
        if (listaProdutos is null)
            return NotFound();
        var listaProdutosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(listaProdutos);

        return Ok(listaProdutosDTO);
    }
    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> GetByIdAsync(int id)
    {
        var produtoById = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);
        if (produtoById is null)
            return NotFound("Produto não encontrado");

        var produtoDTOById = _mapper.Map<ProdutoDTO>(produtoById);
        return Ok(produtoDTOById);
    }
    [HttpPost]
    public ActionResult<ProdutoDTO> CadastrarProduto([FromBody] ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDto);

        var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
        _unitOfWork.Commit();

        var novoProdutoDTO = _mapper.Map<ProdutoDTO>(novoProduto);

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDTO.ProdutoId }, novoProdutoDTO);
    }


    [HttpPatch("{id:int}/UpdatePartial")]
    public ActionResult<ProdutoDTOUpdateResponse> AtualizarPatchProduto(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if (patchProdutoDTO is null || id <= 0)
            return BadRequest();

        var produto = _unitOfWork.ProdutoRepository.Get(c => c.ProdutoId == id);
        if (produto == null)
          return NotFound();

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
        
        patchProdutoDTO.ApplyTo(produtoUpdateRequest,ModelState);

        if (!ModelState.IsValid || TryValidateModel(produtoUpdateRequest))
            return BadRequest(ModelState);

        _mapper.Map(produtoUpdateRequest,produto);

        _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));    
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> AtualizarPutProduto(int id, ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.ProdutoId)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDTO);

        var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        var produtoAtualizadoDTO = _mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(produtoAtualizadoDTO);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> DeleteProduto(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);
        if (produto is null)
            return Ok($"Produto de id = {id} foi excluído");

        var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
        _unitOfWork.Commit();

        var produtoDeletadoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);
        return Ok(produtoDeletadoDTO);
    }
}
