using RabbitMQ.Client;
using System.Text;
using ImplementandoRabiitMQ.BackgroundServices.Interface;
using ImplementandoRabiitMQ.Application.Interface;
using ImplementandoRabiitMQ.Application.Domain;
using Newtonsoft.Json;

namespace ImplementandoRabiitMQ.BackgroundServices.Consumer
{
    public class ConsumerService : IConsumerService
    {
        private readonly ILogger<ConsumerService> _logger;
        private readonly IUnitOfWork _unitOfWork;
       
        public ConsumerService(ILogger<ConsumerService> logger, IUnitOfWork unitOfWork, ITarefasRepository tarefasRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;     
        }
        public async Task ConsumerAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "tarefas-fila",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

           
            while (true)
            {
                var result = channel.BasicGet(queue: "tarefas-fila", autoAck: true);

                if (result != null)
                {
                    string message = Encoding.UTF8.GetString(result.Body.ToArray());
                    Console.WriteLine(message);
                    await SaveMessageToDatabaseAsync(message);
                }
                else
                {
                    // Nenhuma mensagem disponível                   
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        private async Task SaveMessageToDatabaseAsync(string message)
        {
            try
            {
                object obj = JsonConvert.DeserializeObject<object>(message);

                 Tarefa tarefa = JsonConvert.DeserializeObject<Tarefa>(obj.ToString());

                await _unitOfWork.tarefas.AddAsync(tarefa);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
    }
}
