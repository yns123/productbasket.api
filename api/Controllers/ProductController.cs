using System.Reflection;

namespace api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductController : BaseApiController
{
    //TODO: LOCALIZATION işlemlerini ayarla ve tüm hataları ona göre dön
    readonly ILogger<ProductController> _logger;
    readonly IProductService _productService;

    public ProductController(
        ILogger<ProductController> logger,
        IProductService productService)
        : base(logger)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetProductRequestModel request)
    {
        if (request.Id is not null && request.Id.Length != 24)
            throw new ApiValidationException("ID 24 karakter olmalı!");

        var result = await _productService.GetAsync(new GetProductRequestDto
        {
            Id = request.Id,
            Name = request.Name,
            Category = request.Category,
            Price = request.Price,
            IsActive = request.IsActive,
            PageLimit = request.PageLimit,
            PageNumber = request.PageNumber
        });

        return Ok(new BaseApiResult { Data = result?.List, Count = result?.Count ?? 0 });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Detail(string id)
    {
        if (id.Length != 24)
            throw new ApiValidationException("ID 24 karakter olmalı!");

        var result = await _productService.GetDetailAsync(id);

        return Ok(new BaseApiResult { Data = result });
    }

    ///<summary>
    /// Yeni ürünü kaydeder.
    /// Önce parametreleri kontrol eder ve ardından kaydı oluşturmak için servis katmanına yönlendirir. Sorunsuz oluşması durumunda geriye 200 döner
    ///</summary>
    [HttpPost]
    //TODO: RATELIMIT İŞLEMLERİNİ AKTİFLEŞTİR
    //TODO: KULLANICI ROLE İŞLEMLERİNİ KONTROL ET
    public async Task<IActionResult> Create([FromBody] CreateProductRequestModel request)
    {
        ValidationResult validationResult = await new CreateProductValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ApiValidationException(validationResult.Errors);

        //TODO: GİRİŞ YAPAN KULLANICI BİLGİLERİ ALINACAK VE KONTROL EDİLECEK

        await _productService.CreateAsync(new CreateProductRequestDto
        {
            Name = request.Name,
            Category = request.Category,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Price = request.Price
        });

        return Ok(new BaseApiResult());
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProductRequestModel request)
    {
        ValidationResult validationResult = await new UpdateProductValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ApiValidationException(validationResult.Errors);

        //TODO: GİRİŞ YAPAN KULLANICI BİLGİLERİ ALINACAK VE KONTROL EDİLECEK

        await _productService.UpdateAsync(new UpdateProductRequestDto
        {
            Id = request.Id,
            IsActive = request.IsActive,
            Name = request.Name,
            Category = request.Category,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Price = request.Price
        });

        return Ok(new BaseApiResult());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (id.Length != 24)
            throw new ApiValidationException("ID 24 karakter olmalı!");

        await _productService.DeleteAsync(id);

        return Ok(new BaseApiResult());
    }

    [HttpGet("dummyData")]
    public async Task<IActionResult> GetDummyData()
    {
        List<CreateProductRequestModel> products;

        using (StreamReader r = new StreamReader("../../products.json"))
        {
            string json = await r.ReadToEndAsync();
            products = json.FromJson<List<CreateProductRequestModel>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return Ok(new BaseApiResult { Data = products, Count = products.Count });
    }

    [HttpPost("dummyData")]
    public async Task<IActionResult> FillDummyData()
    {
        List<CreateProductRequestModel> products;

        using (StreamReader r = new StreamReader(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, CacheKeys.PRODUCTS)))
        {
            string json = await r.ReadToEndAsync();
            products = json.FromJson<List<CreateProductRequestModel>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        foreach (var product in products)
        {
            try
            {
                await Create(product);
            }
            catch (Exception)
            {
                continue;
            }
        }

        return Ok(new BaseApiResult());
    }
}
