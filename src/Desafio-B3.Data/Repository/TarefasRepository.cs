using ImplementandoRabiitMQ.Application.Domain;
using ImplementandoRabiitMQ.Application.Interface;
using ImplementandoRabiitMQ.Data.Context;

namespace ImplementandoRabiitMQ.Data.Repository
{
    public class TarefasRepository : Repository<Tarefa>, ITarefasRepository
    {
        public TarefasRepository(B3DbContext context) : base(context)
        {
        }       
    }
}
