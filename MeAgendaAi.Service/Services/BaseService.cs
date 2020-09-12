using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces;
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<T> InsertAsync(T item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<T> SelectAsync(Guid id)
        {
            return await _repository.SelectAsync(id);
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            return await _repository.SelectAsync();
        }

        public async Task<T> UpdateAsync(T item)
        {
            return await _repository.UpdateAsync(item);
        }
    }
}
