namespace service.dtos.basket;

public class CreateBasketRequestDto
{
    public string? ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
