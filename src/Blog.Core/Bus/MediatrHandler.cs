using Blog.Core.Messages;

namespace Blog.Core.Bus
{
    public class MediatrHandler : IMediatrHandler
    {
        public async Task PublishEvent(Event evento)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMediatrHandler
    {
        Task PublishEvent(Event evento);
    }
}
