using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Model
{
    public class ApplicationUser : IdentityUser
    {
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public  List<RefreshToken>? refreshTokens { get; set; }
        public List<UserCart>? MyCart { get; set; }
        public List<Order>? MyOrders { get; set; } = new List<Order>();
        public List<Notification>? MyNotifications { get; set; } = new List<Notification>();
        public List<Product> MyProducts { get; set; } = new List<Product>();

    }
}
