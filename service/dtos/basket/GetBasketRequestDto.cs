namespace service.dtos.basket;

public class GetBasketRequestDto : BaseRequestDto
{
    public string? Id { get; set; }
    public string? ProductId { get; set; }
    public decimal? UnitPrice { get; set; }
    public int? Quantity { get; set; }
}
