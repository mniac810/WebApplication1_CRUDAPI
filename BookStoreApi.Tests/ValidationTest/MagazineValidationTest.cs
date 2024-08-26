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

public class MagazineValidationTest
{
    private readonly IValidator<Magazine> _validator;

    public MagazineValidationTest()
    {
        _validator = new MagazineValidator();
    }

    [Fact]
    public void MagazineName_IsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var magazine = new Magazine
        {
            MagazineName = "",
            Author = "Some Author",
            Price = 5
        };

        var result = _validator.TestValidate(magazine);

        result.ShouldHaveValidationErrorFor(a => a.MagazineName); 
    }
    [Fact]
    public void MagazineName_NameLengthLessThan3_ShouldHaveValidationError()
    {
        // Arrange
        var magazine = new Magazine
        {
            MagazineName = "ab",
            Author = "Some Author",
            Price = 5
        };

        var result = _validator.TestValidate(magazine);

        result.ShouldHaveValidationErrorFor(a => a.MagazineName);
    }

    [Fact]
    public void Author_IsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var magazine = new Magazine
        {
            MagazineName = "exist",
            Author = "",
            Price = 5
        };

        var result = _validator.TestValidate(magazine);

        result.ShouldHaveValidationErrorFor(a => a.Author);
    }

    [Fact]
    public void Price_IsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var magazine = new Magazine
        {
            MagazineName = "exist",
            Author = "",
            Price = null
        };

        var result = _validator.TestValidate(magazine);

        result.ShouldHaveValidationErrorFor(a => a.Price);
    }
    
    [Fact]
    public void Price_IsLessThan0_ShouldHaveValidationError()
    {
        // Arrange
        var magazine = new Magazine
        {
            MagazineName = "exist",
            Author = "",
            Price = -5
        };

        var result = _validator.TestValidate(magazine);

        result.ShouldHaveValidationErrorFor(a => a.Price);
    }

}
