namespace api.models.basket;

public class CreateBasketRequestModel
{
    public string? ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
