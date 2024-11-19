using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ_DocumentGenerator.RabbitMQCore.Interfaces;
using System.Diagnostics;
using System.Text;
using static RabbitMQ_DocumentGenerator.Controllers.TestApiController;

namespace RabbitMQ_DocumentGenerator.RabbitMQCore.Services
{
    public class RabbitMQListenerService<T> : IMessageListener<T> where T:class
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQListenerService(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
        }

        public T StartListening(string queueName)
        {
            _channel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);



            var consumer =new EventingBasicConsumer(_channel);

            T modelJson = null;

            var result = _channel.BasicGet(queueName, true);
            if (result == null)
            {
                Debug.WriteLine("Kuyrukta mesaj yok!");
            }
            else
            {
                var body = Encoding.UTF8.GetString(result.Body.ToArray());
                modelJson=JsonConvert.DeserializeObject<T>(body);
                Debug.WriteLine($"Mesaj alındı: {body}");
            }

            #region Asekron Delegate eventde sorun yaşadık
            //var mesaj = "";
            //consumer.Received += (model, response) =>
            //{

            //    var message = Encoding.UTF8.GetString(response.Body.ToArray());
            //    modelJson = JsonConvert.DeserializeObject<T>(message);
            //    mesaj = message;
            //    tcs.SetResult(modelJson);

            //};
            #endregion

            _channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);
            return  modelJson;
        }
    }
}
