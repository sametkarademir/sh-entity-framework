using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SoftwareHospital.EntityFramework.Aggregates.Entities;
using SoftwareHospital.EntityFramework.Extensions;
using SoftwareHospital.EntityFramework.Models;
using SoftwareHospital.EntityFramework.Repositories.Interface;

namespace SoftwareHospital.EntityFramework.Repositories.Concrete;

public class ReadRepository<TEntity, TKey, TContext>(TContext context) :
    IReadRepository<TEntity, TKey>, IQuery<TEntity>
    where TEntity : class, IEntity<TKey>
    where TContext : DbContext
{
    public IQueryable<TEntity> Query()
    {
        return context.Set<TEntity>();
    }
    
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int index = 0,
        int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            queryable = orderBy(queryable);
        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.AnyAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.CountAsync(cancellationToken);
    }
}

public class ReadRepository<TEntity, TContext>(TContext context) :
    IReadRepository<TEntity>, IQuery<TEntity>
    where TEntity : class, IEntity
    where TContext : DbContext
{
    public IQueryable<TEntity> Query()
    {
        return context.Set<TEntity>();
    }
    
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int index = 0,
        int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            queryable = orderBy(queryable);
        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }
    
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.AnyAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.CountAsync(cancellationToken);
    }
}