using Domain.Interfaces;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Repositories
{
    public class DataFlowRepository : IDataFlowRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public DataFlowRepository(IMongoDatabase database, IOptions<MongoSettings> settings)
        {
            _collection = database.GetCollection<BsonDocument>(settings.Value.DataFlowCollectionName);
        }

        public async Task<string> CreateAsync(string dataFlowJson)
        {
            var document = BsonDocument.Parse(dataFlowJson);
            await _collection.InsertOneAsync(document);
            return document["_id"].ToString();
        }

        public async Task<string> GetByIdAsync(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            if (document == null) return null;

            return document.ToString();
        }

        public async Task UpdateAsync(string id, string dataFlowJson)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<BsonDocument>.Update.Set("nodes", BsonDocument.Parse(dataFlowJson)["nodes"])
            .Set("edges", BsonDocument.Parse(dataFlowJson)["edges"]);
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            await _collection.DeleteOneAsync(filter);
        }
    }
}
