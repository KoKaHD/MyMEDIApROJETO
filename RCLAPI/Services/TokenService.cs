using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}