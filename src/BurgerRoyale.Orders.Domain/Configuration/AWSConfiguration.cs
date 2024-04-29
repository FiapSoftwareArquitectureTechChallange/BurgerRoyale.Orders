namespace BurgerRoyale.Orders.Domain.Configuration
{
    public class AWSConfiguration
    {
        public string? AccessKey { get; set; } = string.Empty;
        public string? SecretKey { get; set; } = string.Empty;
        public string? SessionToken { get; set; } = string.Empty;
        public string? Region { get; set; } = string.Empty;
    }
}
