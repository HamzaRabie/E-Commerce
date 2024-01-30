using E_Commerce.Model;
using E_Commerce.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public ProductController( IUnitOfWork UnitOfWork )
        {
            unitOfWork = UnitOfWork;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await unitOfWork.Products.GetAllAsync());
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return Ok(await unitOfWork.Products.GetByIdAsync(id));
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetProductByName( string name )
        {
            return Ok(await unitOfWork.Products.GetOneAsync( p=>p.Name == name ));
        }

        [HttpPost("AddProduct")] 
        public async Task<IActionResult> AddProduct(Product product)
        {
            if(!ModelState.IsValid)
            {
                 return BadRequest(ModelState);
            }
            var res = await unitOfWork.Products.AddAsync(product);
            if (res != null)
            {
                unitOfWork.complete();
                return Ok(res);
            }
            return BadRequest("IncorrectData");
            
        }

        [HttpPatch("UpdateProduct")] 
        public IActionResult UpdateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = unitOfWork.Products.Update(product);
            if (res != null) {
                unitOfWork.complete();
                return Ok();
            }
            return BadRequest("Invalid Data");
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var res = await unitOfWork.Products.DeleteAsync(id);
            if (res) {
                unitOfWork.complete();
                return Ok();
            }
            return BadRequest("Ivalid item");
        }


    }
}
