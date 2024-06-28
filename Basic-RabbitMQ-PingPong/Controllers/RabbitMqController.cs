using Basic_RabbitMQ_PingPong.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace Basic_RabbitMQ_PingPong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private readonly IRabbitMqService _mqService;

        public RabbitMqController(IRabbitMqService mqService)
        {
            _mqService = mqService;
        }

        [Route("[action]/{message}")]
        [HttpGet]
        public IActionResult SendMessage(string message)
        {
            _mqService.SendMessage(message, "DefaultQueue");
            
            return Ok("Сообщение отправлено");
        }
    }
}
