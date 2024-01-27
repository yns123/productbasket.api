namespace service.dtos.basket;

public class GetBasketResponseDto
{
    public string? Id { get; set; }
    public string? ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; }
}
