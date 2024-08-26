using BookStoreApi.Models;
using BookStoreApi.Service;
using BookStoreApi.Services;
using BookStoreApi.Interface;
using BookStoreApi.Validators;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : CrudController<Book>
{
    private readonly IBooksService _booksService;

    //public BooksController(BooksService booksService) =>
    //    _booksService =s booksService;

    public BooksController(IBooksService booksService, IValidator<Book> validator, IRepository<Book> repo) : base(repo, validator)
    {
        _booksService = booksService;
    }

    [HttpGet("Search")]
    public async Task<List<Book>> Search([FromQuery] string? title, string? author, string? category) =>
        await _booksService.SearchBooks(title, author, category);

    [HttpGet("ListByAuthor")]
    public List<String> ListByAuthor() =>
        _booksService.ListBookByAuthor();
    
    [HttpGet("ListByCategory")]
    public List<String> ListByCategory() =>
        _booksService.ListBookByCategory();
}