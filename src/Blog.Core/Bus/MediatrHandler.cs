using Blog.Core.Messages;
using MediatR;

namespace Blog.Core.Bus
{
    public class MediatrHandler : IMediatrHandler
    {
        private readonly IMediator _mediator;

        public MediatrHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEvent(Event evento)
        {
            await _mediator.Publish(evento);
        }
    }

    public interface IMediatrHandler
    {
        Task PublishEvent(Event evento);
    }
}
