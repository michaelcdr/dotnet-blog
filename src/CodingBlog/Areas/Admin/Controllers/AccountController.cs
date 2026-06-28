using CodingBlog.Models.Admin;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Login = CodingBlog.Areas.Admin.Features.Login;

namespace CodingBlog.Areas.Admin.Controllers;

[Area("Admin")]
[AllowAnonymous]
public class AccountController : Controller
{
    private readonly Login.Handler _handler;
    private readonly IValidator<Login.Request> _validator;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        Login.Handler handler,
        IValidator<Login.Request> validator,
        ILogger<AccountController> logger)
    {
        _handler = handler;
        _validator = validator;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAdmin(returnUrl);

        ViewData["ReturnUrl"] = returnUrl;
        return View(new AdminLoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] AdminLoginViewModel model, string? returnUrl = null, CancellationToken cancellationToken = default)
    {
        ViewData["ReturnUrl"] = returnUrl;

        var request = new Login.Request
        {
            UserName = model.UserName,
            Password = model.Password
        };

        var validation = await _validator.ValidateAsync(request, cancellationToken);

        foreach (var error in validation.Errors)
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

        if (!validation.IsValid)
            return View(model);

        var result = await _handler.Handle(request, cancellationToken);

        if (!result.Success)
        {
            _logger.LogWarning("Falha de login admin para o usuario {UserName}.", model.UserName);
            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(model);
        }

        return RedirectToAdmin(returnUrl);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    private IActionResult RedirectToAdmin(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Posts", new { area = "Admin" });
    }
}
