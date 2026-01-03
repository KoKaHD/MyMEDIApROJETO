namespace RESTfulAPI.Data.DTOs
{
    public class FavoritoDTO
    {
        public int Id { get; set; }

        public string ClienteId { get; set; } = string.Empty;

        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public decimal ProdutoPreco { get; set; }
        public string? ProdutoUrlImagem { get; set; }
    }
}