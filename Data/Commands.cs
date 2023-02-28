using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Commands<T> : ICommands<T> where T : DataEntityBase
    {
        private readonly IMongoCollection<T> collection;
        public Commands(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            collection = client.GetDatabase(databaseName).GetCollection<T>(collectionName);
        }

        public async Task<bool> AddOne(T item)
        {
            try
            {
                await collection.InsertOneAsync(item);
            }
            catch
            {
                return false;
            }
            return true;
            
        }

        public async Task<bool> RemoveOne(T item)
        {
            var filter = Builders<T>.Filter.Eq(g => g.Id, item.Id);
            var result = await collection.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }

        public async Task<bool> ReplaceOne(T item)
        {
            var filter = Builders<T>.Filter.Eq(g => g.Id, item.Id);
            var result = await collection.ReplaceOneAsync(filter, item);
            return result.IsAcknowledged;
        }
    }
}
