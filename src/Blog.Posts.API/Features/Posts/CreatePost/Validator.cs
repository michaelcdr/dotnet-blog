using FluentValidation;

namespace Blog.Posts.API.Features.Posts.CreatePost;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Categoria)
            .NotEmpty()
            .WithMessage("Informe a Categoria");

        RuleFor(request => request.Titulo)
            .NotEmpty()
            .WithMessage("Informe o Titulo");

        RuleFor(request => request.Descritivo)
            .NotEmpty()
            .WithMessage("Informe o Descritivo");

        RuleFor(request => request.Tags)
            .NotNull()
            .WithMessage("Informe as Tags");
    }
}
