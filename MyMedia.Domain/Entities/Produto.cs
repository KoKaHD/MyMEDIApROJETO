using MyMedia.Domain;

namespace MyMedia.Domain.Entities;

public class Produto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Detalhe { get; set; } = string.Empty;
    public string Origem { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;

    public string? UrlImagem { get; set; }
    public byte[]? Imagem { get; set; }

    // Enunciado: fornecedor define preço base
    public decimal PrecoBase { get; set; }

    // Enunciado: admin/func define percentagem na validação
    public decimal PercentagemPlataforma { get; set; } // ex: 10 = 10%

    // Preço final mostrado ao cliente
    public decimal PrecoFinal { get; set; }

    // Estado do registo (pendente até validação)
    public EstadoProduto Estado { get; set; } = EstadoProduto.Pendente;

    public bool Promocao { get; set; }
    public bool MaisVendido { get; set; }

    // Sugestão: stock é quantidade → normalmente int
    public int EmStock { get; set; }

    // Se queres suportar “apenas listar” vs “para venda”
    public bool ParaVenda { get; set; } = true;
    public bool ParaListagem { get; set; } = true;

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