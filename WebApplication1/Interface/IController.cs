using BookStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Interface
{
    public interface IController<T> where T : BaseEntity
    {
        Task<IActionResult> Post(T entity);
        Task<IActionResult> Update(string entityd, T entity);
        Task<IActionResult> Delete(string entityId);
        Task<List<T>> Get();
        Task<ActionResult<T>>? Get(string entityId);
        Task<IActionResult> Post(T[] entities);
    }
}
