using core.enums;

namespace api.models.product;

public class GetProductRequestModel : BaseRequestModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public Category? Category { get; set; }
    public decimal? Price { get; set; }
    public bool? IsActive { get; set; }
}
