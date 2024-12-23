namespace Blog.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
