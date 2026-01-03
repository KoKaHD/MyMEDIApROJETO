using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLAPI.DTOs
{
    public class CarrinhoDTO
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public string ClienteId { get; set; } = string.Empty;
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public decimal ProdutoPreco { get; set; }
        public string? ProdutoUrlImagem { get; set; }
    }
}
