using ImplementandoRabiitMQ.Application.Domain;
using ImplementandoRabiitMQ.Application.Interface;
using ImplementandoRabiitMQ.Application.RabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace ImplementandoRabiitMQ.Apresentation.Controllers
{
   
    [Route("api/tarefas")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;       
        private readonly IProducer _producer;
        private readonly IConsumer _consumer;

        public TarefasController(IProducer producer, IConsumer consumer, IUnitOfWork unitOfWork)
        {          
            _producer = producer;
            _consumer = consumer;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                {
                    return BadRequest("Tarefa inválida");
                }

                await _unitOfWork.tarefas.AddAsync(tarefa);
                _unitOfWork.Complete();

                return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
               
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var tarefa = await _unitOfWork.tarefas.GetByIdAsync(id);
                if (tarefa == null)
                {
                    return NotFound();
                }

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                Log.Information("Esta é uma mensagem de informação para validar a gravação de um log");
                var tarefas = await _unitOfWork.tarefas.GetAllAsync();
                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Tarefa tarefa)
        {
            try
            {
                if (tarefa == null || tarefa.Id != id)
                {
                    return BadRequest("Dados da tarefa inválidos");
                }

                var tarefaExistente = await _unitOfWork.tarefas.GetByIdAsync(id);
                if (tarefaExistente == null)
                {
                    return NotFound();
                }

                await _unitOfWork.tarefas.UpdateAsync(tarefa);

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var tarefa = await _unitOfWork.tarefas.GetByIdAsync(id);
                if (tarefa == null)
                {
                    return NotFound();
                }
                await _unitOfWork.tarefas.RemoveAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
