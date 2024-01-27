namespace api.models.basket;

public class GetBasketRequestModel : BaseRequestModel
{
    public string? Id { get; set; }
    public string? ProductId { get; set; }
    public decimal? UnitPrice { get; set; }
    public int? Quantity { get; set; }
}
