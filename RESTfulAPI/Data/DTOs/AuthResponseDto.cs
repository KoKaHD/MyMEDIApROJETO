namespace RESTfulAPI.Data.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = "";
        public DateTime ExpiresAtUtc { get; set; }
    }
}
