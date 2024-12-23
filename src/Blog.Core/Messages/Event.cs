using MediatR;

namespace Blog.Core.Messages
{
    public abstract class Event : Message, INotification
    {
        public DateTime DataCriacao { get; private set; }
        protected Event()
        {
            DataCriacao = DateTime.Now;
        }
    }

    public abstract class Message
    {
        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
