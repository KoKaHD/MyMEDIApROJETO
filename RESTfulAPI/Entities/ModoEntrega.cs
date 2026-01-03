namespace RESTfulAPI.Entities
{
    public class ModoEntrega
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty; // ex: "Entrega em casa", "Levantamento na loja"
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
