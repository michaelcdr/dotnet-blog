using FluentValidation;

namespace Blog.Posts.API.Features.Posts.CreateAdminPost;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.CategoriaId)
            .GreaterThan(0)
            .WithMessage("Informe a categoria.");

        RuleFor(request => request.Titulo)
            .NotEmpty()
            .WithMessage("Informe o titulo do post.");

        RuleFor(request => request.Descritivo)
            .NotEmpty()
            .WithMessage("Informe o conteudo do post.");
    }
}
