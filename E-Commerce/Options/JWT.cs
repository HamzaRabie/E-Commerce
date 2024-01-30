namespace E_Commerce.JWT_Options
{
    public class JWT
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public Double DurationInMinutes { get; set; }

    }
}
