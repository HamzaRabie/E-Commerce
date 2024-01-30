using E_Commerce.DTOS;
using E_Commerce.Model;
using System.Linq.Expressions;

namespace E_Commerce.Repository.Interfaces
{
    public interface IProductRepo:IBaseRepo<Product>
    {
        Task<IEnumerable<ProductsDTO>> ViewProucts( Expression<Func<Product,bool>> criteria );
        Task<ProductViewDTO> ViewProduct(int productId);
    }
}
