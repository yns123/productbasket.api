using service.dtos.basket;

namespace service.basket;

public interface IBasketService
{
    Task<List<GetBasketResponseDto>> GetAsync(GetBasketRequestDto request);
    Task<GetBasketResponseDto> GetDetailAsync(string id);
    Task CreateAsync(CreateBasketRequestDto request);
    Task UpdateAsync(UpdateBasketRequestDto request);
    Task DeleteAsync(string id);
}
