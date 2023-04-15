using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        #region Properties
        protected eShopDbContext _context;
        #endregion

        #region Constructor
        protected BaseRepository(eShopDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Implements
        public bool Add(T entity)
        {
            _context.Add(entity);
            return true;
        }

        public List<T> All()
        {
            return _context.Set<T>().ToList();
        }

        public bool Delete(T entity)
        {
            _context.Remove(entity);
            return true;
        }

        public List<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>()
                .AsQueryable()
                .Where(predicate)
                .ToList();
        }

        public T Get(int id)
        {
            return _context.Find<T>(id);
        }

        public bool Update(T entity)
        {
            _context.Update(entity);
            return true;
        }

        //Commit--------------
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual async Task<List<T>> AllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.FindAsync<T>(id);
        }

        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .AsQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            var result = await _context.AddAsync(entity);
            return true;
        }


        public async Task  SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
