namespace api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BasketController : BaseApiController
{
    //TODO: LOCALIZATION işlemlerini ayarla ve tüm hataları ona göre dön
    readonly ILogger<BasketController> _logger;
    readonly IBasketService _basketService;

    public BasketController(
        ILogger<BasketController> logger,
        IBasketService basketService)
        : base(logger)
    {
        _logger = logger;
        _basketService = basketService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetBasketRequestModel request)
    {
        if (request.Id is not null && request.Id.Length != 24)
            throw new ApiValidationException("ID 24 karakter olmalı!");

        var result = await _basketService.GetAsync(new GetBasketRequestDto
        {
            Id = request.Id,
            ProductId = request.ProductId,
            UnitPrice = request.UnitPrice,
            Quantity = request.Quantity,
            PageLimit = request.PageLimit,
            PageNumber = request.PageNumber
        });

        return Ok(new BaseApiResult { Data = result, Count = result?.Count ?? 0 });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Detail(string id)
    {
        if (id.Length != 24)
            throw new ApiValidationException("ID 24 karakter olmalı!");

        var result = await _basketService.GetDetailAsync(id);

        return Ok(new BaseApiResult { Data = result });
    }

    ///<summary>
    /// Yeni ürünü kaydeder.
    /// Önce parametreleri kontrol eder ve ardından kaydı oluşturmak için servis katmanına yönlendirir. Sorunsuz oluşması durumunda geriye 200 döner
    ///</summary>
    [HttpPost]
    //TODO: RATELIMIT İŞLEMLERİNİ AKTİFLEŞTİR
    //TODO: KULLANICI ROLE İŞLEMLERİNİ KONTROL ET
    public async Task<IActionResult> Create([FromBody] CreateBasketRequestModel request)
    {
        ValidationResult validationResult = await new CreateBasketValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ApiValidationException(validationResult.Errors);

        //TODO: GİRİŞ YAPAN KULLANICI BİLGİLERİ ALINACAK VE KONTROL EDİLECEK

        await _basketService.CreateAsync(new CreateBasketRequestDto
        {
            ProductId = request.ProductId,
            UnitPrice = request.UnitPrice,
            Quantity = request.Quantity,
        });

        return Ok(new BaseApiResult());
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBasketRequestModel request)
    {
        ValidationResult validationResult = await new UpdateBasketValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ApiValidationException(validationResult.Errors);

        //TODO: GİRİŞ YAPAN KULLANICI BİLGİLERİ ALINACAK VE KONTROL EDİLECEK

        await _basketService.UpdateAsync(new UpdateBasketRequestDto
        {
            Id = request.Id,
            ProductId = request.ProductId,
            UnitPrice = request.UnitPrice,
            Quantity = request.Quantity,
        });

        return Ok(new BaseApiResult());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (id.Length != 24)
            throw new ApiValidationException("ID 24 karakter olmalı!");

        await _basketService.DeleteAsync(id);

        return Ok(new BaseApiResult());
    }
}
