using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogAPI.Entities
{
    public class Product
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
    }
}
