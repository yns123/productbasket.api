using core;
using core.exceptions;
using core.helpers;
using data.entities;
using data.respositories;
using Microsoft.Extensions.Logging;
using module.redis;
using service.dtos.basket;

namespace service.basket;

public class BasketService : IBasketService
{
    readonly ILogger<BasketService> _logger;
    readonly IBaseRepository<Product> _productRepository;
    readonly IBaseRepository<Basket> _basketRepository;
    readonly IRedisService _redisCacheService;

    public BasketService(ILogger<BasketService> logger,
        IBaseRepository<Product> productRepository,
        IBaseRepository<Basket> basketRepository,
        IRedisService redisCacheService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _basketRepository = basketRepository;
        _redisCacheService = redisCacheService;
    }

    public async Task<List<GetBasketResponseDto>> GetAsync(GetBasketRequestDto request)
    {
        //TODO: PAGINATION uygula (eğer ihtiyaç var ise)
        var result = _redisCacheService.GetList<Basket>(CacheKeys.BASKET_LIST);

        if (result is null || !result.Any())
            return null!;

        var response = result.Select(x => new GetBasketResponseDto
        {
            Id = x.Id,
            CreatedDate = x.CreatedDate,
            ProductId = x.ProductId,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice
        }).ToList();

        return await Task.FromResult(response);
    }

    public async Task<GetBasketResponseDto> GetDetailAsync(string id)
    {
        var basketList = _redisCacheService.GetList<Basket>(CacheKeys.BASKET_LIST);

        var result = basketList?.FirstOrDefault(x => x.Id == id);
        if (result is null)
            throw new ServiceException("Sepet bulunamadı!");

        return await Task.FromResult(new GetBasketResponseDto
        {
            Id = result.Id,
            CreatedDate = result.CreatedDate,
            ProductId = result.ProductId,
            Quantity = result.Quantity,
            UnitPrice = result.UnitPrice
        });
    }

    public async Task CreateAsync(CreateBasketRequestDto request)
    {
        //* Ürünü kontrol et
        var product = await _productRepository.GetByIdAsync(request.ProductId!);
        if (product is null)
            throw new ServiceException("Product Not Found!");

        //* DB'ye de ekle
        var newBasket = new Basket
        {
            CreatedDate = DateTime.Now,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice
        };

        await _basketRepository.CreateAsync(newBasket);

        //* Redise ekle
        _redisCacheService.AddToList(CacheKeys.BASKET_LIST, new List<Basket> { newBasket });

        //TODO: İlgili kişilere NOTIFICATION ata!
    }

    public async Task UpdateAsync(UpdateBasketRequestDto request)
    {
        //* Önce DB'de güncelle
        await UpdateDbAsync(request);

        var basketList = _redisCacheService.GetList<Basket>(CacheKeys.BASKET_LIST);

        var result = basketList?.FirstOrDefault(x => x.Id == request.Id);
        if (result is null)
            throw new ServiceException("Sepet bulunamadı!");

        result!.ProductId = request.ProductId;
        result.Quantity = request.Quantity;
        result.UnitPrice = request.UnitPrice;

        _redisCacheService.AddToList(CacheKeys.BASKET_LIST, new List<Basket> { result });
    }

    public async Task DeleteAsync(string id)
    {
        //* Önce DB'den dil
        await DeleteFromDbAsync(id);

        bool isExist = _redisCacheService.KeyExists(id);
        if (!isExist)
            throw new ServiceException("Ürün bulunamadı!");

        _redisCacheService.Delete($"{CacheKeys.BASKET_ID}_{id}");

        _logger.LogWarning($"{id} ID li Basket silindi!");
    }

    #region Helpers

    async Task UpdateDbAsync(UpdateBasketRequestDto request)
    {
        var basket = await _basketRepository.GetByIdAsync(request.Id!);
        if (basket is null)
            throw new ServiceException("Basket Not Found!");

        var oldProduct = basket!.ToJson();

        basket!.ProductId = request.ProductId;
        basket.Quantity = request.Quantity;
        basket.UnitPrice = request.UnitPrice;

        await _basketRepository.UpdateAsync(basket.Id!, basket!);

        _logger.LogTrace($"{request.Id} ID li Product güncellendi! Eski değeri: {oldProduct}");
    }

    async Task DeleteFromDbAsync(string id)
    {
        var product = await _basketRepository.GetByIdAsync(id);
        if (product is null)
            throw new ServiceException("Basket Not Found!");

        await _basketRepository.DeleteAsync(product);
    }

    #endregion
}
