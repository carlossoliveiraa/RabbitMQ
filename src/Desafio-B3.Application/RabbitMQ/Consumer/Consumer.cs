using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using ImplementandoRabiitMQ.Application.Interface;
using ImplementandoRabiitMQ.Application.Domain;
using Newtonsoft.Json;
using Serilog;

namespace ImplementandoRabiitMQ.Application.RabbitMQ.Consumer
{
    public class Consumer : IConsumer
    {
        private readonly IUnitOfWork _unitOfWork;

        public Consumer(IUnitOfWork unitOfWork)
        {
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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                string message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                Log.Information($"Mensagem recebida: {message}");
            };

            channel.BasicConsume(queue: "tarefas-fila",
                                 autoAck: true,
                                 consumer: consumer);
 
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
                // Trate ou registre a exceção conforme necessário
                throw ex;
            }
        }

    }
}
