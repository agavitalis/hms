namespace HMS.ViewModels
{
    public class JwtOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public int ExpiryMinutes { get; set; }
        public bool ValidateLifetime { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public string Audience { get; set; }
    }
}
