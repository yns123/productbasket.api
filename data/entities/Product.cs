using core.enums;

namespace data.entities;

public class Product : BaseEntity
{
    public Category Category { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
}