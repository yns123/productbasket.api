namespace api.validations;

public class CreateBasketValidator : AbstractValidator<CreateBasketRequestModel>
{
    public CreateBasketValidator()
    {
        RuleFor(x => x.ProductId).NotNull().NotEmpty().Must(x => x!.Length == 24).OverridePropertyName("Ürün ID");
        RuleFor(x => x.UnitPrice).GreaterThan(0).OverridePropertyName("Ürün Fiyatı");
        RuleFor(x => x.Quantity).GreaterThan(0).OverridePropertyName("Ürün Adeti");
    }
}
