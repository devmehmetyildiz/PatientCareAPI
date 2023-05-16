using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class Repository<Tentity> : IRepository<Tentity> where Tentity : class
    {
        protected DbContext _context;
        private DbSet<Tentity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Tentity>();
        }

        public void Add(Tentity entity)
        {
            _dbSet.Add(entity);
            
        }

        public void AddRange(List<Tentity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public List<Tentity> GetAll()
        {
            return _dbSet.ToList();
        }

        public Tentity Getbyid(int id)
        {
            return _dbSet.Find(id);
        }

        public Tentity GetbyName(string Name)
        {
            return _dbSet.Find(Name);
        }

        public List<Tentity> GetTop(int count)
        {
            return _dbSet.Take(count).ToList();
        }

        public void Remove(int id)
        {
            _dbSet.Remove(Getbyid(id));
        }

        public void RemoveRange(List<Tentity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void update(Tentity olditem, Tentity newitem)
        {
            _context.Entry(olditem).CurrentValues.SetValues(newitem);
        }

        public T GetRecord<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            T item = _context.Set<T>().FirstOrDefault(predicate);
            return item;
        }

        public List<T> GetRecords<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            List<T> item = _context.Set<T>().Where(predicate).ToList();
            return item;
        }
    }
}
