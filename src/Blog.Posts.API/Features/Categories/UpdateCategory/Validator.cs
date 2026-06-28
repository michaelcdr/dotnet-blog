using FluentValidation;

namespace Blog.Posts.API.Features.Categories.UpdateCategory;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Id)
            .GreaterThan(0)
            .WithMessage("Informe a categoria.");

        RuleFor(request => request.Nome)
            .NotEmpty()
            .WithMessage("Informe o nome da categoria.");
    }
}
