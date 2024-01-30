using E_Commerce.DTOS;
using E_Commerce.Model;
using E_Commerce.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.Repository.Classes
{
    public class ProductRepo : BaseRepo<Product> , IProductRepo
    {
        private readonly AppDbContext context;

        public ProductRepo( AppDbContext context ) : base( context ) { 
            this.context = context;
        }
      

        public async Task<IEnumerable<ProductsDTO>> ViewProucts(Expression<Func<Product, bool>> criteria)
        {
            var products  = await context.Products.Where( criteria ).ToListAsync();
            List<ProductsDTO> res = new List<ProductsDTO>();
            foreach ( var product in products )
            {
                res.Add(new ProductsDTO
                {
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price,
                });
            }
            return res;
        }

        public async Task<ProductViewDTO>ViewProduct(int productId)
        {
            var product = await context.Products.SingleOrDefaultAsync(p=>p.Id == productId);
            if (product == null)
                return null;
            return new ProductViewDTO
            {
                Category = product.Category,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Rating = product.Rating,
                Reviews = product.Reviews,
                Quantity = product.Quantity,//just to update availablity
            };
        }
    }
}
