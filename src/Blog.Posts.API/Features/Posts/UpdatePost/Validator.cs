using FluentValidation;

namespace Blog.Posts.API.Features.Posts.UpdatePost;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Id)
            .GreaterThan(0)
            .WithMessage("Informe o Id do post.");

        RuleFor(request => request.Categoria)
            .NotEmpty()
            .WithMessage("Informe a Categoria");

        RuleFor(request => request.Titulo)
            .NotEmpty()
            .WithMessage("Informe o Titulo");

        RuleFor(request => request.Descritivo)
            .NotEmpty()
            .WithMessage("Informe o Descritivo");
    }
}
