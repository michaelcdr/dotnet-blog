using NBomber.Contracts;
using NBomber.CSharp;
using System.Net.Http;
using System.Text;
using System.Text.Json;

// Código executado diretamente no arquivo principal.
Console.WriteLine("Iniciando teste de carga para o endpoint de criação de posts...");

//// Configuração do cliente HTTP
//var httpClient = new HttpClient
//{
//    BaseAddress = new Uri("https://sua-aplicacao.com/api/posts")
//};

//// Método para gerar o payload dinâmico
//string GeneratePayload()
//{
//    var post = new
//    {
//        Title = "Post de teste",
//        Content = "Este é um conteúdo de teste gerado automaticamente.",
//        Author = "Teste NBomber",
//        CreatedAt = DateTime.UtcNow
//    };
//    return JsonSerializer.Serialize(post);
//}

//// Cenário do teste de carga
//var createPostScenario = Scenario.Create("Teste de carga - Criar Post", async context =>
//{
//    var payload = GeneratePayload();
//    var content = new StringContent(payload, Encoding.UTF8, "application/json");

//    var response = await httpClient.PostAsync("", content);
//    return response.IsSuccessStatusCode
//        ? Response.Ok()
//        : Response.Fail();
//})
//.WithLoadSimulations(
//    Simulation.InjectPerSecRate(rate: 50, during: TimeSpan.FromSeconds(30)) // Simula 50 requisições por segundo durante 30 segundos
//);

//// Execução do teste de carga com NBomber
//NBomberRunner
//    .RegisterScenarios(createPostScenario)
//    .Run();

//Console.WriteLine("Teste de carga finalizado.");