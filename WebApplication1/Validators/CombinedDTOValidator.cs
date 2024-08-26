using BookStoreApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace BookStoreApi.Validators;

public class CombinedDTOValidator : AbstractValidator<CombinedDTOModels>
{
    public CombinedDTOValidator()
    { 
        RuleFor(x => x.Books).NotEmpty().WithMessage("Books can't be empty");
        RuleFor(x => x.Magazines).NotEmpty().WithMessage("Magazine can't be empty");
        
    }
}
