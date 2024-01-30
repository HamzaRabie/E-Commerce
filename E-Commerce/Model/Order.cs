using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Model
{
    public class Order
    {
        public int Id { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<int> Quantities { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } 
        /*
        {
            get { return Status; }
            set
            {
                if (DateTime.Now >= Date.AddDays(9)) Status = "Delivered";
                else if (DateTime.Now >= Date.AddDays(6)) Status = "Shipped";
                else if (DateTime.Now >= Date.AddDays(3)) Status = "Processing";
                else Status = "Pending";
            }
        }*/
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public  double Price { get; set; }
        public string OrderNumber { get; set; }
        public bool IsDelivered => DateTime.Now >= Date.AddDays(9);
    }
}
