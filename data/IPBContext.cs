using data.entities;
using MongoDB.Driver;

namespace data;

public interface IPBContext
{
    IMongoCollection<Product> Products { get; set; }
    IMongoCollection<Basket> Baskets { get; set; }
}
