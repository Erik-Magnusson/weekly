using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Data
{
    public class Queries<T> : IQueries<T> where T : DataEntityBase
    {
        private readonly IMongoCollection<T> collection;
        public Queries(string connectionString, string databaseName, string collectionName)
        { 
            var client = new MongoClient(connectionString);
            collection = client.GetDatabase(databaseName).GetCollection<T>(collectionName);
        }
        public async Task<IList<T>> GetAll()
        {
            var filter = Builders<T>.Filter.Empty;
            return await collection.Find(filter).ToListAsync();
        }

        public async Task<T?> GetOne(string id)
        {
            var filter = Builders<T>.Filter.Eq(g => g.Id, id);
            var result = await collection.Find(filter).ToListAsync();
            return result?.FirstOrDefault();
        }

        public async Task<T?> GetOne<U>(Expression<Func<T, U>> expression, U value)
        {
            var filter = Builders<T>.Filter.Eq(expression, value);
            var result = await collection.Find(filter).ToListAsync();
            return result?.FirstOrDefault();
        }
    }
}