using FluentValidation;

namespace Blog.Auth.Features.Login;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.UserName)
            .NotEmpty()
            .WithMessage("Informe o UserName");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Informe a Password");
    }
}
