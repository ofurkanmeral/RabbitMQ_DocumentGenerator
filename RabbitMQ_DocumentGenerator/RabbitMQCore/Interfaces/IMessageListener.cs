using RabbitMQ.Client;

namespace RabbitMQ_DocumentGenerator.RabbitMQCore.Interfaces
{
    public interface IMessageListener<T>
    {
        T StartListening(string queueName);
    }
}
