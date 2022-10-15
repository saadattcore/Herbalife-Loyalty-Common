using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace HL.Loyalty.EventStore
{
    public class AzureDocumentDbEventStore : IEventStore
    {
        private readonly Uri _serviceEndPoint;
        private readonly string _authKey;
        private readonly string _dataBaseId;
        private readonly string _collectionId;
        private readonly DocumentClient _client;
        private readonly FeedOptions _queryOptions;

        public AzureDocumentDbEventStore(Uri serviceEndPoint, string authKey, string dataBaseId, string collectionId)
        {
            if (serviceEndPoint == null)
            {
                throw new ArgumentException("Service end point can't be null", nameof(serviceEndPoint));
            }

            if (authKey == null)
            {
                throw new ArgumentException("Auth key can't be null", nameof(authKey));
            }

            if (dataBaseId == null)
            {
                throw new ArgumentException("Database id can't be null", nameof(dataBaseId));
            }

            if (collectionId == null)
            {
                throw new ArgumentException("Collection id can't be null", nameof(collectionId));
            }

            _serviceEndPoint = serviceEndPoint;
            _authKey = authKey;
            _dataBaseId = dataBaseId;
            _collectionId = collectionId;
            _client = new DocumentClient(_serviceEndPoint, _authKey);
            _queryOptions = new FeedOptions
            {
                MaxItemCount = -1,
                EnableCrossPartitionQuery = true,
                MaxDegreeOfParallelism = 10,
                MaxBufferedItemCount = 100
            };
        }

        public async Task<bool> SaveAsync<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("Object can't be null", nameof(obj));
            }

            var saveResult =
                await _client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(_dataBaseId, _collectionId),
                    obj);
            var result = saveResult.StatusCode == HttpStatusCode.Created || saveResult.StatusCode == HttpStatusCode.OK;

            return result;
        }

        public async Task<bool> SaveAsync<T>(T obj, string partitionKey)
        {
            if (obj == null)
            {
                throw new ArgumentException("Object can't be null", nameof(obj));
            }

            var options = new RequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            };

            var saveResult =
                await _client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(_dataBaseId, _collectionId),
                    obj, options);
            var result = saveResult.StatusCode == HttpStatusCode.Created || saveResult.StatusCode == HttpStatusCode.OK;

            return result;
        }

        public List<T> Read<T>(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id can't be null or empty", nameof(id));
            }

            IQueryable<T> query =
                _client.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(_dataBaseId, _collectionId),
                    $"SELECT * FROM {typeof(T).Name} WHERE {typeof(T).Name}.Id = '{id}'",
                    _queryOptions);

            return query.ToList();
        }

        public List<T> Read<T>(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("Expression can't be null", nameof(expression));
            }
            IQueryable<T> query = _client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(_dataBaseId, _collectionId), _queryOptions)
                .Where(expression);

            return query.ToList();
        }

        public void CreateDatabase(string databaseId)
        {
            if (string.IsNullOrWhiteSpace(databaseId))
            {
                throw new ArgumentException("DatabaseId can't be null or empty", nameof(databaseId));
            }

            try
            {
                var r = _client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId)).Result;
            }
            catch (AggregateException de)
            {
                if (de.InnerException != null && de.InnerException.Message.Contains("Resource Not Found"))
                {
                    var r = _client.CreateDatabaseAsync(new Database {Id = databaseId}).Result;
                }
            }
        }

        public void CreateCollection(string databaseId, string collectionId, string[] partitionKeys)
        {
            if (string.IsNullOrWhiteSpace(databaseId))
            {
                throw new ArgumentException("DatabaseId can't be null or empty", nameof(databaseId));
            }

            if (string.IsNullOrWhiteSpace(collectionId))
            {
                throw new ArgumentException("CollectionId can't be null or empty", nameof(collectionId));
            }

            try
            {
                var r =
                    _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId))
                        .Result;
            }
            catch (AggregateException de)
            {
                if (de.InnerException != null && de.InnerException.Message.Contains("Resource Not Found"))
                {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionId;

                    if (partitionKeys != null)
                    {
                        for (int i = 0; i < partitionKeys.Length; i++)
                        {
                            collectionInfo.PartitionKey.Paths.Add(partitionKeys[i]);
                        }
                    }

                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) {Precision = -1});

                    var r = _client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseId),
                        collectionInfo,
                        new RequestOptions {OfferThroughput = 20000}).Result;
                }
            }
        }
    }
}
