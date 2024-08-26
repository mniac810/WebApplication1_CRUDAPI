using BookStoreApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using MongoDB.Driver.Core.Operations;

namespace BookStoreApi.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(x => x.BookName).NotEmpty().WithMessage("Book name is required");

        //RuleFor(x => x.BookName)
        //    .Must(name => !IsNumeric(name))
        //    .WithMessage("Book name cannot consist of only numeric characters.")
        //    .When(context => IsPutRequest(context));

        RuleFor(x => x.BookName).Custom((bookName, context) =>{
            if (bookName != null)
            {
                if (IsNumeric(bookName) && IsPutRequest(context))
                {
                    context.AddFailure("BookName", "Book name cannot consist of only numeric characters.");
                }
            }
        });

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
