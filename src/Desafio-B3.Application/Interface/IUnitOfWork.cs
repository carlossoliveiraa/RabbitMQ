using ImplementandoRabiitMQ.Application.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplementandoRabiitMQ.Application.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Tarefa> tarefas { get; }
     
        int Complete();
    }
}
