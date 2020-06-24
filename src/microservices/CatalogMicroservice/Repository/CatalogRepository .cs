using CatalogMicroservice.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatalogMicroservice.Repository
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IMongoCollection<CatalogItem> _col;

        public CatalogRepository(IMongoDatabase db)
        {
            _col = db.GetCollection<CatalogItem>(CatalogItem.DocumentName);
        }

        public IEnumerable<CatalogItem> GetCatalogItems()
        {
            var catalogItems = _col.Find(FilterDefinition<CatalogItem>.Empty).ToEnumerable();
            return catalogItems;
        }

        public CatalogItem GetCatalogItem(Guid catalogItemId)
        {
            var catalogItem = _col.Find(c => c.Id == catalogItemId).FirstOrDefault();
            return catalogItem;
        }

        public void InsertCatalogItem(CatalogItem catalogItem)
        {
            _col.InsertOne(catalogItem);
        }

        public void UpdateCatalogItem(CatalogItem catalogItem)
        {
            var update = Builders<CatalogItem>.Update
                    .Set(c => c.Name, catalogItem.Name)
                    .Set(c => c.Description, catalogItem.Description)
                    .Set(c => c.Price, catalogItem.Price);
            _col.UpdateOne(c => c.Id == catalogItem.Id, update);
        }

        public void DeleteCatalogItem(Guid catalogItemId)
        {
            _col.DeleteOne(c => c.Id == catalogItemId);
        }
    }
}
