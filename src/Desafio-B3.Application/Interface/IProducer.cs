namespace ImplementandoRabiitMQ.Application.Interface
{
    public interface IProducer
    {
        Task ProducerAsync(string json);
    }
}
