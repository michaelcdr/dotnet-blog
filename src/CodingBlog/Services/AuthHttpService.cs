using Blog.Core.Services;
using CodingBlog.Models;

namespace CodingBlog.Services;

public class AuthHttpService : ServiceBase, IAuthHttpService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthHttpService> _logger;

    public AuthHttpService(
        HttpClient client,
        ISerializerService serializerService,
        ILogger<AuthHttpService> logger) : base(serializerService)
    {
        _httpClient = client;
        _logger = logger;
    }

    public async Task<TokenResponse> Login(LoginModel loginModel, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.PostAsync("api/conta/login", FormatarConteudo(loginModel), cancellationToken);

            if (!ManipularResponseErrors(response))
            {
                _logger.LogWarning("Falha de autenticacao para o usuario {UserName}: credenciais invalidas ou validacao recusada.", loginModel.UserName);
                return BuildLoginError("Usuario ou senha invalidos.");
            }

            return await Deserializar<TokenResponse>(response);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Falha ao comunicar com a API de autenticacao.");
            return BuildLoginError("Nao foi possivel autenticar agora. Tente novamente em instantes.");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Tempo esgotado ao comunicar com a API de autenticacao.");
            return BuildLoginError("Nao foi possivel autenticar agora. Tente novamente em instantes.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao autenticar o usuario {UserName}.", loginModel.UserName);
            return BuildLoginError("Nao foi possivel autenticar agora. Tente novamente em instantes.");
        }
    }

    private static TokenResponse BuildLoginError(string message)
    {
        return new TokenResponse
        {
            ResponseResult = new ResponseResult
            {
                Errors = new ResponseErrorMessages
                {
                    Mensagens = [message]
                }
            }
        };
    }
}
