using RabbitMQ.Client;
using System.Text;
using ImplementandoRabiitMQ.Application.Interface;
using Newtonsoft.Json;

namespace ImplementandoRabiitMQ.Application.RabbitMQ.Producer
{
    public class Producer : IProducer
    {
        public async Task ProducerAsync(string json)
        {

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                string queueName = "tarefas-fila";

                // Criando a fila (se não existir, será criada)
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);


                var messageBody = JsonConvert.SerializeObject(json);

                // Convertendo a mensagem em bytes
                var body = Encoding.UTF8.GetBytes(messageBody);

                // Publicando a mensagem na fila
                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);

            }
        }
    }
}
