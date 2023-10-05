using ImplementandoRabiitMQ.Application.Domain;
using ImplementandoRabiitMQ.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace ImplementandoRabiitMQ.Apresentation.Controllers
{
    [Route("api/Rabbit")]
    [ApiController]
    public class RabbitMQController : Controller
    {
        private readonly IProducer _producer;
        private readonly IConsumer _consumer;

        public RabbitMQController(IProducer producer, IConsumer consumer)
        {
            _producer = producer;
            _consumer = consumer;
        }

        [HttpPost("Producer")]
        public async Task<IActionResult> Produzir([FromBody] Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                {
                    return BadRequest("Tarefa inválida");
                }

                await _producer.ProducerAsync(JsonConvert.SerializeObject(tarefa));

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
                
    }
}
