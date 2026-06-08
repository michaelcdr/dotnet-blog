using Blog.Core.Bus;
using Blog.Posts.Domain.Events;
using Blog.Posts.Domain.Repositories;

namespace Blog.Posts.Domain.Services.CadastrarPost;

public class CadastrarPost : ICadastrarPost
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMediatrHandler _bus;

    public CadastrarPost(ICategoryRepository categoryRepository,
                         IPostRepository postRepository,
                         IMediatrHandler bus)
    {
        _categoryRepository = categoryRepository;
        _postRepository = postRepository;
        _bus = bus;
    }

    public async Task Executar(string titulo, string categoria, string description, List<string>tags, string imagem)
    {
        Categoria? category = await _categoryRepository.GetByName(categoria);

        if (category == null) throw new Exception("Categoria nao encontrada");
      
        var postCriado = new Post(
            0,
            titulo,
            description,
            string.Empty,
            "michael",
            string.Join(",", tags),
            category.Id
        );
        
        _postRepository.Add(postCriado);

        await _postRepository.UnitOfWork.Commit();

        var updateQtdEvent = new UpdateQtdPostsCategoryEvent(
            postCriado.Id,
            category.Id
        );

        await _bus.PublishEvent(updateQtdEvent);
    }
}
