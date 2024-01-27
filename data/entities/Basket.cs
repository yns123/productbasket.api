namespace data.entities;

public class Basket : BaseEntity
{
    public string? ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}