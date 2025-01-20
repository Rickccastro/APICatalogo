﻿using APICatalogo.Context;
using APICatalogo.Repositories.interfaces.GenericInterface;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories.methods;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking().ToList();
    }

    public T? Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefault(predicate);
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public T Update(T entity)
    {

        _context.Set<T>().Update(entity);
        //indicado quando for atualizar alguns campos não todas
        // _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
        return entity;
    }

}
