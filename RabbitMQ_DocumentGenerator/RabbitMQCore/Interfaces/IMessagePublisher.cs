using RabbitMQ.Client;

namespace RabbitMQ_DocumentGenerator.RabbitMQCore.Interfaces
{
    public interface IMessagePublisher
    {
        IModel PublishMessageAsync<T>(string queueName, T message);
    }
}
