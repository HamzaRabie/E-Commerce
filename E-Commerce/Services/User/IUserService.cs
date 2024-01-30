using E_Commerce.DTOS;
using E_Commerce.Model;

namespace E_Commerce.Services.User
{
    public interface IUserService
    {
        Task<string> AddToCard(int productID, string userId, int quantity);
        Task<IEnumerable<CartViewDTO>> ViewCard(string userId);
        Task<string> RemoveFromCard(int productID, string userId, int quantity);
        Task<string> CheckOut(string userId);
        Task<List<OrderDTO>> ViewOrders(string userId);
        Task<string> CancelOrder(string userId, string orderN);
        Task<string> TrackOrder(string userId, string orderN);
        Task<List<NotificationDTO>> GetNotifications(string userId);
        Task<string> AddReview(string userId, int proudctId, string review);
        Task SellProduct(string userId, ProductCreationDTO product);
        Task<List<Product>> ViewMyProducts(string userId);
    }
}
