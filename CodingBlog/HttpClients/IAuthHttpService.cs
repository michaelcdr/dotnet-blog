using CodingBlog.Models;

namespace CodingBlog.HttpClients;

public interface IAuthHttpService
{
    Task<TokenResponse> Login(LoginModel loginModel);
}