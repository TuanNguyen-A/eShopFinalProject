using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        List<T> All();
        T Get(int id);
        List<T> Find(Expression<Func<T, bool>> predicate);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);

        //Async
        Task<List<T>> AllAsync();
        Task<T> GetAsync(int id);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AddAsync(T entity);

        // Commit
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
