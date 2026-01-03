namespace RESTfulAPI.Data.DTOs
{
    
        public class ProdutoDTO
        {
            public int Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Detalhe { get; set; } = string.Empty;
            public string Origem { get; set; } = string.Empty;
            public string Titulo { get; set; } = string.Empty;
            public string? UrlImagem { get; set; }
            public decimal Preco { get; set; }
            public bool Promocao { get; set; }
            public bool MaisVendido { get; set; }
            public decimal EmStock { get; set; }
            public bool Disponivel { get; set; }

            public int CategoriaId { get; set; }
            public string CategoriaNome { get; set; } = string.Empty;

            public int ModoEntregaId { get; set; }
            public string ModoEntregaNome { get; set; } = string.Empty;

            public string FornecedorId { get; set; } = string.Empty;
            public string FornecedorNome { get; set; } = string.Empty;
        
    }
}
