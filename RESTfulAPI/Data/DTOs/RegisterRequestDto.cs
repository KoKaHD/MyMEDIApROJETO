namespace RESTfulAPI.Data.DTOs
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string? Role { get; set; }

        public string Nome { get; set; } = "";
        public string Apelido { get; set; } = "";
        public DateTime? DataNascimento { get; set; }
        public int? NIF { get; set; }
    }
}