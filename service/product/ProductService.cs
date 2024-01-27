using core.exceptions;
using core.helpers;
using data.entities;
using data.respositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using module.redis;
using service.dtos.product;

namespace service.product;

public class ProductService : IProductService
{
    readonly ILogger<ProductService> _logger;
    readonly IBaseRepository<Product> _productRepository;

    public ProductService(ILogger<ProductService> logger,
        IBaseRepository<Product> productRepository,
        IRedisService redisCacheService)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<(List<GetProductResponseDto>? List, int Count)?> GetAsync(GetProductRequestDto request)
    {
        var products = _productRepository.GetList(x =>
            (string.IsNullOrEmpty(request.Id) || x.Id == request.Id)
            && (string.IsNullOrEmpty(request.Name) || x.Name!.Contains(request.Name))
            && (request.Category == null || x.Category == request.Category)
            && (request.Price == null || x.Price == request.Price)
            && (request.IsActive == null || x.IsActive == request.IsActive),
        out int count,
        page: request.PageNumber,
        limit: request.PageLimit,
        withCount: true)
        .Select(x => new GetProductResponseDto
        {
            Id = x.Id,
            CreatedDate = x.CreatedDate,
            Name = x.Name,
            Category = x.Category,
            Description = x.Description,
            ImageUrl = x.ImageUrl,
            Price = x.Price,
            IsActive = x.IsActive
        }).ToList();

        return await Task.FromResult((products, count));
    }

    public async Task<GetProductResponseDto> GetDetailAsync(string id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new ServiceException("Product Not Found!");

        return new GetProductResponseDto
        {
            Id = product.Id,
            CreatedDate = product.CreatedDate,
            Name = product.Name,
            Category = product.Category,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            IsActive = product.IsActive
        };
    }

    public async Task CreateAsync(CreateProductRequestDto request)
    {
        var product = await _productRepository.GetAsync(x => x.Category == request.Category && string.Equals(x.Name, request.Name, StringComparison.OrdinalIgnoreCase));
        if (product is not null)
            throw new ServiceException("The Product Already Exist!");

        product = new Product
        {
            Category = request.Category,
            Name = request.Name,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Price = request.Price
        };

        await _productRepository.CreateAsync(product);

        //TODO: İlgili kişilere NOTIFICATION ata!
    }

    public async Task UpdateAsync(UpdateProductRequestDto request)
    {
        var product = await _productRepository.GetByIdAsync(request.Id!);
        if (product is null)
            throw new ServiceException("Product Not Found!");

        // var isExists = await _productRepository.GetAsync(x => x.Category == request.Category
        //     && x.Name!.ToLower() == request.Name!.ToLower()
        //     && x.Id != product.Id);

        // if (isExists is not null)
        //     throw new ServiceException("The Product Already Exist!");

        var oldProduct = product!.ToJson();

        product!.Category = request.Category;
        product.Name = request.Name;
        product.Description = request.Description;
        product.IsActive = request.IsActive;
        product.Price = request.Price;
        product.ImageUrl = request.ImageUrl;

        await _productRepository.UpdateAsync(product.Id!, product!);

        _logger.LogTrace($"{request.Id} ID li Product güncellendi! Eski değeri: {oldProduct}");
    }

    //TODO: Giriş yapan kullanıcınını Silme yetkisi var mı kontrol et
    public async Task DeleteAsync(string id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new ServiceException("Product Not Found!");

        await _productRepository.DeleteAsync(product);

        _logger.LogWarning($"{product.Id} ID li Product silindi!");
    }
}
