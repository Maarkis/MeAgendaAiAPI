using System;
using System.Collections.Generic;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        void Add(T obj);
        void Remove(T obj);
        void Edit(T obj);
        void Dispose();
    }
}