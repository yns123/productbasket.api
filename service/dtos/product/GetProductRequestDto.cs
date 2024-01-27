using core.enums;

namespace service.dtos.product;

public class GetProductRequestDto : BaseRequestDto
{
    string? _name;
    decimal? _price;

    public string? Id { get; set; }
    public Category? Category { get; set; }
    public bool? IsActive { get; set; }
    public string? Name { get => _name; set => _name = value?.ToLower(); }
    public decimal? Price { get => _price; set => _price = (value is not null && value < 0) ? 0 : value; }
}
