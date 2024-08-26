using BookStoreApi.Models;

namespace BookStoreApi.Interface;

public interface IBooksService : IRepository<Book>
{
    Task<List<Book>> SearchBooks(string title, string author, string category);
    List<String> ListBookByAuthor();
    List<String> ListBookByCategory();
}
