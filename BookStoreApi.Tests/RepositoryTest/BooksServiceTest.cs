using BookStoreApi.Interface;
using BookStoreApi.Models;
using BookStoreApi.Services;
using FakeItEasy;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookStoreApi.Tests.Service
{
    public class BooksServiceTests
    {
        private readonly IBooksService _booksService;
        private readonly IMongoCollection<Book> _fakeBookCollection;
        private readonly IOptions<BookStoreDatabaseSettings> _fakeDatabaseSettings;

        public BooksServiceTests()
        {
            // Arrange
            _fakeBookCollection = A.Fake<IMongoCollection<Book>>();
            _fakeDatabaseSettings = A.Fake<IOptions<BookStoreDatabaseSettings>>();
            var _mongoDatabase = A.Fake<IMongoDatabase>();
            _booksService = A.Fake<IBooksService>();
        }

        [Fact]
        public void ListBookByAuthor_ReturnsBooksGroupedByAuthor()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookName = "Book 1", Author = "Author 1", Category = "Category 1" },
                new Book { BookName = "Book 2", Author = "Author 1", Category = "Category 1" },
                new Book { BookName = "Book 3", Author = "Author 2", Category = "Category 2" }
            }.AsQueryable();

            var queryResult = books.GroupBy(
                x => x.Author,
                x => x.BookName,
                (AuthorName, Names) => new { Author = AuthorName, BookName = Names.ToList() });

            //A.CallTo(() => _fakeBookCollection.AsQueryable().GroupBy(
            //    x => x.Author,
            //    x => x.BookName,
            //    (AuthorName, Names) => new { Author = AuthorName, BookName = Names.ToList() })).Returns(queryResult);

            A.CallTo(() => _fakeBookCollection.AsQueryable()).Returns(books);

            // Act
            var result = _booksService.ListBookByAuthor();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("Author 1: [Book 1, Book 2]", result);
            Assert.Contains("Author 2: [Book 3]", result);
        }

        [Fact]
        public void ListBookByCategory_ReturnsBooksGroupedByCategory()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookName = "Book 1", Author = "Author 1", Category = "Category 1" },
                new Book { BookName = "Book 2", Author = "Author 1", Category = "Category 1" },
                new Book { BookName = "Book 3", Author = "Author 2", Category = "Category 2" }
            }.AsQueryable();

            A.CallTo(() => _fakeBookCollection.AsQueryable()).Returns(books);

            // Act
            var result = _booksService.ListBookByCategory();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("Category 1: [Book 1, Book 2]", result);
            Assert.Contains("Category 2: [Book 3]", result);
        }

        [Fact]
        public async Task SearchBooks_ReturnsFilteredBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = "1", BookName = "Book 1", Author = "Author 1", Category = "Category 1" },
                new Book { Id = "2", BookName = "Book 2", Author = "Author 2", Category = "Category 2" }
            };

            var fakeAsyncCursor = A.Fake<IAsyncCursor<Book>>();
            A.CallTo(() => fakeAsyncCursor.Current).Returns(books);
            A.CallTo(() => fakeAsyncCursor.MoveNext(default)).Returns(true);
            A.CallTo(() => fakeAsyncCursor.MoveNextAsync(default)).Returns(Task.FromResult(true));
            A.CallTo(() => _fakeBookCollection.FindAsync(A<FilterDefinition<Book>>._, default, default)).Returns(Task.FromResult(fakeAsyncCursor));

            // Act
            var result = await _booksService.SearchBooks("Book 1", null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal("1", result[0].Id);
        }

        [Fact]
        public async Task SearchBooks_ReturnsAllBooksWhenNoFilterIsApplied()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = "1", BookName = "Book 1", Author = "Author 1", Category = "Category 1" },
                new Book { Id = "2", BookName = "Book 2", Author = "Author 2", Category = "Category 2" }
            };

            A.CallTo(() => _booksService.GetAsync()).Returns(Task.FromResult(books));

            // Act
            var result = await _booksService.SearchBooks(null, null, null);

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
