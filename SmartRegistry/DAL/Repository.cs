using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartRegistry.Interfaces;

namespace SmartRegistry.DAL
{
    public class Repository : IRepository, IDisposable
    {
        private ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _context.Database.SetCommandTimeout(180);
        }

        public IQueryable<T> All<T>() where T : class
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> All<T>(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties) where T : class
        {
            DbSet<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query.Include(includeProperty);
            }
            return query.AsQueryable<T>();
        }

        public T Find<T>(int id) where T : class
        {
            return _context.Set<T>().Find(id);
        }

        public IQueryable<T> Search<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties) where T : class
        {
            try
            {
                DbSet<T> query = _context.Set<T>();

                foreach (var includeProperty in includeProperties)
                {
                    query.Include(includeProperty);
                }

                return query.Where(predicate).AsQueryable();
            }
            catch
            {
                throw;
            }
        }

        public int Count<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {
            try
            {
                DbSet<T> query = _context.Set<T>();

                return query.Where(predicate).Count();
            }
            catch
            {
                throw;
            }
        }

        public T AddEntity<T>(T entity) where T : class
        {
            try
            {
                _context.Set<T>().Add(entity);
                _context.Entry<T>(entity).State = EntityState.Added;
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public T UpdateEntity<T>(T entity) where T : class
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
            return entity;
        }

        public void DeleteEntity<T>(T entity) where T : class
        {
            _context.Entry<T>(entity).State = EntityState.Deleted;
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                throw;
            }
        }
        
        public void Detach<T>(T source) where T : class
        {
            _context.Entry<T>(source).State = EntityState.Detached;
        }

        public IQueryable<T> CloneEntity<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties) where T : class
        {
            try
            {
                DbSet<T> query = _context.Set<T>();

                foreach (var includeProperty in includeProperties)
                {
                    query.Include(includeProperty);
                }

                return query.Where(predicate).AsQueryable();
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}
