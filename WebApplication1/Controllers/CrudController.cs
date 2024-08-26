using BookStoreApi.Models;
using BookStoreApi.Service;
using BookStoreApi.Services;
using BookStoreApi.Interface;
using BookStoreApi.Validators;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CrudController <T> : ControllerBase, IController<T> where T : BaseEntity
{
    private readonly IRepository<T> _repository;
    private readonly IValidator<T> _validator;

    public CrudController(IRepository<T> repository, IValidator<T> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    

    [HttpPost("GetAll")]
    public async Task<List<T>> Get() =>
        await _repository.GetAsync();

    [HttpGet("{entityId}")]
    public async Task<ActionResult<T>>? Get(string entityId)
    {
        var entity = await _repository.GetAsync(entityId);

        if (entity is null)
        {
            return NotFound("Entity not found");
            //throw new Exception("404");
        }

        return Ok(entity);
    }
    [HttpPost]
    public async Task<IActionResult> Post(T entity)
    {
        var context = new ValidationContext<T>(entity);
        context.RootContextData["RequestType"] = "POST";


        var validationResult = await _validator.ValidateAsync(context);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _repository.CreateAsync(entity);

        return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
    }

    [HttpPost("InsertMany")]
    public async Task<IActionResult> Post(T[] entities)
    {
        foreach (var entity in entities)
        {
            var validationResult = await _validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
        }

        await _repository.CreateMultipleAsync(entities);

        return CreatedAtAction(nameof(Get), new { ids = entities.Select(e => e.Id) }, entities);
    }

    [HttpPut("{id:length(24)}")] 
    public async Task<IActionResult> Update(string id, T updatedEntity)
    {
        var context = new ValidationContext<T>(updatedEntity);
        context.RootContextData["RequestType"] = "PUT";

        var entity = await _repository.GetAsync(id);

        if (entity is null)
        {
            return NotFound();
        }

        updatedEntity.Id = entity.Id;

        var validationResult = await _validator.ValidateAsync(context);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _repository.UpdateAsync(id, updatedEntity);

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var entity = await _repository.GetAsync(id);

        if (entity is null)
        {
            return NotFound();
        }

        await _repository.RemoveAsync(id);

        return NoContent();
    }

    
}
