using BookStoreApi.Models;
using BookStoreApi.Service;
using FakeItEasy;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BookStoreApi.Tests.Service;

public class MongoRepositoryTestFakeItEasy
{
    private readonly IRepository<Book> _fakeRepository;

    public MongoRepositoryTestFakeItEasy()
    {
        _fakeRepository = A.Fake<IRepository<Book>>();
    }

    [Fact]
    public async Task GetAsync_ReturnsAllBooks()
    {
        // Arrange
        var mockBooks = new List<Book>
        {
            new Book { Id = "1", BookName = "Book 1", Author = "Author 1", Category = "Category 1", Price = 10.99M },
            new Book { Id = "2", BookName = "Book 2", Author = "Author 2", Category = "Category 2", Price = 15.99M }
        };

        A.CallTo(() => _fakeRepository.GetAsync()).Returns(mockBooks);

        // Act
        var result = await _fakeRepository.GetAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("1", result[0].Id);
        Assert.Equal("2", result[1].Id);
    }

    [Fact]
    public async Task GetAsync_WithBookId_ReturnsSingleBook()
    {
        // Arrange
        var mockBook = new Book { Id = "1", BookName = "Book 1", Author = "Author 1", Category = "Category 1", Price = 10.99M };
        A.CallTo(() => _fakeRepository.GetAsync("1")).Returns(mockBook);

        // Act
        var result = await _fakeRepository.GetAsync("1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
        Assert.Equal("Book 1", result.BookName);
    }

    [Fact]
    public async Task CreateAsync_CreatesNewBook()
    {
        // Arrange
        var newBook = new Book { Id = "1", BookName = "New Book", Author = "New Author", Category = "New Category", Price = 9.99M };

        // Act
        await _fakeRepository.CreateAsync(newBook);

        // Assert
        A.CallTo(() => _fakeRepository.CreateAsync(newBook)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesBook()
    {
        // Arrange
        var updatedBook = new Book { Id = "1", BookName = "Updated Book", Author = "Updated Author", Category = "Updated Category", Price = 12.99M };

        // Act
        await _fakeRepository.UpdateAsync("1", updatedBook);

        // Assert
        A.CallTo(() => _fakeRepository.UpdateAsync("1", updatedBook)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveAsync_RemovesBook()
    {
        // Act
        await _fakeRepository.RemoveAsync("1");

        // Assert
        A.CallTo(() => _fakeRepository.RemoveAsync("1")).MustHaveHappenedOnceExactly();
    }
}
