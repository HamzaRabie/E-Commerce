using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Model
{
    [Owned]
    public class RefreshToken
    {
        public  string Token { get; set; }
        public  DateTime CreatedOn { get; set; }
        public  DateTime ExpiresOn { get; set; }
        public bool IsExpired => ExpiresOn <= DateTime.UtcNow;
        public DateTime ?RevokedON { get; set; }
        public bool IsActive => !IsExpired && RevokedON==null;
    }
}
