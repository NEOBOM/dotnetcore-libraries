using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;

namespace Library.Framework.MongoDB
{
    public abstract class DataContext<TEntity> : DbClient where TEntity : class
    {
        public DataContext(IMongoClient mongoClient, string mongoDbName, string collectionName) : base(mongoClient, mongoDbName, collectionName)
        {
        }

        protected TEntity Find(Expression<Func<TEntity, bool>> expression)
        {
            return Collection<TEntity>().Find(expression).FirstOrDefault();
        }

        protected async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Collection<TEntity>().Find(expression).FirstOrDefaultAsync();
        }

        protected List<TEntity> Select(Expression<Func<TEntity, bool>> expression)
        {
            return Collection<TEntity>().Find(expression).ToList();
        }

        protected async Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Collection<TEntity>().Find(expression).ToListAsync();
        }

        protected async Task<List<TEntity>> SelectAsync<T>(Func<T, Expression<Func<TEntity, bool>>> expression, List<T> entities) where T : class
        {
            var filters = Builders<TEntity>.Filter.And(entities?.Select(entity => Builders<TEntity>.Filter.Where(expression(entity))));

            return await Collection<TEntity>().Find(filters).ToListAsync();
        }

        protected List<TEntity> SelectAll()
        {
            return Collection<TEntity>().Find(_ => true).ToList();
        }

        protected async Task<List<TEntity>> SelectAllAsync()
        {
            return await Collection<TEntity>().Find(_ => true).ToListAsync();
        }

        protected void InsertOne(TEntity entity)
        {
            Collection<TEntity>().InsertOne(entity);
        }

        protected async Task InsertOneAsync(TEntity entity)
        {
            await Collection<TEntity>().InsertOneAsync(entity);
        }

        protected void InsertMany(List<TEntity> entities)
        {
            Collection<TEntity>().InsertMany(entities);
        }

        protected async Task InsertManyAsync(List<TEntity> entities)
        {
            await Collection<TEntity>().InsertManyAsync(entities, new InsertManyOptions { IsOrdered = true });
        }

        protected void UpdateOne(Expression<Func<TEntity, bool>> expression, TEntity entity)
        {
            Collection<TEntity>().ReplaceOne(expression, entity, new UpdateOptions { IsUpsert = false });
        }

        protected async Task UpdateOneAsync(Expression<Func<TEntity, bool>> expression, TEntity entity)
        {
            await Collection<TEntity>().ReplaceOneAsync(expression, entity);
        }

        protected bool BulkWrite(Func<TEntity, Expression<Func<TEntity, bool>>> expression, List<TEntity> entities)
        {
            var bulkOps = entities?.Select(entity => new ReplaceOneModel<TEntity>(expression(entity), entity) { IsUpsert = true });

            if (bulkOps != null && bulkOps.Any())
                return Collection<TEntity>().BulkWrite(bulkOps).IsAcknowledged;

            return false;
        }

        protected async Task<bool> BulkWriteAsync(Func<TEntity, Expression<Func<TEntity, bool>>> expression, List<TEntity> entities)
        {
            var bulkOps = entities?.Select(entity => new ReplaceOneModel<TEntity>(expression(entity), entity) { IsUpsert = true });

            if (bulkOps != null && bulkOps.Any())
            {
                var result = await Collection<TEntity>().BulkWriteAsync(bulkOps);

                return result.IsAcknowledged;
            }

            return false;
        }

        protected bool DeleteOne(Expression<Func<TEntity, bool>> expression)
        {
            return Collection<TEntity>().DeleteOne(expression).IsAcknowledged;
        }

        protected async Task<bool> DeleteOneAsync(Expression<Func<TEntity, bool>> expression)
        {
            var result = await Collection<TEntity>().DeleteOneAsync(expression);

            return result.IsAcknowledged;
        }

        protected bool DeleteMany(Expression<Func<TEntity, bool>> expression)
        {
            return Collection<TEntity>().DeleteMany(expression).IsAcknowledged;
        }

        protected async Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> expression)
        {
            var result = await Collection<TEntity>().DeleteManyAsync(expression);

            return result.IsAcknowledged;
        }

        //public void RegistrarClasseBson<T>()
        //{
        //    if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
        //    {
        //        BsonClassMap.RegisterClassMap<T>(cm =>
        //        {
        //            cm.AutoMap();
        //            cm.SetIgnoreExtraElements(true);
        //        });
        //    }
        //}

        protected async Task ChangeStreamsAsync(ChangeStreamOperationType changeStreamOperationType, Action<TEntity> action)
        {
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<TEntity>>().Match(x => x.OperationType == changeStreamOperationType);

            using (var cursor = await Collection<TEntity>().WatchAsync(pipeline))
            {
                await cursor.ForEachAsync(change =>
                {
                    if (change.OperationType == changeStreamOperationType)
                        action(change.FullDocument);
                });
            }
        }

        protected void ChangeStreamsSync(ChangeStreamOperationType changeStreamOperationType, Action<TEntity> action)
        {
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<TEntity>>().Match(x => x.OperationType == changeStreamOperationType);

            using (var cursor = Collection<TEntity>().Watch(pipeline))
            {
                cursor.ForEachAsync(change =>
                {
                    if (change.OperationType == changeStreamOperationType)
                        action(change.FullDocument);
                });
            }
        }
    }
}
