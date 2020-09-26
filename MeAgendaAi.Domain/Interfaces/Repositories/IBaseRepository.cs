using MeAgendaAi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface IBaseRepository<T> where T: class
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        void Add(T obj);
        void Remove(T obj);
        void Edit(T obj);
        void Dispose();
    }
}
