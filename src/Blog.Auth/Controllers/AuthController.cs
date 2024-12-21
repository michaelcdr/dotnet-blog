using Blog.Auth.Jwt;
using Blog.Auth.Models;
using Blog.Auth.Services;
using Blog.Core.Controller;
using Blog.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Auth.Controllers;

[Route("api/conta")]
public class AuthController : MainApiController
{
    private readonly IAuthService _userService;

    public AuthController(IAuthService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Gera um token para usar na autenticação da API
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLogin model)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        AppResponse<TokenGeneratedResponse> response = await _userService.Login(model);

        if (!response.Success)
        {
            foreach (var item in response.Errors)
                AddError(item.Message);

            return CustomResponse();
        }

        return CustomResponse(response.Data);
    }
}