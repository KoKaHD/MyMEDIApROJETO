using RESTfulAPI.Data;

namespace RESTfulAPI.Entities
{
    public class Encomenda
    {
        public int Id { get; set; }
        public DateTime DataEncomenda { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Pendente"; // Pendente, Confirmada, Rejeitada, Expedida
        public decimal PrecoTotal { get; set; }

        public string ClienteId { get; set; } = string.Empty;
        public ApplicationUser? Cliente { get; set; }

        public ICollection<DetalheEncomenda> DetalhesEncomenda { get; set; } = new List<DetalheEncomenda>();
    }
}
