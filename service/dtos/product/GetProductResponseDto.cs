using core.enums;

namespace service.dtos.product;

public class GetProductResponseDto
{
    public string? Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Name { get; set; }
    public Category Category { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
