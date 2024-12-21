using Blog.Auth.Jwt;
using Blog.Auth.Models;
using Blog.Core.Models;

namespace Blog.Auth.Services;

public interface IAuthService
{
    Task<AppResponse<TokenGeneratedResponse>> Login(UserLogin request);
}