using MongoDB.Driver;
using BookStoreApi.Models;
using BookStoreApi.Service;
using BookStoreApi.Services;
using BookStoreApi.Interface;
using Microsoft.Extensions.Options;

namespace BookStoreApi.Service;

public class AllService : IAllService
{
    private readonly IBooksService _booksService;
    private readonly IMagazineService _magazineService;

    public AllService(IBooksService booksService, IMagazineService magazineService)
    {
        _booksService = booksService ?? throw new ArgumentNullException(nameof(booksService));
        _magazineService = magazineService ?? throw new ArgumentNullException(nameof(magazineService));
    }

    public async Task<CombinedDTOModels> GetAll()
    {
        var books = await _booksService.GetAsync();
        var magazines = await _magazineService.GetAsync();

        var result = new CombinedDTOModels
        {
            Books = books,
            Magazines = magazines
        };
        return result;
    }
}



