namespace CodingBlog.Services;

public class ApiValidationException : Exception
{
    public ApiValidationException(IEnumerable<string> errors)
        : base("A requisicao para a API retornou erro de validacao.")
    {
        Errors = errors.ToArray();
    }

    public IReadOnlyCollection<string> Errors { get; }
}
