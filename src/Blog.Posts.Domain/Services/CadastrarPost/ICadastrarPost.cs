namespace Blog.Posts.Domain.Services.CadastrarPost;

public interface ICadastrarPost
{
    Task Executar(string titulo, string categoria, string description, List<string> tags, string imagem);
}
