namespace CodingBlog.Models;

public class PostsPorTagViewModel
{
    public PostsPorTagViewModel(List<PostViewModel> posts )
    {
        Posts = posts; 
    }

    public List<PostViewModel> Posts { get; } 
}