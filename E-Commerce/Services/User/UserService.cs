using E_Commerce.DTOS;
using E_Commerce.Migrations;
using E_Commerce.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using E_Commerce.Services.Business;
using E_Commerce.Repository.Classes;

namespace E_Commerce.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailService;

        public UserService(AppDbContext context, UserManager<ApplicationUser> userManager , IEmailSender emailService )
        {
            this.context = context;
            this.userManager = userManager;
            this.emailService = emailService;
        }
        public async Task<string> AddToCard(int productID, string userID, int quantity)
        {
            var user = await userManager.Users.SingleAsync(u => u.Id == userID);
            var product = await context.Products.FindAsync(productID);
            if (product != null)
            {
                var duplicateProduct = await context.UsersCarts.SingleOrDefaultAsync(c => c.User.Id == userID && c.Product.Id == productID);
                if (duplicateProduct != null)
                {
                    duplicateProduct.Qunatity += quantity;
                    user.MyNotifications.Add(new Notification
                    {
                        Date = DateTime.Now,
                        notification = $"You have increased the product   ( name : {product.Name} , Discription : {product.Description} ) quantity to {duplicateProduct.Qunatity}",
                    });

                    return "Product Addedd Successfully";
                }
                await context.UsersCarts.AddAsync(new UserCart
                {
                    Date = DateTime.Now,
                    Qunatity = quantity,
                    Product = product,
                    User = user,
                });
                user.MyNotifications.Add(new Notification
                {
                    Date = DateTime.Now,
                    notification = $"You have added new product ( name : {product.Name} , Discription : {product.Description} ) to your cart",

                });
                return "Product Addedd Successfully";
               
            }
            return "Sorry there is problem within the system";
        }
        public async Task<string> RemoveFromCard(int productID, string userID, int quantity)
        {
            var user = await userManager.Users.Include(u=>u.MyCart).SingleAsync(u => u.Id == userID);
            var Product = await context.UsersCarts.Include(c=>c.Product).FirstOrDefaultAsync(c => c.User.Id == userID && c.Product.Id == productID);
            if (Product != null && (quantity == Product.Qunatity || quantity > Product.Qunatity))
            {
                user.MyNotifications.Add(new Notification
                {
                    Date = DateTime.Now,
                    notification = $"You have remove  product ( name : {Product.Product.Name} , Discription : {Product.Product.Description} ) from your cart"
                });
                context.UsersCarts.Remove(Product);
                return "Product Removed Successfully";
            }
            else if (Product != null && Product.Qunatity > quantity)
            {
                Product.Qunatity -= quantity;
                
                user.MyNotifications.Add(new Notification
                {
                    Date = DateTime.Now,
                    notification = $"You have decreased the product   ( name : {Product.Product.Name} , Discription : {Product.Product.Description} ) quantity to {Product.Qunatity}"
                });

                return "Done";
            }
            else
                return "Sorry there is problem within the system";

        }

        public async Task<IEnumerable<CartViewDTO>> ViewCard(string userID)
        {
            var userProducts = await context.UsersCarts.Where(c => c.User.Id == userID).Include(c => c.Product).ToListAsync();
            List<CartViewDTO> userCart = new List<CartViewDTO>();
            foreach (var product in userProducts)
            {
                userCart.Add(new CartViewDTO
                {
                    ProductDiscription = product.Product.Description,
                    ProductName = product.Product.Name,
                    ProductPrice = product.Product.Price,
                    Quantity = product.Qunatity
                });
            }
            return userCart;
        }
        public async Task<string> CheckOut(string userID)
        {
            var user = await userManager.Users.Include(u => u.MyOrders).Include(u => u.MyCart).SingleAsync(u => u.Id == userID);
            var userProducts = await context.UsersCarts.Where(c => c.User.Id == userID).Include(c => c.Product).ToListAsync();
            if (userProducts.Count == 0)
                return "You have no item in your cart";
            double Payment = 0;
            var productsDb = new List<Product>();
            var quantities = new List<KeyValuePair<int, int>>();
            var sortedQuantities = new List<int>();
            foreach (var item in userProducts)
            {
                if (item.Product.Quantity < item.Qunatity)
                    return $"Sorry This Qunatity of {item.Product.Name} is Not Available";
                Payment = Payment + item.Product.Price * item.Qunatity;
                item.Product.Quantity -= item.Qunatity;
                productsDb.Add(item.Product);
                if (item.Product.UserId != null)
                {
                    var seller = await userManager.FindByIdAsync(item.Product.UserId);
                    await emailService.SendEmailAsync(seller.Email, "E-Commerce", $"The user ( {user.UserName} ) has bought a {item.Qunatity} of your product ( {item.Product.Name} ) ");
                }
                quantities.Add(new KeyValuePair<int, int>(item.Product.Id, item.Qunatity));
            }
            quantities = quantities.OrderBy(o => o.Key).ToList();
            foreach (var item in quantities)
            {
                sortedQuantities.Add(item.Value);
            }
            var orderNumber = Guid.NewGuid().ToString();
            user.MyOrders.Add(new Order
            {
                Date = DateTime.Now,
                Products = productsDb,
                Price = Payment,
                Quantities = sortedQuantities,
                OrderNumber = orderNumber,
                Status = "Pending"
                
            });
            user.MyCart.Clear();
            user.MyNotifications.Add(new Notification
            {
                Date = DateTime.Now,
                notification = $"You have Created an order with number ({orderNumber}) and total payment {Payment} "
            });
            await emailService.SendEmailAsync(user.Email,"E-Commerce",$"You Made Order with number ( {orderNumber} ) and total payment={Payment}");
            return $"Total Payment is {Payment} ";
        }
        public async Task<List<OrderDTO>> ViewOrders(string userId)
        {
            //   var ordersDB = await context.Orders.Include(o => o.Products).Where(o => o.User.Id == userId).ToListAsync();
            var user = await userManager.Users.Include(u => u.MyOrders).ThenInclude(o => o.Products).SingleAsync(u => u.Id == userId);
            List<OrderDTO> userOrders = new List<OrderDTO>();//return
            foreach (var item in user.MyOrders)
            {
                int indx = 0;
                List<ProductOrderDTO> productsView = new List<ProductOrderDTO>();
                foreach (var product in item.Products)
                {
                    productsView.Add(new ProductOrderDTO
                    {
                        Description = product.Description,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = item.Quantities[indx],
                    });
                    indx++;
                }
                userOrders.Add(new OrderDTO
                {
                    OerderDate = item.Date,
                    ProductStatus = item.Status,
                    OerderPrice = item.Price,
                    Products = productsView,
                    OrderNumber = item.OrderNumber

                });
            }
            return userOrders;
        }
        public async Task<string> CancelOrder(string userId, string orderN)
        {
            var user = await userManager.FindByIdAsync(userId);
            var orderDB = await context.Orders.Include(o => o.Products).FirstAsync(o => o.OrderNumber == orderN && o.User.Id == userId);
            if (orderDB == null)
                return "The order number is inavlid";
            if (DateTime.Now >= orderDB.Date.AddDays(3))
                return "Sorry you cannot cancel the order after 3 days";
            int indx = 0;
            foreach (var item in orderDB.Quantities)
            {
                orderDB.Products[indx].Quantity += item;
                if(orderDB.Products[indx].UserId != null)
                {
                    var seller = await userManager.FindByIdAsync(orderDB.Products[indx].UserId);
                    await emailService.SendEmailAsync(seller.Email, "E-Commerce", $"The user ( {user.UserName} ) has Cancelled the order of a {orderDB.Products[indx].Quantity} of your product ( {orderDB.Products[indx].Name} ) ");

                }
                indx++;
            }
            context.Orders.Remove(orderDB);
            user.MyNotifications.Add(new Notification
            {
                Date = DateTime.Now,
                notification = $"You have Cancelled an order with number ({orderN}) "

            });
            await emailService.SendEmailAsync(user.Email, "E-Commerce", $"You have cancelled Order with number{orderN}");
            return "Order Cancelled Successfully";

        }
        public async Task<string> TrackOrder(string userId, string orderN)
        {
            var orderDb = await context.Orders.FirstAsync(o => o.OrderNumber == orderN && o.User.Id == userId);
            if (orderDb == null)
                return "The order number is inavlid";

            if (DateTime.Now >= orderDb.Date.AddDays(9)) orderDb.Status = "Delivered";
            else if (DateTime.Now >= orderDb.Date.AddDays(6)) orderDb.Status = "Shipped";
            else if (DateTime.Now >= orderDb.Date.AddDays(3)) orderDb.Status = "Processing";
            else orderDb.Status = "Pending";

            return orderDb.Status;

        }
        public async Task<List<NotificationDTO>> GetNotifications(string userId)
        {
            var user = await userManager.Users.Include(u=>u.MyNotifications).FirstOrDefaultAsync(u=>u.Id == userId);
            if (user.MyNotifications.Count == 0)
                return null;
            var notifications = new List<NotificationDTO>();
            foreach(var notification in user.MyNotifications)
            {
                notifications.Add(new NotificationDTO
                {
                    Date = notification.Date,
                    Notification = notification.notification
                });
            }
            return notifications;
        }
        public async Task<string> AddReview( string userId ,int productId, string review)
        {
            var user = await userManager.Users.Include(u => u.MyOrders).ThenInclude(o=>o.Products).SingleAsync(u=> u.Id == userId);
            var order = user.MyOrders.FirstOrDefault(o=>o.Products.Any(p=>p.Id == productId) && o.IsDelivered);
            if(order == null)
                return "You cannot add review to product you didnt buy it ";

            var product = await context.Products.FirstAsync(p => p.Id == productId);
            if (product.Reviews == null)
                product.Reviews = new List<string>();
            product.Reviews.Add( $"{user.UserName} : " + review);
            return "your review added successfully"; 
           
        }
        /*
        public async Task<string> Rate( string userId ,int productId , int rating )
        {
            if (rating < 0 || rating > 100)
                return "Enter Value between 0 and 100";
            var user = await userManager.Users.Include(u => u.MyOrders).ThenInclude(o => o.Products).SingleAsync(u => u.Id == userId);
            var order = user.MyOrders.FirstOrDefault(o => o.Products.Any(p => p.Id == productId));
            if (order == null)
                return "You cannot rate to product you didnt try it ";

            var product = await context.Products.FirstAsync(p => p.Id == productId);
            product.Rating = ( product.Rating + );

        }
        */
        public async Task SellProduct( string userId , ProductCreationDTO product  )
        {
            var user = await userManager.Users.Include(u=>u.MyProducts).SingleAsync (u=>u.Id == userId);
            user.MyProducts.Add( new Product
            {
                Image = product.Image,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,
                Quantity = product.Quantity,
                Creation_Date = DateTime.Now,
                Seller = user,
                UserId = userId
            } );

            user.MyNotifications.Add(new Notification
            {
                Date = DateTime.Now,
                notification = $"You have Offered Product ( {product.Name} ) "
            });
            await emailService.SendEmailAsync(user.Email, "E-Commerce", $"You have Offered Product ( {product.Name} )");

        }
        public async Task<List<Product>> ViewMyProducts(string userId)
        {
            var user = await userManager.Users.Include(u => u.MyProducts).SingleAsync(u => u.Id == userId);
            return user.MyProducts.Where(p=>p.Availability==true).ToList();
        }
    }
}
