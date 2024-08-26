using BookStoreApi.Models;

namespace BookStoreApi.Interface;

public interface IAllService
{
    public Task<CombinedDTOModels> GetAll();
}
