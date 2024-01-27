namespace api.validations;

public class CreateProductValidator : AbstractValidator<CreateProductRequestModel>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(50).OverridePropertyName("Ürün Adı");
        RuleFor(x => x.Description).NotNull().NotEmpty().MaximumLength(500).OverridePropertyName("Ürün Açıklaması");
        RuleFor(x => x.ImageUrl).NotNull().NotEmpty().MaximumLength(300).OverridePropertyName("Ürün Resmi");
        RuleFor(x => x.Category).NotNull().NotEmpty().IsInEnum().OverridePropertyName("Ürün Kategorisi");
        RuleFor(x => x.Price).GreaterThan(0).OverridePropertyName("Ürün Fiyatı");
    }
}
