using Blog.Auth.Jwt;
using Blog.Auth.Models;
using Blog.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.Auth.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenGenerator _tokenGenerator;
    private const string MSG_ERRO = "Não foi possivel cadastrar o usuário.";
    private const string MSG_SUCESSO = "Usuário cadastrado com sucesso.";
    private const string MSG_ERRO_LOGAR = "Não foi possivel logar.";
    private const string MSG_ERRO_TOKEN = "Usuário registrado mas, não foi possivel gerar o token.";
    private const string ROLE_COMUM = "comum";
    private const string ROLE_VENDEDOR = "vendedor";
    private const string ROLE_ADMIN = "admin";

    public AuthService(UserManager<IdentityUser> userManager,
                       SignInManager<IdentityUser> signInManager,
                       ITokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<AppResponse<TokenGeneratedResponse>> Login(UserLogin request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);

        if (!result.Succeeded)
            return new AppResponse<TokenGeneratedResponse>(false, MSG_ERRO_LOGAR, new List<Notification> {
                new Notification(MSG_ERRO_LOGAR,"")
            });

        TokenGeneratedResponse? tokenResultado = await _tokenGenerator.Generate(request.UserName);

        if (tokenResultado == null)
            return new AppResponse<TokenGeneratedResponse>(false, MSG_ERRO_LOGAR, new List<Notification> {
                new Notification(MSG_ERRO_LOGAR,"")
            });

        return new AppResponse<TokenGeneratedResponse>(true, "Logado com sucesso.")
        {
            Data = tokenResultado
        };
    }
}