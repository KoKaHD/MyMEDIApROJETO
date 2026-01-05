using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMedia.Domain;

namespace MyMEDIA.GestaoLoja.Services;

public class UserBackofficeService
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IHttpContextAccessor _http;

    public UserBackofficeService(UserManager<ApplicationUser> userManager, IHttpContextAccessor http)
    {
        _userManager = userManager;
        _http = http;
    }
    public async Task<(List<ApplicationUser> Items, int Total)> SearchAsync(
        string? q, string? role, string? estado, int page, int pageSize)
    {
        var query = _userManager.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(u =>
                (u.Email != null && u.Email.Contains(q)) ||
                u.UserName!.Contains(q) ||
                u.Nome.Contains(q) ||
                u.Apelido.Contains(q));
        }

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(u => u.Estado == estado);

        // filtro por role (precisa de join às tabelas Identity)
        if (!string.IsNullOrWhiteSpace(role))
        {
            // forma simples: filtrar por IDs via UserManager (mais lento mas ok para demo)
            var all = await query.ToListAsync();
            var filtered = new List<ApplicationUser>();

            foreach (var u in all)
            {
                var roles = await _userManager.GetRolesAsync(u);
                if (roles.Contains(role))
                    filtered.Add(u);
            }

            var total2 = filtered.Count;
            var items2 = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (items2, total2);
        }

        var total = await query.CountAsync();
        var items = await query.OrderBy(u => u.Email).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, total);
    }

    public Task<IList<string>> GetRolesAsync(ApplicationUser user) => _userManager.GetRolesAsync(user);

    public async Task SetEstadoAsync(string userId, string novoEstado, string currentUserId)
    {
        if (userId == currentUserId)
            throw new InvalidOperationException("Não pode desativar/alterar o seu próprio utilizador.");

        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new InvalidOperationException("Utilizador não encontrado.");

        user.Estado = novoEstado;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
    }

    public async Task SetRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new InvalidOperationException("Utilizador não encontrado.");

        // Regra: um user pode ter várias roles, mas aqui vamos garantir 1 "principal"
        var roles = await _userManager.GetRolesAsync(user);

        // remove roles de negócio (mantém outras se existirem)
        var businessRoles = new[] { "Cliente", "Fornecedor", "Funcionario", "Administrador" };
        var toRemove = roles.Where(r => businessRoles.Contains(r)).ToList();
        if (role is "Funcionario" or "Administrador")
        {
            var isAdmin = _http.HttpContext?.User?.IsInRole("Administrador") ?? false;
            if (!isAdmin)
                throw new InvalidOperationException("Apenas Administrador pode atribuir Funcionário/Administrador.");
        }
        if (toRemove.Count > 0)
        {
            var rem = await _userManager.RemoveFromRolesAsync(user, toRemove);
            if (!rem.Succeeded)
                throw new InvalidOperationException(string.Join("; ", rem.Errors.Select(e => e.Description)));
        }

        var add = await _userManager.AddToRoleAsync(user, role);
        if (!add.Succeeded)
            throw new InvalidOperationException(string.Join("; ", add.Errors.Select(e => e.Description)));
    }
}