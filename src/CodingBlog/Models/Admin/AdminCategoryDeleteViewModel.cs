namespace CodingBlog.Models.Admin;

public class AdminCategoryDeleteViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int QtdPosts { get; set; }
    public int? DestinationCategoryId { get; set; }
    public string? NewCategoryName { get; set; }
    public IReadOnlyCollection<CategoryOptionViewModel> AvailableCategories { get; set; } = [];
}
