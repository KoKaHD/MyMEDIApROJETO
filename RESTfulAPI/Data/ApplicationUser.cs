using Microsoft.AspNetCore.Identity;

namespace RESTfulAPI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public int? NIF { get; set; }
        public string Estado { get; set; } = "Pendente";
    }
}
