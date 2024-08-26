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
using BookStoreApi.Services;

namespace BookStoreApi.Tests.ControllerTest;

public class MagazineControllerTest
{
    private readonly IRepository<Magazine> _repository;
    private readonly IMagazineService _magazineService;
    private readonly MagazineController _controller;
    private readonly IValidator<Magazine> _validator;

    public MagazineControllerTest()
    {

        _repository = A.Fake<IRepository<Magazine>>();
        _validator = A.Fake<IValidator<Magazine>>();
        _magazineService = A.Fake<IMagazineService>();
        _controller = new MagazineController(_magazineService, _repository, _validator);
    }

    [Fact]
    public async void MagazineController_GetAll_ReturnOK()
    {
        //Arrange
        var magazines = new List<Magazine>
        {
            new Magazine { Id = "E606B5E376CF989C7680588A", MagazineName = "Book1", Author = "Author1", Price = 5 },
            new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book2", Author = "Author2", Price = 6 }
        };
        A.CallTo(() => _repository.GetAsync()).Returns(magazines);

        //Act
        var result = await _controller.Get();

        //Assert
        result.Should().BeEquivalentTo(magazines);
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeOfType<List<Magazine>>();
    }

    [Fact]
    public async void MagazineController_GetAll_ReturnNotFound()
    {
        //Arrange
        var magazines = new List<Magazine>();
        A.CallTo(() => _repository.GetAsync()).Returns(magazines);
        //var controller = new BooksController(_booksService, _validator, _repository);

        //Act
        var result = await _controller.Get();

        //Assert
        result.Should().BeEquivalentTo(magazines);
        result.Should().NotBeNull();
        //result.Should().HaveCount(2);
        result.Should().BeOfType<List<Magazine>>();
    }

    public async void MagazineController_GetById_ReturnOK()
    {
        //Arrange
        var magazines = new List<Magazine>
        {
            new Magazine { Id = "E606B5E376CF989C7680588A", MagazineName = "Book1", Author = "Author1", Price = 5 },
            new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book2", Author = "Author2", Price = 6 }
        };
        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(magazines[0]);

        //Act
        var serializeResult = await _controller.Get("E606B5E376CF989C7680588A");
        var temp = (OkObjectResult)serializeResult.Result;
        var result = temp.Value;

        //Assert
        result.Should().BeEquivalentTo(magazines[0]);
        result.Should().BeSameAs(magazines[0]);
        result.Should().NotBeNull();
        result.Should().BeOfType<Book>();
    }

    [Fact]
    public async void MagazineController_GetById_ReturnNotFound()
    {
        //Arrange
        var magazines = new List<Magazine>
        {
            new Magazine { Id = "E606B5E376CF989C7680588A", MagazineName = "Book1", Author = "Author1", Price = 5 },
            new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book2", Author = "Author2", Price = 6 }
        };
        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(magazines[0]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D1")).Returns(magazines[1]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D2")).Returns((Magazine)null);

        //Act
        var result = await _controller.Get("BAB491765705B050F5E695D2");


        //Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();

        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be("Entity not found");
    }
    [Fact]
    public async void MagazineController_Update_ReturnNotFound()
    {
        //Arrange
        var magazines = new List<Magazine>
        {
            new Magazine { Id = "E606B5E376CF989C7680588A", MagazineName = "Book1", Author = "Author1", Price = 5 },
            new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book2", Author = "Author2", Price = 6 }
        };
        var editedMagazine = new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book3", Author = "Author3", Price = 9 };

        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(magazines[0]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D1")).Returns(magazines[1]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D2")).Returns((Magazine)null);

        //Act
        var result = await _controller.Update("BAB491765705B050F5E695D2", editedMagazine);
        result = (NotFoundResult)result;


        //Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async void MagazineController_Update_ReturnBadRequest()
    {
        //Arrange
        var magazines = new List<Magazine>
        {
            new Magazine { Id = "E606B5E376CF989C7680588A", MagazineName = "Book1", Author = "Author1", Price = 5 },
            new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book2", Author = "Author2", Price = 6 }
        };
        var editedMagazine = new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book3", Author = "Author3", Price = 9 };

        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(magazines[0]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D1")).Returns(magazines[1]);

        //Act
        var result = await _controller.Update("BAB491765705B050F5E695D1", editedMagazine);

        result = (BadRequestObjectResult)result;


        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task BookController_Update_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        //var id = "BAB491765705B050F5E695D1";
        //var existingEntity = new Book(); // Create an instance of the entity
        //var updatedEntity = new Book(); // Create an instance of the entity

        //Arrange
        var magazines = new List<Magazine>
        {
            new Magazine { Id = "E606B5E376CF989C7680588A", MagazineName = "Book1", Author = "Author1", Price = 5 },
            new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book2", Author = "Author2", Price = 6 }
        };
        var editedMagazine = new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book3", Author = "Author3", Price = 9 };

        A.CallTo(() => _repository.GetAsync("E606B5E376CF989C7680588A")).Returns(magazines[0]);
        A.CallTo(() => _repository.GetAsync("BAB491765705B050F5E695D1")).Returns(magazines[1]);


        A.CallTo(() => _validator.ValidateAsync(A<ValidationContext<Magazine>>.Ignored, default))
            .Returns(new FluentValidation.Results.ValidationResult());

        // Act
        var result = await _controller.Update("BAB491765705B050F5E695D1", editedMagazine);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async void MagazineController_Create_ReturnCreated()
    {
        //Arrange
        var magazine = new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book3", Price = 51, Author = "Author3"};

        A.CallTo(() => _validator.ValidateAsync(A<ValidationContext<Magazine>>.Ignored, default))
                    .Returns(new FluentValidation.Results.ValidationResult());
        A.CallTo(() => _repository.CreateAsync(magazine)).Returns(Task.CompletedTask);

        //Act
        var result = await _controller.Post(magazine);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async void BooksController_CreateMany_ReturnCreated()
    {
        //Arrange
        var magazines = new Magazine[]
        {
            new Magazine { Id = "E606B5E376CF989C7680588A", MagazineName = "Book1", Author = "Author1", Price = 5 },
            new Magazine { Id = "BAB491765705B050F5E695D1", MagazineName = "Book2", Author = "Author2", Price = 6 }
        };

        A.CallTo(() => _validator.ValidateAsync(A<Magazine>.Ignored, default))
                    .Returns(new FluentValidation.Results.ValidationResult());

        A.CallTo(() => _repository.CreateMultipleAsync(magazines)).Returns(Task.CompletedTask);

        //Act
        var result = await _controller.Post(magazines);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

}
