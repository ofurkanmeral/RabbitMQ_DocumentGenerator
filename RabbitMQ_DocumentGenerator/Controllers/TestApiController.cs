using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ_DocumentGenerator.RabbitMQCore.Services;
using System.Net;

namespace RabbitMQ_DocumentGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        private readonly IConnection _connection;
        private readonly string createDocument = "create_document_q";
        private readonly string documentCreated = "document_created_q";
        private readonly string documentCreatedExchange = "document_created_exchanges";
        public TestApiController(IConnection connection)
        {
            _connection = connection;
        }

        public  class RequestModel
        {
            public Guid Id { get; set; }
            public string Filename { get; set; }
            public DocumentType DocumentType { get; set; }
            public string RequestMessage { get; set; }
            public bool Status { get; set; }
        }
        public enum DocumentType
        {
            Pdf=1,
            Xlsx=2,
            Doc=3,
            Txt=4
        }
        [HttpGet]
        public   RequestModel Index()
        {
            RabbitMQService sendMessage = new RabbitMQService(_connection);
            var channel= sendMessage.PublishMessageAsync(createDocument, new RequestModel { Id = Guid.NewGuid(), Filename="OnurFurkanMeral",DocumentType=DocumentType.Pdf,Status=false });

            //Pdf hazırlanmaya başlayacak bildirimi için request statusu false'dan true'ya çektik
            RabbitMQListenerService<RequestModel> listener = new RabbitMQListenerService<RequestModel>(_connection);
            var result = listener.StartListening(createDocument);
            result.Status = true;
            result.RequestMessage = "Pdf Kuyruğa Başarı İle Yüklendi";
            return result;

        }
    }
}
