using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

namespace BookStoreApi.Tests.ValidationTest;

public class BookValidationTest
{
    private readonly IValidator<Book> _validator;

    public BookValidationTest()
    {
        _validator = new BookValidator();
    }

    [Fact]
    public void BookName_IsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var book = new Book
        {
            BookName = "",
            Author = "Some Author",
            Category = "Category",
            Price = 5
        };

        var result = _validator.TestValidate(book);

        result.ShouldHaveValidationErrorFor(a => a.BookName);
    }
    
    [Fact]
    public void Author_IsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var book = new Book
        {
            BookName = "exist",
            Author = "",
            Category = "Category",
            Price = 5
        };

        var result = _validator.TestValidate(book);

        result.ShouldHaveValidationErrorFor(a => a.Author);
    }
 
    [Fact]
    public void Category_IsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var book = new Book
        {
            BookName = "exist",
            Author = "Author1",
            Category = "",
            Price = 5
        };

        var result = _validator.TestValidate(book);

        result.ShouldHaveValidationErrorFor(a => a.Category);
    }
 
    [Fact]
    public void Price_IsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var book = new Book
        {
            BookName = "exist",
            Author = "",
            Category = "Category",
            Price = null
        };

        var result = _validator.TestValidate(book);

        result.ShouldHaveValidationErrorFor(a => a.Author);
    }

    [Fact]
    public void OnlyNumeric_Update_Validation_IsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var book = new Book
        {
            BookName = "123",
            Author = "great",
            Category = "Category",
            Price = null
        };

        var context = new ValidationContext<Book>(book);
        context.RootContextData["RequestType"] = "PUT";


        var result = _validator.TestValidate(context);

        result.ShouldHaveValidationErrorFor(a => a.BookName);
    }
 
}
