using BookStoreApi.Controllers;
using BookStoreApi.Interface;
using BookStoreApi.Models;
using BookStoreApi.Validators;
using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Runtime;
using FluentValidation.TestHelper;
using System.ComponentModel.DataAnnotations;



namespace BookStoreApi.Tests.ControllerTest;

public class BookControllerTests
{
    private readonly IRepository<Book> _repository;
    private readonly IBooksService _booksService;
    //private readonly MongoRepository<Book> _mongoRepository;
    private readonly BooksController _controller;
    private readonly IValidator<Book> _validator;

    //private readonly IOptions<BookStoreDatabaseSettings> _fakeOptions;
    //private readonly IMongoClient _mongoClient;
    //private readonly IMongoDatabase _mongoDb;
    //private readonly IMongoCollection<Book> _bookCollection;


    public BookControllerTests()
    {

        _repository = A.Fake<IRepository<Book>>();
        //_mongoRepository = A.Fake<MongoRepository<Book>>();
        _validator = A.Fake<IValidator<Book>>();
        _booksService = A.Fake<IBooksService>();
        _controller = new BooksController(_booksService, _validator, _repository);

        //_fakeOptions = A.Fake<IOptions<BookStoreDatabaseSettings>>();
        //_mongoClient = A.Fake<IMongoClient>();
        //_mongoDb = A.Fake<IMongoDatabase>();
        //_bookCollection = A.Fake<IMongoCollection<Book>>();

    }

    [Fact]
    public async void BooksController_GetAll_ReturnOK()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Author = "Author1", Category = "Category1" },
            new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2", Author = "Author2", Category = "Category2" }
        };
        A.CallTo(() => _repository.GetAsync()).Returns(books);
        //var controller = new BooksController(_booksService, _validator, _repository);

        //Act
        var result = await _controller.Get();

        //Assert
        result.Should().BeEquivalentTo(books);
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeOfType<List<Book>>();
    }

    [Fact]
    public async void BooksController_GetAll_ReturnNotFound()
    {
        //Arrange
        var books = new List<Book>();
        A.CallTo(() => _repository.GetAsync()).Returns(books);
        //var controller = new BooksController(_booksService, _validator, _repository);

        //Act
        var result = await _controller.Get();

        //Assert
        result.Should().BeEquivalentTo(books);
        result.Should().NotBeNull();
        //result.Should().HaveCount(2);
        result.Should().BeOfType<List<Book>>();
    }

    [Fact]
    public async void BooksController_GetById_ReturnOK()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1",Price = 9, Author = "Author1", Category = "Category1" },
            new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2",Price = 9, Author = "Author2", Category = "Category2" }
        };
        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(books[0]);

        //Act
        var serializeResult = await _controller.Get("E606B5E376CF989C7680588A");
        var temp = (OkObjectResult)serializeResult.Result;
        var result = temp.Value;

        //Assert
        result.Should().BeEquivalentTo(books[0]);
        result.Should().BeSameAs(books[0]);
        result.Should().NotBeNull();
        result.Should().BeOfType<Book>();
    }

    [Fact]
    public async void BooksController_GetById_ReturnNotFound()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Author = "Author1", Category = "Category1" },
            new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2", Author = "Author2", Category = "Category2" }
        };
        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(books[0]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D1")).Returns(books[1]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D2")).Returns((Book)null);

        //Act
        var result = await _controller.Get("BAB491765705B050F5E695D2");


        //Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();

        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be("Entity not found");

    }

    [Fact]
    public async void BooksController_Update_ReturnNotFound()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Author = "Author1", Category = "Category1" },
            new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2", Author = "Author2", Category = "Category2" }
        };
        var editedBook = new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book3", Author = "Author3", Category = "Category3" };

        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(books[0]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D1")).Returns(books[1]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D2")).Returns((Book)null);

        //Act
        var result = await _controller.Update("BAB491765705B050F5E695D2", editedBook);
        result = (NotFoundResult)result;


        //Assert
        result.Should().BeOfType<NotFoundResult>();
    }
    
    [Fact]
    public async void BooksController_Update_ReturnBadRequest()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Author = "Author1", Category = "Category1" },
            new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2", Author = "Author2", Category = "Category2" }
        };
        var editedBook = new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book3", Price = -1, Author = "Author3", Category = "Category3" };

        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(books[0]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D1")).Returns(books[1]);

        //Act
        var result = await _controller.Update("BAB491765705B050F5E695D1", editedBook);

        result = (BadRequestObjectResult)result;


        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task BookController_Update_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var id = "BAB491765705B050F5E695D1";
        var existingEntity = new Book(); // Create an instance of the entity
        var updatedEntity = new Book(); // Create an instance of the entity

        A.CallTo(() => _repository.GetAsync(id)).Returns(existingEntity);
        A.CallTo(() => _validator.ValidateAsync(A<ValidationContext<Book>>.Ignored, default))
            .Returns(new FluentValidation.Results.ValidationResult());

        // Act
        var result = await _controller.Update(id, updatedEntity);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async void BooksController_Delete_ReturnNotFound()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Author = "Author1", Category = "Category1" },
            new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2", Author = "Author2", Category = "Category2" }
        };

        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(books[0]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D1")).Returns(books[1]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D2")).Returns((Book)null);

        //Act
        var result = await _controller.Delete("BAB491765705B050F5E695D2");
        result = (NotFoundResult)result;


        //Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async void BooksController_Delete_ReturnNoContent_DeleteSuccesful()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Author = "Author1", Category = "Category1" },
            new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2", Author = "Author2", Category = "Category2" }
        };

        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(books[0]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D1")).Returns(books[1]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D2")).Returns((Book)null);

        //Act
        var result = await _controller.Delete("BAB491765705B050F5E695D1");


        //Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async void BooksController_Create_ReturnBadRequest()
    {
        //Arrange
        var book = new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book3", Price = 10, Author = "Author3", Category = "Category3" };

        A.CallTo(() => _validator.ValidateAsync(book, default)).Returns(_validator.TestValidate(book));

        //Act
        var result = await _controller.Post(book);

        result = (BadRequestObjectResult)result;
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async void BooksController_Create_ReturnCreated()
    {
        //Arrange
        var book = new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book3", Price = 51, Author = "Author3", Category = "Category3" };

        A.CallTo(() => _validator.ValidateAsync(A<ValidationContext<Book>>.Ignored, default))
                    .Returns(new FluentValidation.Results.ValidationResult());
        A.CallTo(() => _repository.CreateAsync(book)).Returns(Task.CompletedTask);

        //Act
        var result = await _controller.Post(book);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }
    
    [Fact]
    public async void BooksController_CreateMany_ReturnCreated()
    {
        //Arrange
        var books = new Book[]
        {
            new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Price = 5, Author = "Author1", Category = "Category1" },
            new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2", Price = 6, Author = "Author2", Category = "Category2" }
        }; 

        A.CallTo(() => _validator.ValidateAsync(A<Book>.Ignored, default))
                    .Returns(new FluentValidation.Results.ValidationResult());

        A.CallTo(() => _repository.CreateMultipleAsync(books)).Returns(Task.CompletedTask);

        //Act
        var result = await _controller.Post(books);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task BooksController_Search_ReturnsListOfBooks_WhenCalledWithValidParameters()
    {
        // Arrange
        var books = new List<Book>
        {
           new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Price = 5, Author = "Author1", Category = "Category1" },
           new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2", Price = 6, Author = "Author2", Category = "Category2" }
        };

        A.CallTo(() => _booksService.SearchBooks("Book1", "Author1", "Category1"))
            .Returns(books.GetRange(0, 1));

        // Act
        var result = await _controller.Search("Book1", "Author1", "Category1");

        // Assert
        Assert.Equal(books.GetRange(0, 1), result);
        A.CallTo(() => _booksService.SearchBooks("Book1", "Author1", "Category1"))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task BooksController_Search_ReturnsListOfBooks_WhenCalledWithLessParameters()
    {
        // Arrange
        var books = new List<Book>
        {
           new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Price = 5, Author = "Author1", Category = "Category1" },
           new Book { Id = "E606B5E376CF989C7680566B", BookName = "Book1", Price = 5, Author = "Author1", Category = "Category1" },
           new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2", Price = 6, Author = "Author2", Category = "Category2" }
        };

        A.CallTo(() => _booksService.SearchBooks("Book1", "Author1", null))
            .Returns(books.GetRange(0, 2));

        // Act
        var result = await _controller.Search("Book1", "Author1", null);

        // Assert
        Assert.Equal(books.GetRange(0, 2), result);
        A.CallTo(() => _booksService.SearchBooks("Book1", "Author1", null))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task BooksController_Search_ReturnsListOfBooks_WhenCalledWithNoParameters()
    {
        // Arrange
        var books = new List<Book>
        {
           new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Price = 5, Author = "Author1", Category = "Category1" },
           new Book { Id = "E606B5E376CF989C7680566B", BookName = "Book1", Price = 5, Author = "Author1", Category = "Category1" },
           new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book2", Price = 6, Author = "Author2", Category = "Category2" }
        };

        A.CallTo(() => _booksService.SearchBooks(null, null, null))
            .Returns(books.GetRange(0, 3));

        // Act
        var result = await _controller.Search(null, null, null);

        // Assert
        Assert.Equal(books, result);
        A.CallTo(() => _booksService.SearchBooks(null, null, null))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void BooksController_ListByCategory_ReturnsListOfCategories()
    {
        // Arrange
        var books = new List<Book>
        {
           new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Price = 5, Author = "Author1", Category = "Category1" },
           new Book { Id = "E606B5E376CF989C7680566B", BookName = "Book2", Price = 5, Author = "Author1", Category = "Category1" },
           new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book3", Price = 6, Author = "Author2", Category = "Category2" }
        };

        var expectedResult = new List<string>
        {
            "Category1: [Book1, Book2]",
            "Category2: [Book3]"
        };

        A.CallTo(() => _booksService.ListBookByCategory())
            .Returns(expectedResult);

        // Act
        var result = _controller.ListByCategory();

        // Assert
        Assert.Equal(expectedResult, result);
        A.CallTo(() => _booksService.ListBookByCategory())
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public void BooksController_ListByAuthor_ReturnsListOfAuthor()
    {
        // Arrange
        var books = new List<Book>
        {
           new Book { Id = "E606B5E376CF989C7680588A", BookName = "Book1", Price = 5, Author = "Author1", Category = "Category1" },
           new Book { Id = "E606B5E376CF989C7680566B", BookName = "Book2", Price = 5, Author = "Author1", Category = "Category1" },
           new Book { Id = "BAB491765705B050F5E695D1", BookName = "Book3", Price = 6, Author = "Author2", Category = "Category2" }
        };

        var expectedResult = new List<string>
        {
            "Author1: [Book1, Book2]",
            "Author2: [Book3]"
        };

        A.CallTo(() => _booksService.ListBookByAuthor())
            .Returns(expectedResult);

        // Act
        var result = _controller.ListByAuthor();

        // Assert
        Assert.Equal(expectedResult, result);
        A.CallTo(() => _booksService.ListBookByAuthor())
            .MustHaveHappenedOnceExactly();
    }

}
