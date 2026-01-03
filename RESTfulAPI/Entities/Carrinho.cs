using RESTfulAPI.Data;

namespace RESTfulAPI.Entities
{
    public class Carrinho
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }

        public string ClienteId { get; set; } = string.Empty;
        public ApplicationUser? Cliente { get; set; }

        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }
    }
}
