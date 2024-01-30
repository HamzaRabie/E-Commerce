using E_Commerce.Model;

namespace E_Commerce.DTOS
{
    public class OrderDTO
    {
        public  List<ProductOrderDTO> Products { get; set; }
        public double OerderPrice { get; set; }
        public string ProductStatus  { get; set; }
        public  DateTime OerderDate { get; set; }
        public string OrderNumber { get; set; }
    }
}
