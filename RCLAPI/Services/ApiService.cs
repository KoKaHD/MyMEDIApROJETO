using RCLAPI.DTOs;
using System.Net.Http.Json;

namespace RCLAPI.Services
{
    public class ApiService
    {
        private readonly HttpClient _public;
        private readonly HttpClient _auth;

        public ApiService(IHttpClientFactory factory)
        {
            _public = factory.CreateClient("PublicApi");
            _auth = factory.CreateClient("AuthApi");
        }

        // PUBLICO
        public async Task<List<CategoriaDTO>> GetCategoriasAsync()
        {
            var resp = await _public.GetAsync("api/categorias");

            // Ajuda a perceber rapidamente o que veio
            var body = await resp.Content.ReadAsStringAsync();

            if (resp.StatusCode == System.Net.HttpStatusCode.NoContent || string.IsNullOrWhiteSpace(body))
                return new List<CategoriaDTO>();

            resp.EnsureSuccessStatusCode();

            return System.Text.Json.JsonSerializer.Deserialize<List<CategoriaDTO>>(
                body,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<CategoriaDTO>();
        }

        public async Task<List<ProdutoDTO>?> GetProdutosAsync() =>
            await _public.GetFromJsonAsync<List<ProdutoDTO>>("api/produtos");

        public async Task<ProdutoDTO?> GetProdutoAsync(int id) =>
            await _public.GetFromJsonAsync<ProdutoDTO>($"api/produtos/{id}");

        public async Task<List<ProdutoDTO>?> GetProdutosPorCategoriaAsync(int categoriaId) =>
            await _public.GetFromJsonAsync<List<ProdutoDTO>>($"api/produtos/categoria/{categoriaId}");

        // AUTH (apenas quando fizer sentido exigir login)
        public async Task<List<CarrinhoDTO>?> GetCarrinhoAsync(string clienteId) =>
            await _auth.GetFromJsonAsync<List<CarrinhoDTO>>($"api/carrinho/{clienteId}");

        public async Task AddCarrinhoAsync(CarrinhoDTO item) =>
            await _auth.PostAsJsonAsync("api/carrinho", item);

        public async Task<List<FavoritoDTO>?> GetFavoritosAsync(string clienteId) =>
            await _auth.GetFromJsonAsync<List<FavoritoDTO>>($"api/favoritos/{clienteId}");

        public async Task AddFavoritoAsync(FavoritoDTO fav) =>
            await _auth.PostAsJsonAsync("api/favoritos", fav);

        public async Task<List<EncomendaDTO>?> GetEncomendasAsync(string clienteId) =>
            await _auth.GetFromJsonAsync<List<EncomendaDTO>>($"api/encomendas/{clienteId}");

        public async Task FinalizarEncomendaAsync(EncomendaDTO dto) =>
            await _auth.PostAsJsonAsync("api/encomendas", dto);

        // Login/Registo devem ser públicos
        /* public async Task<AuthResponseDto?> LoginAsync(LoginDTO dto)
         {
             using var req = new HttpRequestMessage(HttpMethod.Post, "api/identity/login")
             {
                 Content = JsonContent.Create(dto)
             };

             req.Headers.Accept.Clear();
             req.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

             var rsp = await _public.SendAsync(req);
             if (!rsp.IsSuccessStatusCode) return null;

             return await rsp.Content.ReadFromJsonAsync<AuthResponseDto>();
         }*/
        public async Task<HttpResponseMessage> LoginAsync(LoginDTO dto) =>
     await _public.PostAsJsonAsync("api/identity/login", dto);
        public async Task<HttpResponseMessage> RegistoAsync(RegistoDTO dto) =>
            await _public.PostAsJsonAsync("api/identity/register", dto);

        public async Task RemoveCarrinhoAsync(int id) =>
            await _auth.DeleteAsync($"api/carrinho/{id}");

        public async Task UpdateCarrinhoAsync(CarrinhoDTO item) =>
            await _auth.PutAsJsonAsync($"api/carrinho/{item.Id}", item);

        public async Task ClearCarrinhoAsync(string clienteId) =>
            await _auth.DeleteAsync($"api/carrinho/limpar/{clienteId}");

        public async Task RemoveFavoritoAsync(int id) =>
            await _auth.DeleteAsync($"api/favoritos/{id}");
    }
}