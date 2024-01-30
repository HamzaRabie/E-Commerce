using E_Commerce.Model;
using System.Text.Json.Serialization;

namespace E_Commerce.DTOS
{
    public class ProductViewDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<string> Reviews { get; set; }
        public double Price { get; set; }
        [JsonIgnore]
        public int Quantity { get; set; }
        // to do (seller as appUser 
        public Byte? Image { get; set; }
        public bool Availability => Quantity > 0;
        public double? Rating { get; set; } = 0;
    }
}
