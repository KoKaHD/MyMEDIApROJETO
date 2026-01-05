using MyMedia.Domain;

namespace MyMedia.Domain.Entities
{
    public class Favorito
    {
        public int Id { get; set; }

        public string ClienteId { get; set; } = string.Empty;
        public ApplicationUser? Cliente { get; set; }

        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }
    }
}
