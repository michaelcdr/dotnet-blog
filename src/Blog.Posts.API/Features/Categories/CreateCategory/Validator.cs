using FluentValidation;

namespace Blog.Posts.API.Features.Categories.CreateCategory;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Nome)
            .NotEmpty()
            .WithMessage("Informe o nome da categoria.");
    }
}
