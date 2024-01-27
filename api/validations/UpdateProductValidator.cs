namespace api.validations;

public class UpdateProductValidator : AbstractValidator<UpdateProductRequestModel>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().Must(x => x!.Length == 24).OverridePropertyName("Ürün ID");
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(50).OverridePropertyName("Ürün Adı");
        RuleFor(x => x.Description).NotNull().NotEmpty().MaximumLength(500).OverridePropertyName("Ürün Açıklaması");
        RuleFor(x => x.ImageUrl).NotNull().NotEmpty().MaximumLength(300).OverridePropertyName("Ürün Resmi");
        RuleFor(x => x.Category).NotNull().NotEmpty().IsInEnum().OverridePropertyName("Ürün Kategorisi");
        RuleFor(x => x.Price).GreaterThan(0).OverridePropertyName("Ürün Fiyatı");
    }
}
