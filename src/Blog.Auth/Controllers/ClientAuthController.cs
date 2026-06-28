using Blog.Auth.Models;
using Blog.Core.Controller;
using Blog.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using GenerateClientToken = Blog.Auth.Features.GenerateClientToken;
using RefreshClientToken = Blog.Auth.Features.RefreshClientToken;

namespace Blog.Auth.Controllers;

[Route("api/clients")]
public class ClientAuthController : MainApiController
{
    [HttpPost("token")]
    public async Task<IActionResult> Token(
        [FromServices] GenerateClientToken.Handler handler,
        [FromServices] IValidator<GenerateClientToken.Request> validator,
        GenerateClientToken.Request request,
        CancellationToken cancellationToken)
    {
        if (!await Validate(request, validator, cancellationToken))
            return CustomResponse();

        var response = await handler.Handle(request, cancellationToken);
        return BuildResponse(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromServices] RefreshClientToken.Handler handler,
        [FromServices] IValidator<RefreshClientToken.Request> validator,
        RefreshClientToken.Request request,
        CancellationToken cancellationToken)
    {
        if (!await Validate(request, validator, cancellationToken))
            return CustomResponse();

        var response = await handler.Handle(request, cancellationToken);
        return BuildResponse(response);
    }

    private IActionResult BuildResponse(AppResponse<ClientTokenResponse> response)
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
