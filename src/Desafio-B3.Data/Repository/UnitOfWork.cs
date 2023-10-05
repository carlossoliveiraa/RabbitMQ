using ImplementandoRabiitMQ.Application.Domain;
using ImplementandoRabiitMQ.Application.Interface;
using ImplementandoRabiitMQ.Data.Context;

namespace ImplementandoRabiitMQ.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly B3DbContext _context;

        public IRepository<Tarefa> tarefas { get; private set; }

        public UnitOfWork(B3DbContext context)
        {
            _context = context;
            tarefas = new Repository<Tarefa>(_context);
        }
        
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
