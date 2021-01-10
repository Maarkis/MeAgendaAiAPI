using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeAgendaAi.Service.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public IEnumerable<T> GetAll() {
            return _repository.GetAll();
        }
        public T GetById(Guid id)
        {
            return _repository.GetById(id);
        }
        public void Add(T obj) {
            _repository.Add(obj);
        }
        public void Remove(T obj)
        {
            _repository.Remove(obj);
        }
        public void Edit(T obj)
        {
            _repository.Edit(obj);
        }
        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
