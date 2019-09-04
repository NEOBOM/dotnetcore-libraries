using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;

namespace Library.Framework.MongoDB
{
    public abstract class DbClient
    {
        protected readonly string _collectionName;
        protected readonly IMongoDatabase _db = null;

        protected DbClient(IMongoClient mongoClient, string mongoDbName, string collectionName)
        {
            if (string.IsNullOrEmpty(mongoDbName))
                throw new Exception("DbName is empty. DbName is necessary for access a DataBase");

            _collectionName = collectionName;
            _db = mongoClient.GetDatabase(mongoDbName);

            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("onventions", pack, t => true);
        }

        protected DbClient(IMongoDatabase mongoDatabase)
        {
            _db = mongoDatabase;
        }

        protected IMongoCollection<TEntity> Collection<TEntity>() where TEntity : class => _db.GetCollection<TEntity>(_collectionName);

        protected void CreateCollectionAndIndexes()
        {
            //_db.CreateCollectionAsync(_collectinoName, );
            //_db.CreateCollectionAsync<CobrancaExcessivaItemDocument>(_collectinoName, true, (long)Math.Pow(1024, 3), _maxDocs);
            //_databaseFacade.CreateIndexIfNotExistsAsync<CobrancaExcessivaItemDocument>(_collectinoName, "consulta-relatorio", null,
            //      e => e.IdTipoEntrega
            //    , e => e.IdUnidadeNegocio
            //    , e => e.IdCategoria
            //    , e => e.IdDepartamento
            //    , e => e.CnpjFilial
            //    , e => e.IdSku
            //    , e => e.DataCalculo);
        }
    }
}
