namespace RCLAPI.DTOs
{
    public class RegistoDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // NOVO: "Cliente" | "Fornecedor"
        public string Role { get; set; } = "Cliente";

        public string Nome { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public int? NIF { get; set; }
    }
}