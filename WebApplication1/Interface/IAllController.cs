using BookStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Interface
{
    public interface IAllController
    {
        Task<CombinedDTOModels> Get();
    }
}
