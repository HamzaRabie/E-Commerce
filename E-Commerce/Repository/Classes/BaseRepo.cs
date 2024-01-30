using E_Commerce.Model;
using E_Commerce.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace E_Commerce.Repository.Classes
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly AppDbContext context;

        public BaseRepo( AppDbContext context )
        {
            this.context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }
      /*  public async Task<IEnumerable<T>> GetAllMatch(Expression<Func<T, bool>> criteria)
        {
             return  await context.Set<T>().Where( criteria ).ToListAsync();
            //return (IQueryable<T>)context.Products.Where(p => p.Name == productName).ToList();
        }*/
        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }
        public async Task<T> GetOneAsync(Expression<Func<T, bool>> criteria , string[] includes=null)
        {
            IQueryable<T> query = context.Set<T>();
            if (includes != null)
            {
                foreach (string include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(criteria);
        }
        public async Task<T> AddAsync(T model)
        {
            await context.Set<T>().AddAsync(model);
            return model;
        }
        public T Update(T model)
        {
            context.Set<T>().Attach(model);
            context.Entry(model).State = EntityState.Modified;
            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await context.Set<T>().FindAsync(id);
            if (model != null)
            {
                context.Set<T>().Remove(model);
                return true;
            }
            return false;
        }

    }
}
