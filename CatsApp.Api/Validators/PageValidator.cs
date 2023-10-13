using CatsApp.Api.Models;
using CatsApp.Domain;
using CatsApp.Domain.Aggregates;
using FluentValidation;

namespace CatsApp.Api.Validators;

public class PageValidator : AbstractValidator<SearchModel>
{
    public PageValidator()
    {
        RuleFor(x => x.SearchText).MaximumLength(Cat.MaxNameLength);
        RuleFor(x => x.PageNum).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(Page<Cat>.MinSize).LessThanOrEqualTo(Page<Cat>.MaxSize);
    }
}
