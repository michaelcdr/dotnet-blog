using FluentValidation;

namespace Blog.Auth.Features.GenerateClientToken;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.ClientId)
            .NotEmpty()
            .WithMessage("Informe o ClientId");

        RuleFor(request => request.ClientSecret)
            .NotEmpty()
            .WithMessage("Informe o ClientSecret");
    }
}
