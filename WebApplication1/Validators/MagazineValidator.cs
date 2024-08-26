using BookStoreApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace BookStoreApi.Validators;

public class MagazineValidator : AbstractValidator<Magazine>
{
    public MagazineValidator()
    {
        RuleFor(x => x.MagazineName).NotEmpty().WithMessage("Magazine name is required");
        RuleFor(x => x.MagazineName).MinimumLength(3).WithMessage("Magazine name must be at least 3 characters long");

        RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required");


        RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");

    }
}
