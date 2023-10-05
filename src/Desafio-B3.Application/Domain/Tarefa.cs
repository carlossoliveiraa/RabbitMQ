namespace ImplementandoRabiitMQ.Application.Domain
{
    public class Tarefa : Entity
    {
        public Tarefa(string descricao, DateTime data)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("A descrição da tarefa não pode ser vazia.");
                      
            Descricao = descricao;
            Data = data;
            Status = false;
        }

        public string Descricao { get; private set; }
        public DateTime Data { get; private set; }
        public bool Status { get; private set; }
        
    }   
}
