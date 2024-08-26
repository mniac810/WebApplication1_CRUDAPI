namespace BookStoreApi.Models;

public class BookStoreDatabaseSettings
{
    public virtual string ConnectionString { get; set; } = null!;
    public virtual string DatabaseName { get; set; } = null!;
    public string[] BooksCollectionName { get; set; } = null!;
}

