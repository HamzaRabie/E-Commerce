using E_Commerce.Model;

namespace E_Commerce.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IProductRepo Products { get; }
        int complete();
    }
}
