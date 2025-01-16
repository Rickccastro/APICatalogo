﻿using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Produto> GetProdutos()
    {
        return _context.Produtos!;
    }

    public Produto GetProduto(int id)
    {
        return _context.Produtos!.FirstOrDefault(produtos => produtos.ProdutoId == id)!;
    }

    public Produto Create(Produto produto)
    {
        if (produto == null)
            throw new ArgumentException(nameof(produto));

        _context.Produtos!.Add(produto);
        _context.SaveChanges();
        return produto;

    }

    public bool Update(Produto produto)
    {
        if (produto == null)
            throw new ArgumentException(nameof(produto));

       if(_context.Produtos.Any(p => p.ProdutoId == produto.ProdutoId))
        {
            _context.Produtos!.Update(produto);
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public bool Delete(int id)
    {
        var produto = _context.Produtos.Find(id);
        if (produto is not null)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return true;
        }

        return false;
    }
}
