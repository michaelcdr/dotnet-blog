using CodingBlog.Models;

namespace CodingBlog.Services;

public interface IAuthHttpService
{
    Task<TokenResponse> Login(LoginModel loginModel);
}