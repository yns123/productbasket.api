namespace api.models.product;

public class UpdateProductRequestModel : CreateProductRequestModel
{
    public string? Id { get; set; }
    public bool IsActive { get; set; }
}
