using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.JSInterop;

namespace RCLAPI.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _js;
        public TokenService(IJSRuntime js) => _js = js;

        public async Task<string?> GetTokenAsync() =>
            await _js.InvokeAsync<string?>("localStorage.getItem", "token");

        public async Task SetTokenAsync(string token) =>
            await _js.InvokeVoidAsync("localStorage.setItem", "token", token);

        public async Task RemoveTokenAsync() =>
            await _js.InvokeVoidAsync("localStorage.removeItem", "token");

        public async Task<string?> GetUserIdAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token)) return null;

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c =>
                       c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")
                      ?.Value;
        }
    }
}