using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Service;

public class MongoRepository<T> : IRepository<T> where T : BaseEntity
{
    public required IMongoDatabase mongoDatabase;
    public readonly IMongoCollection<T> _collection;

    public MongoRepository(IOptions<BookStoreDatabaseSettings> databaseSettings)
    {
        MongoClient mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

        string className = typeof(T).Name;
        mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<T>(className);
    }

    public async Task<List<T>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

    public async Task<T?> GetAsync(string entityId) =>
        await _collection.Find(x => x.Id == entityId).FirstOrDefaultAsync();

    public async Task CreateAsync(T newItem) =>
        await _collection.InsertOneAsync(newItem);
    public async Task CreateMultipleAsync(T[] newItem) =>
        await _collection.InsertManyAsync(newItem);

    public async Task UpdateAsync(string id, T updatedItem) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, updatedItem);

    public async Task RemoveAsync(string id) => await _collection.DeleteOneAsync(x => x.Id == id);
}
