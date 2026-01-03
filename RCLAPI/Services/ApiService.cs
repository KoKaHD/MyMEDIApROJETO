using RCLAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RCLAPI.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService(HttpClient http) => _http = http;

        // Categorias
        public async Task<List<CategoriaDTO>?> GetCategoriasAsync() =>
            await _http.GetFromJsonAsync<List<CategoriaDTO>>("api/categorias");

        // Produtos
        public async Task<List<ProdutoDTO>?> GetProdutosAsync() =>
            await _http.GetFromJsonAsync<List<ProdutoDTO>>("api/produtos");

        public async Task<ProdutoDTO?> GetProdutoAsync(int id) =>
            await _http.GetFromJsonAsync<ProdutoDTO>($"api/produtos/{id}");

        // Carrinho
        public async Task<List<CarrinhoDTO>?> GetCarrinhoAsync(string clienteId) =>
            await _http.GetFromJsonAsync<List<CarrinhoDTO>>($"api/carrinho/{clienteId}");

        public async Task AddCarrinhoAsync(CarrinhoDTO item) =>
            await _http.PostAsJsonAsync("api/carrinho", item);

        // Favoritos
        public async Task<List<FavoritoDTO>?> GetFavoritosAsync(string clienteId) =>
            await _http.GetFromJsonAsync<List<FavoritoDTO>>($"api/favoritos/{clienteId}");

        public async Task AddFavoritoAsync(FavoritoDTO fav) =>
            await _http.PostAsJsonAsync("api/favoritos", fav);

        // Encomendas
        public async Task<List<EncomendaDTO>?> GetEncomendasAsync(string clienteId) =>
            await _http.GetFromJsonAsync<List<EncomendaDTO>>($"api/encomendas/{clienteId}");

        public async Task FinalizarEncomendaAsync(EncomendaDTO dto) =>
            await _http.PostAsJsonAsync("api/encomendas", dto);

        // Identity
        public async Task<HttpResponseMessage> LoginAsync(LoginDTO dto) =>
            await _http.PostAsJsonAsync("api/identity/login", dto);

        public async Task<HttpResponseMessage> RegistoAsync(RegistoDTO dto) =>
            await _http.PostAsJsonAsync("api/identity/register", dto);

        // Remover item do carrinho
        public async Task RemoveCarrinhoAsync(int id) =>
            await _http.DeleteAsync($"api/carrinho/{id}");

        // Atualizar quantidade
        public async Task UpdateCarrinhoAsync(CarrinhoDTO item) =>
            await _http.PutAsJsonAsync($"api/carrinho/{item.Id}", item);

        // Limpar carrinho cliente
        public async Task ClearCarrinhoAsync(string clienteId) =>
            await _http.DeleteAsync($"api/carrinho/limpar/{clienteId}");

        // Remover favorito
        public async Task RemoveFavoritoAsync(int id) =>
            await _http.DeleteAsync($"api/favoritos/{id}");

        // Produtos por categoria
        public async Task<List<ProdutoDTO>?> GetProdutosPorCategoriaAsync(int categoriaId) =>
            await _http.GetFromJsonAsync<List<ProdutoDTO>>($"api/produtos/categoria/{categoriaId}");
    }
}
