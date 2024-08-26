using BookStoreApi.Interface;
using BookStoreApi.Models;
using BookStoreApi.Service;
using BookStoreApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AllController : IAllController
{
  
    private readonly IAllService _allService;
    private readonly IValidator<CombinedDTOModels> _validator;

    //public AllController(BooksService booksService) =>
    //    _booksService = booksService;

    public AllController(IAllService allService, IValidator<CombinedDTOModels> validator)
    {
        _allService = allService;
        _validator = validator;
    }

    [HttpGet]

    public async Task<CombinedDTOModels> Get() =>
        await _allService.GetAll();

}