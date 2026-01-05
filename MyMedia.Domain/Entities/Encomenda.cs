namespace MyMedia.Domain.Entities
{
    public class Encomenda
    {
        public int Id { get; set; }
        public DateTime DataEncomenda { get; set; } = DateTime.Now;

        public string Estado { get; set; } = "Pendente"; // Pendente, Confirmada, Rejeitada, Expedida
        public decimal PrecoTotal { get; set; }

        // Pagamento (simulado)
        public string EstadoPagamento { get; set; } = "Pendente"; // Pendente, Pago, Rejeitado
        public DateTime? DataPagamento { get; set; }

        // Expedição (simulada)
        public DateTime? DataExpedicao { get; set; }

        public string ClienteId { get; set; } = string.Empty;
        public ApplicationUser? Cliente { get; set; }

        public ICollection<DetalheEncomenda> DetalhesEncomenda { get; set; } = new List<DetalheEncomenda>();
    }
}