namespace api.validations;

public class UpdateBasketValidator : AbstractValidator<UpdateBasketRequestModel>
{
    public UpdateBasketValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().Must(x => x!.Length == 24).OverridePropertyName("Sepet ID");
        RuleFor(x => x.ProductId).NotNull().NotEmpty().Must(x => x!.Length == 24).OverridePropertyName("Ürün ID");
        RuleFor(x => x.UnitPrice).GreaterThan(0).OverridePropertyName("Ürün Fiyatı");
        RuleFor(x => x.Quantity).GreaterThan(0).OverridePropertyName("Ürün Adeti");
    }
}
