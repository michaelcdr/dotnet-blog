using Blog.Core.DomainObjects;

namespace Blog.Posts.Domain.Events
{
    public class UpdateQtdPostsCategoryEvent : DomainEvent
    {
        public UpdateQtdPostsCategoryEvent(int postId, int categoriaId)
        {
            PostId = postId;
            CategoriaId = categoriaId;
        }

        public int PostId { get; private set; }
        public int CategoriaId { get; private set; }

    }
}
