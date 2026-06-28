using FluentValidation;

namespace Blog.Auth.Features.RefreshClientToken;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.ClientId)
            .NotEmpty()
            .WithMessage("Informe o ClientId");

        RuleFor(request => request.RefreshToken)
            .NotEmpty()
            .WithMessage("Informe o RefreshToken");
    }
}
