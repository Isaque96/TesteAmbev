using System.Collections;
using System.Collections.Specialized;
using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Generic repository implementation using Entity Framework Core
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DefaultContext Context;
    protected readonly DbSet<TEntity> DbSet;

    /// <summary>
    /// Initializes a new instance of Repository
    /// </summary>
    /// <param name="context">The database context</param>
    public Repository(DefaultContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    /// <summary>
    /// Creates a new entity in the database
    /// </summary>
    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier
    /// </summary>
    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync([id], cancellationToken);
    }

    /// <summary>
    /// Retrieves all entities
    /// </summary>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Finds entities based on a predicate
    /// </summary>
    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    /// Deletes an entity by its unique identifier
    /// </summary>
    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
            return false;

        DbSet.Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Checks if an entity exists based on a predicate
    /// </summary>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(predicate, cancellationToken);
    }
    
    /// <summary>
    /// Default order by
    /// </summary>
    /// <param name="query">Initial Query</param>
    /// <param name="order">Ordination ASC or DESC</param>
    /// <returns></returns>
    public static IQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> query, string? order)
    {
        var orderDict = GetKeysForOrderBy(order);
        if (orderDict.Count == 0)
            return query;

        IOrderedQueryable<TEntity>? orderedQuery = null;

        foreach (DictionaryEntry entry in orderDict)
        {
            var propertyName = (string)entry.Key;
            var direction = ((string?)entry.Value)?.ToLower() ?? "asc";

            // Cria expressão para acessar a propriedade
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            string methodName;

            if (orderedQuery == null)
            {
                methodName = direction == "desc" ? "OrderByDescending" : "OrderBy";
                var method = typeof(Queryable)
                    .GetMethods()
                    .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(TEntity), property.Type);

                orderedQuery = (IOrderedQueryable<TEntity>)method.Invoke(null, [query, lambda])!;
            }
            else
            {
                methodName = direction == "desc" ? "ThenByDescending" : "ThenBy";
                var method = typeof(Queryable)
                    .GetMethods()
                    .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(TEntity), property.Type);

                orderedQuery = (IOrderedQueryable<TEntity>)method.Invoke(null, [orderedQuery, lambda])!;
            }
        }

        return orderedQuery ?? query;
    }

    public static OrderedDictionary GetKeysForOrderBy(string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
            return new OrderedDictionary();

        order = order.Trim('"');
        
        var orders = order.Split(',', StringSplitOptions.TrimEntries);
        var dict = new OrderedDictionary();
        foreach (var ord in orders)
        {
            var keyValue = ord.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            dict[keyValue[0].ToLower()] = keyValue.Length switch
            {
                0 => throw new BadFormatException("_order", "property asc|desc"),
                1 => "asc",
                2 => keyValue[1],
                _ => throw new BadFormatException("_order", "property asc|desc")
            };
        }

        return dict;
    }
}