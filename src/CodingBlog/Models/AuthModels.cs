namespace CodingBlog.Models;

public class LoginModel
{
    public string? UserName { get; set; } = string.Empty;

    public string? Password { get; set; } = string.Empty;
}

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public double ExpiresIn { get; set; }
    public UserToken? UserToken { get; set; }
    public ResponseResult? ResponseResult { get; set; }
}

public class UserToken
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public IList<UserClaim> Claims { get; set; } = new List<UserClaim>();
}

public class ResponseResult
{
    public ResponseResult()
    {
        Errors = new ResponseErrorMessages();
    }

    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public ResponseErrorMessages Errors { get; set; }
}

public class ResponseErrorMessages
{
    public ResponseErrorMessages()
    {
        Mensagens = [];
    }

    public List<string> Mensagens { get; set; }
}

public class UserClaim
{
    public string? Type { get; set; }
    public string? Value { get; set; }
}
