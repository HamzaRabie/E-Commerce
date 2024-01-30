using E_Commerce.Model;

namespace E_Commerce.DTOS
{
    public class CartViewDTO
    {
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public  string ProductDiscription { get; set; }
        public int Quantity { get; set; }
    }
}
