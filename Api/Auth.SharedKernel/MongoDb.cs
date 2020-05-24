using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Auth.SharedKernel
{
    public class MongoDb
    {
        private readonly MongoClient _mongoClient;
        private readonly string _dataBase;

        public MongoDb(string connectionString, string dataBase)
        {
            _dataBase = dataBase;
            _mongoClient = new MongoClient(connectionString);
        }

        private IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return _mongoClient.GetDatabase(_dataBase).GetCollection<BsonDocument>(collectionName);
        }

        public IEnumerable<T> Get<T>(string collectionName)
        {
            var query = GetCollection(collectionName).Find(x => true)
                .Project<BsonDocument>(Builders<BsonDocument>.Projection.Exclude(doc => doc[""]))
                .ToCursorAsync().Result;

            query.MoveNextAsync();
            return query.Current.Select(x => BsonSerializer.Deserialize<T>(x));
        }


        public IEnumerable<T> Get<T>(string collectionName, BsonDocument filter)
        {
            var query = GetCollection(collectionName).Find(filter)
                .Project<BsonDocument>(Builders<BsonDocument>.Projection.Exclude(doc => doc[""]))
                .ToCursorAsync().Result;

            query.MoveNextAsync();
            return query.Current.Select(x => BsonSerializer.Deserialize<T>(x));
        }
        public IEnumerable<T> GetWithId<T>(string collectionName)
        {
            var query = GetCollection(collectionName).Find(x => true)
                .Project<BsonDocument>(Builders<BsonDocument>.Projection.Include(doc => doc[""]))
                .ToCursorAsync().Result;

            query.MoveNextAsync();
            return query.Current.Select(x => BsonSerializer.Deserialize<T>(x));
        }

        public IEnumerable<T> GetAll<T>(string collectionName, BsonDocument filter)
        {
            var query = GetCollection(collectionName).Find(filter)
                .Project<BsonDocument>(Builders<BsonDocument>.Projection.Exclude(doc => doc[""]));

            return query.ToList().Select(x => BsonSerializer.Deserialize<T>(x));
        }

        public IEnumerable<T> GetAll<T>(string collectionName, Expression<Func<BsonDocument, bool>> filter)
        {
            var query = GetCollection(collectionName).Find(filter)
                .Project<BsonDocument>(Builders<BsonDocument>.Projection.Exclude(doc => doc[""]));

            return query.ToList().Select(x => BsonSerializer.Deserialize<T>(x));
        }

        public IEnumerable<T> GetAll<T>(string collectionName)
        {
            var query = GetCollection(collectionName).Find(x => true)
                .Project<BsonDocument>(Builders<BsonDocument>.Projection.Exclude(doc => doc[""]));

            return query.ToList().Select(x => BsonSerializer.Deserialize<T>(x));
        }

        public void Insert(string collectionName, List<BsonDocument> itens)
        {
            GetCollection(collectionName).InsertMany(itens);
        }

        public void Insert(string collectionName, object item)
        {
            GetCollection(collectionName).InsertMany(new[] { item.ToBsonDocument() });
        }

        // public void Update(string collectionName, BsonDocument filter, object replacement)
        // {
        //     GetCollection(collectionName).ReplaceOne(filter, replacement.ToBsonDocument(), new UpdateOptions { IsUpsert = true });
        // }
    }
}