using Blog.Posts.Domain.Repositories;
using MediatR;

namespace Blog.Posts.Domain.Events;

public class UpdateQtdPostsCategoryEventHandler : INotificationHandler<UpdateQtdPostsCategoryEvent>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateQtdPostsCategoryEventHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(UpdateQtdPostsCategoryEvent notification, CancellationToken cancellationToken)
    {
        Categoria category = await _categoryRepository.ObterPorId(notification.CategoriaId);

        category.IncrementarQtdPosts();

        await _categoryRepository.Salvar();
    }
}
