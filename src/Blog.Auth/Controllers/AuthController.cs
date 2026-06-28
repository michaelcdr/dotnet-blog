using Blog.Core.Controller;
using Blog.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Login = Blog.Auth.Features.Login;

namespace Blog.Auth.Controllers;

[Route("api/conta")]
public class AuthController : MainApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromServices] Login.Handler handler,
        [FromServices] IValidator<Login.Request> validator,
        Login.Request request,
        CancellationToken cancellationToken)
    {
        if (!await Validate(request, validator, cancellationToken))
            return CustomResponse();

        var response = await handler.Handle(request, cancellationToken);
        return BuildResponse(response);
    }

    private IActionResult BuildResponse<T>(AppResponse<T> response)
    {
        if (response.Success)
            return CustomResponse(response.Data);

        foreach (var item in response.Errors)
            AddError(item.Message);

        return CustomResponse();
    }

    private async Task<bool> Validate<TRequest>(
        TRequest request,
        IValidator<TRequest> validator,
        CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(request, cancellationToken);

        foreach (var error in result.Errors)
            AddError(error.ErrorMessage);

        return result.IsValid;
    }
}
