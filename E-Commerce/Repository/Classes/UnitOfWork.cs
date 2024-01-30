using E_Commerce.Model;
using E_Commerce.Repository.Interfaces;

namespace E_Commerce.Repository.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public IProductRepo Products { get; private set; }

        public UnitOfWork( AppDbContext context )
        {
            this.context = context;
            Products = new ProductRepo(context);
        }

        public int complete()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
