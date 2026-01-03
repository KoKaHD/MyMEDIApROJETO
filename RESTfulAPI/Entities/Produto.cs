using RESTfulAPI.Data;

namespace RESTfulAPI.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Detalhe { get; set; } = string.Empty;
        public string Origem { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? UrlImagem { get; set; }
        public byte[]? Imagem { get; set; }
        public decimal Preco { get; set; }
        public bool Promocao { get; set; }
        public bool MaisVendido { get; set; }
        public decimal EmStock { get; set; }
        public bool Disponivel { get; set; }

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public int ModoEntregaId { get; set; }
        public ModoEntrega? ModoEntrega { get; set; }

        public string FornecedorId { get; set; } = string.Empty;
        public ApplicationUser? Fornecedor { get; set; }

        public ICollection<Carrinho> Carrinhos { get; set; } = new List<Carrinho>();
        public ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
        public ICollection<DetalheEncomenda> DetalhesEncomenda { get; set; } = new List<DetalheEncomenda>();
    }
}
