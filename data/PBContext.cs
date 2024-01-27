using MongoDB.Driver;
using data.entities;
using Microsoft.Extensions.Options;
using core.settings;

namespace data;

public class PBContext : MongoClient, IPBContext
{
    readonly IOptions<MongoSettings> _settings;

    public PBContext(IOptions<MongoSettings> settings) : base(settings.Value.ConnectionString)
    {
        _settings = settings;
    }

    public IMongoCollection<Product> Products { get; set; }
    public IMongoCollection<Basket> Baskets { get; set; }
}
