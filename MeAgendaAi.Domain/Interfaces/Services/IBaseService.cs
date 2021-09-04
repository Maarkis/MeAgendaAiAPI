using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.Entities;

namespace MeAgendaAi.Domain.Interfaces.Services
{
    public interface IBaseService<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        void Add(T obj);
        void Remove(T obj);
        void Edit(T obj);
        void Dispose();
    }
}