using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Commerce.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public  string Category { get; set; }
        public List<string>? Reviews { get; set; } = new List<string>();
        public double Price { get; set; }
        public  int Quantity { get; set; }
        public  Byte ?Image { get; set; }
        public  DateTime Creation_Date { get; set; }
        public bool Availability => Quantity > 0;
        public double ?Rating { get; set; } = 0;
        [JsonIgnore]
        public List<Order> ?Orders { get; set; }
        [ForeignKey("Seller")]
        [JsonIgnore]
        public string? UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? Seller { get; set; }


    }

}
