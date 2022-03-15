using CatalogAPI.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.Bson;
namespace CatalogAPI.Data
{
    public static class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> Products)
        {
            Products.InsertManyAsync(PreConf());
        }

        public static IEnumerable<Product> PreConf()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "T-Shirt",
                    Category="Summer",
                    Description ="Just a blank t-shirt",
                    ImageFile ="tshirt.jpg",
                    Price=200
                },
                new Product()
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Jacket",
                    Category="Spring",
                    Description ="Coll jacket",
                    ImageFile ="jacket.jpg",
                    Price=1500
                }
            };
        }
    }
}
