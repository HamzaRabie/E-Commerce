using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Model
{
    public class Notification 
    {
        public  int  Id { get; set; }
        public  string notification { get; set; }
        public  DateTime Date { get; set; }
        [ForeignKey("User")]
        public string userId { get; set; }
        public  ApplicationUser User  { get; set; }
    }
}
