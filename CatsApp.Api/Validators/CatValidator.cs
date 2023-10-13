using CatsApp.Api.Models;
using CatsApp.Domain.Aggregates;
using FluentValidation;

namespace CatsApp.Api.Validators;

public class CatValidator : AbstractValidator<CatModel>
{
    public CatValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(Cat.MaxNameLength).WithMessage("The cat must have a name");
        RuleFor(x => x.Weight).GreaterThanOrEqualTo(Cat.MinWeight).LessThanOrEqualTo(Cat.MaxWeight);
        RuleFor(x => x.Age).GreaterThan(Cat.MinAge).LessThanOrEqualTo(Cat.MaxAge);
    }
}
