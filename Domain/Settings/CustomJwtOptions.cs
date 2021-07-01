namespace Domain.Settings
{
    public class CustomJwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int ExpiredTime { get; set; }
        public int RefreshTokenExpiredDaysTime { get; set; }
    }
}
