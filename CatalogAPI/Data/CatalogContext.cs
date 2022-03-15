using CatalogAPI.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using CatalogAPI.Data.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace CatalogAPI.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }
        public CatalogContext(IConfiguration DbSettings)
        {
            var client = new MongoClient(DbSettings.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(DbSettings.GetValue<string>("DatabaseSettings:DatabaseName"));

            Products = database.GetCollection<Product>(DbSettings.GetValue<string>("DatabaseSettings:CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }
    }
}
