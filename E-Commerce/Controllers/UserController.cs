using E_Commerce.DTOS;
using E_Commerce.Migrations;
using E_Commerce.Model;
using E_Commerce.Repository.Interfaces;
using E_Commerce.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;

        public UserController(IUnitOfWork UnitOfWork , IUserService userService)
        {
            unitOfWork = UnitOfWork;
            this.userService = userService;
        }

        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName( string name )
        {
            var res = await unitOfWork.Products.ViewProucts( p=>p.Name.Contains(name) );
            if(res is null) 
                return BadRequest("No product matches your search");
            return Ok(res);
        }

        [HttpGet("SearchByCategory")]
        public async Task<IActionResult> SearchByCategory(string category)
        {
            var res = await unitOfWork.Products.ViewProucts(p=>p.Category == category);
            if (res is null)
                return BadRequest($"No Product Of {category} availabe ");
            return Ok(res);
        }

        [HttpGet("ViewProduct/{id}")]
        public async Task<IActionResult> ViewProduct(int id)
        {
            var res = await unitOfWork.Products.ViewProduct(id);
            if (res is null)
                return Ok("Product Not Found");
            return Ok(res);
        }
        
        [HttpPost("AddToCart/{productId}")]
        public async Task<IActionResult> AddToCart(int productId , [FromBody]int quantity=1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await userService.AddToCard(productId, userId , quantity);
            unitOfWork.complete();
            return Ok(res);
        }
        
        [HttpPost("RemoveFromCart/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId , [FromBody] int quantity=0)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await userService.RemoveFromCard(productId, userId , quantity);
            unitOfWork.complete();
            return Ok(res);
        }
        
        [HttpGet("ViewCart")]
        public async Task<IActionResult> ViewCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await userService.ViewCard(userId);
            return Ok(res);
        }
        
        [HttpGet("CheckOut")]
        public async Task<IActionResult> CheckOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await userService.CheckOut(userId);
            unitOfWork.complete();
            return Ok(res);
        }
        
        [HttpGet("ViewOrders")]
        public async Task<IActionResult> ViewOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await userService.ViewOrders(userId);
           
            return Ok(res);
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder([FromBody] string orderNumber)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await userService.CancelOrder(userId,orderNumber);
            unitOfWork.complete();
            return Ok(res);
        }
        [HttpPost("TrackOrder")]
        public async Task<IActionResult> TrackOrder( [FromBody] string orderNumber)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await userService.TrackOrder(userId, orderNumber);
            unitOfWork.complete();
            return Ok(res);
        }

        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await userService.GetNotifications(userId);
            if (res == null)
                return BadRequest("You have no notifications");
            return Ok(res);
        }   

        [HttpPost("AddReview/{productId}")]
        public async Task<IActionResult> AddReview( int productId , [FromBody] string review )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await userService.AddReview(userId,productId,review);
            unitOfWork.complete();
            return Ok(res);

        }

        [HttpPost("SellProduct")]
        public async Task<IActionResult> SellProduct(ProductCreationDTO product)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await userService.SellProduct(userId, product);
            unitOfWork.complete();
            return Ok("Done");
        }

        [HttpGet("ShowMyProducts")]
        public async Task<IActionResult> ShowMyProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await userService.ViewMyProducts(userId);
            if (res == null) return BadRequest("You Have not any products on market");
            return Ok(res);
        }

    }
}
