using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ_DocumentGenerator.RabbitMQCore.Interfaces;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQ_DocumentGenerator.RabbitMQCore.Services
{
    public class RabbitMQService : IMessagePublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
        }

        public  IModel PublishMessageAsync<T>(string queueName, T message)
        {
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            _channel.QueueDeclare(queueName,false,false,false);

            var body=Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: properties,
                body: body
            );
            return _channel;
        }
    }
}
