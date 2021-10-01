using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Shared;
using System.Text;
using webapi.Repositories;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageRepository messageRepository;
        public MessagesController(IMessageRepository messageRepository, ILogger<MessagesController> logger)
        {
            this.messageRepository = messageRepository;
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return $"Count of messages in {Configurations.QueueName} is {messageRepository.Count(Configurations.QueueName)}";
        }
        [HttpPost]
        public void Post([FromBody] string message)
        {
            _logger.LogTrace($"{nameof(MessagesController)}.{nameof(Post)} - {message}");
            messageRepository.Send(message);
        }
    }
}