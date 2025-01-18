using System.ComponentModel.DataAnnotations;

namespace CodingBlog.Models;

public class CategoriaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int QtdPosts { get; set; }
}

public class LoginModel
{
    [Required(ErrorMessage = "Informe o {0}")]
    [Display(Name = "Usuário")]
    public string? UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a {0}")]
    [Display(Name = "Senha")]
    public string? Password { get; set; } = string.Empty;
}

public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserToken UserToken { get; set; }
    public ResponseResult ResponseResult { get; set; }
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

    public string Title { get; set; }
    public int Status { get; set; }
    public ResponseErrorMessages Errors { get; set; }
}

public class ResponseErrorMessages
{
    public ResponseErrorMessages()
    {
        Mensagens = new List<string>();
    }

    public List<string> Mensagens { get; set; }
}

public class UserClaim
{
    public string? Type { get; set; }
    public string? Value { get; set; }
}

public class CategoriaCadastroModel
{
    public string Nome { get; set; }
}

public class CategoriaEdicaoModel
{
    public string Nome { get; set; }
    public int CategoriaId { get; set; }
}