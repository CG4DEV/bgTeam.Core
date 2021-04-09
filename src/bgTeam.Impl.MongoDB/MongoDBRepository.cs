namespace bgTeam.Impl.MongoDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using bgTeam.Extensions;
    using global::MongoDB.Driver;

    public class MongoDBRepository : IMongoDBRepository
    {
        private readonly IMongoDatabase _db;

        public MongoDBRepository(IMongoDBClient client)
        {
            _db = client.GetDatabase();
        }

        public virtual T Get<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            predicate.CheckNull(nameof(predicate));

            var collection = _db.GetCollection<T>(typeof(T).Name);

            return collection.Find(predicate).FirstOrDefault();
        }

        public virtual async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            predicate.CheckNull(nameof(predicate));

            var collection = _db.GetCollection<T>(typeof(T).Name);
            var findResult = await collection.FindAsync(predicate);

            return await findResult.FirstOrDefaultAsync();
        }

        public virtual IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> predicate = null)
            where T : class
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);

            return collection.Find(predicate ?? (_ => true)).ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null)
            where T : class
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            var findResult = await collection.FindAsync(predicate ?? (_ => true));

            return await findResult.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetPageAsync<T>(int skip, int limit, Expression<Func<T, bool>> predicate)
            where T : class
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            var findResult = await collection.FindAsync(predicate ?? (_ => true), new FindOptions<T>
            {
                Skip = skip <= 0 ? null : (int?)skip,
                Limit = limit <= 0 ? null : (int?)limit,
            });

            return await findResult.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetPageAsync<T>(int skip, int limit, IList<ISort> sort, params Expression<Func<T, bool>>[] predicates)
            where T : class
        {
            FilterDefinition<T>[] filters;
            var builder = Builders<T>.Filter;

            if (predicates.NullOrEmpty())
            {
                filters = new[] { builder.Where(_ => true) };
            }
            else
            {
                filters = predicates.Select(x => builder.Where(x)).ToArray();
            }

            var collection = _db.GetCollection<T>(typeof(T).Name);
            var filter = builder.And(filters);

            var findResult = await collection.FindAsync(filter, new FindOptions<T>
            {
                Sort = SortGenerate<T>(sort),
                Skip = skip <= 0 ? null : (int?)skip,
                Limit = limit <= 0 ? null : (int?)limit,
            });

            return await findResult.ToListAsync();
        }

        public virtual async Task<IEnumerable<V>> GetAllWithProjectionAsync<T, V>(Expression<Func<T, V>> result, Expression<Func<T, bool>> predicate = null)
            where T : class
        {
            result.CheckNull(nameof(result));

            var collection = _db.GetCollection<T>(typeof(T).Name);
            var res = collection.Find(predicate ?? (_ => true));

            return await res.Project(result).ToListAsync();
        }

        public virtual void Insert<T>(T document)
            where T : class
        {
            document.CheckNull(nameof(document));

            var collection = _db.GetCollection<T>(typeof(T).Name);

            collection.InsertOne(document);
        }

        public virtual async Task InsertAsync<T>(T document)
            where T : class
        {
            document.CheckNull(nameof(document));

            var collection = _db.GetCollection<T>(typeof(T).Name);

            await collection.InsertOneAsync(document);
        }

        public virtual void InsertMany<T>(IEnumerable<T> documents)
            where T : class
        {
            documents.CheckNull(nameof(documents));

            if (documents.Any())
            {
                var collection = _db.GetCollection<T>(typeof(T).Name);

                collection.InsertMany(documents);
            }
        }

        public virtual async Task InsertManyAsync<T>(IEnumerable<T> documents)
            where T : class
        {
            documents.CheckNull(nameof(documents));

            if (documents.Any())
            {
                var collection = _db.GetCollection<T>(typeof(T).Name);

                await collection.InsertManyAsync(documents);
            }
        }

        public virtual bool Delete<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            predicate.CheckNull(nameof(predicate));

            var collection = _db.GetCollection<T>(typeof(T).Name);

            var result = collection.DeleteOne(predicate);

            return result.IsAcknowledged;
        }

        public virtual async Task<bool> DeleteAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            predicate.CheckNull(nameof(predicate));

            var collection = _db.GetCollection<T>(typeof(T).Name);

            var result = await collection.DeleteOneAsync(predicate);

            return result.IsAcknowledged;
        }

        public virtual bool DeleteMany<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            predicate.CheckNull(nameof(predicate));

            var collection = _db.GetCollection<T>(typeof(T).Name);

            var result = collection.DeleteMany(predicate);

            return result.IsAcknowledged;
        }

        public virtual async Task<bool> DeleteManyAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            predicate.CheckNull(nameof(predicate));

            var collection = _db.GetCollection<T>(typeof(T).Name);

            var result = await collection.DeleteManyAsync(predicate);

            return result.IsAcknowledged;
        }

        public virtual bool Update<T>(Expression<Func<T, bool>> predicate, T entity, bool isUpsert = false)
            where T : class
        {
            predicate.CheckNull(nameof(predicate));

            var collection = _db.GetCollection<T>(typeof(T).Name);

            var result = collection.ReplaceOne<T>(predicate, entity, new UpdateOptions { IsUpsert = isUpsert });

            return result.IsAcknowledged;
        }

        public virtual async Task<bool> UpdateAsync<T>(Expression<Func<T, bool>> predicate, T entity, bool isUpsert = false)
            where T : class
        {
            predicate.CheckNull(nameof(predicate));

            var collection = _db.GetCollection<T>(typeof(T).Name);

            var result = await collection.ReplaceOneAsync<T>(predicate, entity, new UpdateOptions { IsUpsert = isUpsert });

            return result.IsAcknowledged;
        }

        public virtual async Task<bool> UpdateAsync<T>(FilterDefinition<T> filter, UpdateDefinition<T> update)
            where T : class
        {
            filter.CheckNull(nameof(filter));
            update.CheckNull(nameof(update));

            var collection = _db.GetCollection<T>(typeof(T).Name);

            var result = await collection.UpdateOneAsync(filter, update);

            return result.IsAcknowledged;
        }

        public virtual long Count<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);

            return collection.CountDocuments<T>(predicate ?? (_ => true));
        }

        public virtual async Task<long> CountAsync<T>(params Expression<Func<T, bool>>[] predicates)
            where T : class
        {
            FilterDefinition<T>[] filters;
            var builder = Builders<T>.Filter;

            if (predicates.NullOrEmpty())
            {
                filters = new[] { builder.Where(_ => true) };
            }
            else
            {
                filters = predicates.Select(x => builder.Where(x)).ToArray();
            }

            var collection = _db.GetCollection<T>(typeof(T).Name);
            var filter = builder.And(filters);

            return await collection.CountDocumentsAsync(filter);
        }

        private SortDefinition<T> SortGenerate<T>(IList<ISort> sort)
        {
            sort.CheckNull(nameof(sort));

            List<SortDefinition<T>> mongoSort = new List<SortDefinition<T>>();

            foreach (ISort s in sort)
            {
                var field = new StringFieldDefinition<T>(s.PropertyName);
                if (s.Ascending)
                {
                    mongoSort.Add(new SortDefinitionBuilder<T>().Ascending(field));
                }
                else
                {
                    mongoSort.Add(new SortDefinitionBuilder<T>().Descending(field));
                }
            }

            return new SortDefinitionBuilder<T>().Combine(mongoSort.ToArray());
        }
    }
}
