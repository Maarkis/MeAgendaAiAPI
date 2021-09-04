using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

//using Microsoft.Data.SqlClient;

namespace MeAgendaAi.Data.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : class
    {
        protected readonly MeAgendaAiContext _context;
        private readonly SqlConnection Connection;

        public BaseRepository(MeAgendaAiContext context, IConfiguration _configuration)
        {
            _context = context;
            Connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString"));
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual T GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual void Add(T obj)
        {
            _context.Set<T>().Add(obj);
            _context.SaveChanges();
        }

        public virtual void Remove(T obj)
        {
            _context.Set<T>().Remove(obj);
            _context.SaveChanges();
        }

        public virtual void Edit(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}