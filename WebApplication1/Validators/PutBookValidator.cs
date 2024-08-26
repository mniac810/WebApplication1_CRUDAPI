using BookStoreApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using MongoDB.Driver.Core.Operations;

namespace BookStoreApi.Validators;

public class PutBookValidator : AbstractValidator<Book>
{
    public PutBookValidator()
    {
        RuleFor(x => x.BookName).Must(name => !IsNumeric(name)).WithMessage("Book name is required");

        //RuleFor(x => x.BookName)
        //    .Must(name => !IsNumeric(name))
        //    .WithMessage("Book name cannot consist of only numeric characters.");
        //    .When(context => IsPutRequest(context));



        RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }

    private bool IsNumeric(string name)
    {
        return name.All(char.IsDigit);
    }

    private bool IsPutRequest(ValidationContext<Book> context)
    {
        return context.RootContextData.TryGetValue("RequestType", out var requestType) &&
               requestType?.ToString() == "PUT";
    }
}
