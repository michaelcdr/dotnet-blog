using FluentValidation;

namespace CodingBlog.Areas.Admin.Features.Login;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.UserName)
            .NotEmpty()
            .WithMessage("Informe o usuario.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Informe a senha.");
    }
}
