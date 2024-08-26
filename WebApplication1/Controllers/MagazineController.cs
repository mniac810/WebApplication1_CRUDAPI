using BookStoreApi.Interface;
using BookStoreApi.Models;
using BookStoreApi.Service;
using BookStoreApi.Services;
using BookStoreApi.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MagazineController : CrudController<Magazine>
{
    private readonly IMagazineService _magazineService;

    //public BooksController(BooksService booksService) =>
    //    _booksService = booksService;

    public MagazineController(IMagazineService magazineService, IRepository<Magazine> repo, IValidator<Magazine> validator) : base(repo, validator)
    {
        _magazineService = magazineService;
    }



    //[HttpGet]
    //public async Task<List<Magazine>> Get() =>
    //    await _magazineService.GetAsync();

    //[HttpGet("{id:length(24)}")]
    //public async Task<ActionResult<Magazine>> Get(string id)
    //{
    //    var Magazine = await _magazineService.GetAsync(id);

    //    if (Magazine is null)
    //    {
    //        return NotFound();
    //    }

    //    return Magazine;
    //}

    //[HttpPost]
    //public async Task<IActionResult> Post(Magazine newMagazine)
    //{
    //    await _magazineService.CreateAsync(newMagazine);

    //    return CreatedAtAction(nameof(Get), new { id = newMagazine.Id }, newMagazine);
    //}

    //[HttpPut("{id:length(24)}")]
    //public async Task<IActionResult> Update(string id, Magazine updatedMagazine)
    //{
    //    var Magazine = await _magazineService.GetAsync(id);

    //    if (Magazine is null)
    //    {
    //        return NotFound();
    //    }

    //    updatedMagazine.Id = Magazine.Id;

    //    await _magazineService.UpdateAsync(id, updatedMagazine);

    //    return NoContent();
    //}

    //[HttpDelete("{id:length(24)}")]
    //public async Task<IActionResult> Delete(string id)
    //{
    //    var Magazine = await _magazineService.GetAsync(id);

    //    if (Magazine is null)
    //    {
    //        return NotFound();
    //    }

    //    await _magazineService.RemoveAsync(id);

    //    return NoContent();
    //}
}