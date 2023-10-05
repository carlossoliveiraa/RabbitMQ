using ImplementandoRabiitMQ.BackgroundServices.Interface;

namespace ImplementandoRabiitMQ.BackgroundServices.Configuration
{
    public class InfoWork : IHostedService
    {
        private readonly IConsumerService _consumer;
        private TaskCompletionSource<bool> _completionSource;

        public InfoWork(IConsumerService consumer)
        {
            _consumer = consumer;
            _completionSource = new TaskCompletionSource<bool>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Processo Iniciado");
            await _consumer.ConsumerAsync();
            _completionSource.SetResult(true);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _completionSource.Task;
            Console.WriteLine("Processo Finalizado");
        }
    }

}
