using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Model
{
    public class UserCart
    {
        public  int Id { get; set; }
        public  Product Product { get; set; }
        public  int  Qunatity { get; set; }
        public  DateTime Date { get; set; }
        public  ApplicationUser User { get; set; }
    }
}
