using FluentValidation;

namespace Blog.Posts.API.Features.Categories.DeleteCategory;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Id)
            .GreaterThan(0)
            .WithMessage("Informe a categoria.");

        RuleFor(request => request.DestinationCategoryId)
            .NotEqual(request => request.Id)
            .When(request => request.DestinationCategoryId.HasValue)
            .WithMessage("A categoria de destino deve ser diferente da categoria removida.");
    }
}
