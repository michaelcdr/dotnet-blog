namespace CodingBlog.Areas.Admin.Features.Login;

public class Response
{
    public bool Success { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;

    public static Response Ok()
    {
        return new Response { Success = true };
    }

    public static Response Fail(string message)
    {
        return new Response
        {
            Success = false,
            ErrorMessage = message
        };
    }
}
