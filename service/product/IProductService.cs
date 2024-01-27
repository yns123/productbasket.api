using service.dtos.product;

namespace service.product;

public interface IProductService
{
    Task<(List<GetProductResponseDto>? List, int Count)?> GetAsync(GetProductRequestDto request);
    Task<GetProductResponseDto> GetDetailAsync(string id);
    Task CreateAsync(CreateProductRequestDto request);
    Task UpdateAsync(UpdateProductRequestDto request);
    Task DeleteAsync(string id);
}
