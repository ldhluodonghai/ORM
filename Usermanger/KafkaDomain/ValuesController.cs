using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usermanger.KafkaDomain.Service;

namespace Usermanger.KafkaDomain
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly KafkaService _kafkaService;
        private readonly KafkaProducer _kafkaProducer;

        public ValuesController(KafkaService kafkaService, KafkaProducer kafkaProducer)
        {
            _kafkaService = kafkaService;
            _kafkaProducer = kafkaProducer;
        }
        [HttpGet]
        public IActionResult Test()
        {
           _kafkaProducer.MyConsumer();
            return Ok();
        }
    }
}
