using core.enums;

namespace api.models.product;

public class CreateProductRequestModel
{
    public string? Name { get; set; }
    public Category Category { get; set; }
    public string? StrCategory { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
}
