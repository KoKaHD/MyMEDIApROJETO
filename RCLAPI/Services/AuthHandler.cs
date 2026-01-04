using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace RCLAPI.Services
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;

        public AuthHandler(TokenService tokenService) => _tokenService = tokenService;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? token = null;

            try
            {
                token = await _tokenService.GetTokenAsync();
            }
            catch (InvalidOperationException)
            {
                // SSR prerender: não há JS/localStorage ainda
                token = null;
            }
            catch (JSDisconnectedException)
            {
                // circuito caiu/desligado
                token = null;
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}