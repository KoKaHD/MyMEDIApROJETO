namespace RESTfulAPI.Data.DTOs
{
    public class EncomendaDTO
    {
        public int Id { get; set; }
        public DateTime DataEncomenda { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal PrecoTotal { get; set; }

        public string ClienteId { get; set; } = string.Empty;
        public List<DetalheEncomendaDTO> Detalhes { get; set; } = new();
    }

    public class DetalheEncomendaDTO
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
    }
}