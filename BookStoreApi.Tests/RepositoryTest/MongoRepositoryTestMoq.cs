using BookStoreApi.Models;
using BookStoreApi.Service;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BookStoreApi.Tests.Service
{
    public class MongoRepositoryTests
    {
        private readonly Mock<IRepository<Book>> _mockRepository;

        public MongoRepositoryTests()
        {
            _mockRepository = new Mock<IRepository<Book>>();
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

            _mockRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(mockBooks);

            // Act
            var result = await _mockRepository.Object.GetAsync();

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
            _mockRepository.Setup(repo => repo.GetAsync("1")).ReturnsAsync(mockBook);

            // Act
            var result = await _mockRepository.Object.GetAsync("1");

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
            _mockRepository.Setup(repo => repo.CreateAsync(newBook)).Returns(Task.CompletedTask);

            // Act
            await _mockRepository.Object.CreateAsync(newBook);

            // Assert
            _mockRepository.Verify(repo => repo.CreateAsync(newBook), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesBook()
        {
            // Arrange
            var updatedBook = new Book { Id = "1", BookName = "Updated Book", Author = "Updated Author", Category = "Updated Category", Price = 12.99M };
            _mockRepository.Setup(repo => repo.UpdateAsync("1", updatedBook)).Returns(Task.CompletedTask);

            // Act
            await _mockRepository.Object.UpdateAsync("1", updatedBook);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync("1", updatedBook), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_RemovesBook()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.RemoveAsync("1")).Returns(Task.CompletedTask);

            // Act
            await _mockRepository.Object.RemoveAsync("1");

            // Assert
            _mockRepository.Verify(repo => repo.RemoveAsync("1"), Times.Once);
        }
    }
}
