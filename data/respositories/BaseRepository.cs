using System.Linq.Expressions;
using core.exceptions;
using core.settings;
using data.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace data.respositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
{
    protected readonly IMongoCollection<T> Collection;
    readonly MongoSettings settings;

    public BaseRepository(IOptions<MongoSettings> options)
    {
        settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.Database);
        Collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }

    #region Get

    public IQueryable<T> GetList(Expression<Func<T, bool>> predicate, out int count,
        int page = 1,
        int limit = 50,
        bool withCount = false)
    {
        if (limit > 1000)
            throw new DatabaseException("Limit çok fazla!");

        int startIndex = 0;
        count = 0;

        IQueryable<T>? query = Collection.AsQueryable().Where(predicate);

        if (withCount)
            count = query.Count();

        startIndex = limit * page - limit;
        startIndex = startIndex < 0 ? 0 : startIndex;
        query = query.Skip(startIndex).Take(limit);

        var data = query.AsNoTracking();

        return data;
    }

    public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return Collection.Find(predicate).FirstOrDefaultAsync();
    }

    public Task<T> GetByIdAsync(string id)
    {
        return Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    #endregion

    #region Create

    public async Task<T> CreateAsync(T entity)
    {
        entity.CreatedDate = DateTime.UtcNow;
        var options = new InsertOneOptions { BypassDocumentValidation = false };
        await Collection.InsertOneAsync(entity, options);
        return entity;
    }

    public async Task<bool> BulkCreateAsync(IEnumerable<T> entities)
    {
        var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
        return (await Collection.BulkWriteAsync((IEnumerable<WriteModel<T>>)entities, options)).IsAcknowledged;
    }
    #endregion

    #region Update

    public async Task<T> UpdateAsync(string id, T entity)
    {
        return await Collection.FindOneAndReplaceAsync(x => x.Id == id, entity);
    }

    public async Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate)
    {
        return await Collection.FindOneAndReplaceAsync(predicate, entity);
    }
    #endregion

    #region Delete

    //TODO: SoftDelete işlemleri için method yaz

    public async Task<T> DeleteAsync(T entity)
    {
        return await Collection.FindOneAndDeleteAsync(x => x.Id == entity.Id);
    }

    public async Task<T> DeleteAsync(string id)
    {
        return await Collection.FindOneAndDeleteAsync(x => x.Id == id);
    }

    public async Task<T> DeleteAsync(Expression<Func<T, bool>> filter)
    {
        return await Collection.FindOneAndDeleteAsync(filter);
    }

    #endregion
}