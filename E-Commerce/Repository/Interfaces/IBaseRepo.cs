using System.Linq.Expressions;

namespace E_Commerce.Repository.Interfaces
{
    public interface IBaseRepo<T> where T : class
    {
        //crud 
        //get
        Task<IEnumerable<T>> GetAllAsync();
       // Task<IEnumerable<T>> GetAllMatch( Expression<Func<T,bool>> criteria );
       // Task<IQueryable<T>> GetAllMatchAsync(string productName);
        Task<T> GetByIdAsync(int id);
        Task<T> GetOneAsync(Expression<Func<T, bool>> criteria , string[] includes=null);

        //create 
        Task<T> AddAsync(T model);

        //update 
        T Update(T model);
        //delete
        Task<bool> DeleteAsync(int id);


    }
}
