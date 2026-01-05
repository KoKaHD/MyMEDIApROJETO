using Microsoft.AspNetCore.Identity;
using MyMedia.Domain;

namespace MyMEDIA.GestaoLoja.Data;

public static class SeedData
{
    private static readonly string[] Roles =
    [
        "Administrador",
        "Funcionario",
        "Cliente",
        "Fornecedor"
    ];

    public static async Task EnsureSeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Roles
        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
        // Fornecedor Interno (para produtos criados no backoffice)
        var fornecedorEmail = "fornecedor@interno.pt";
        var fornecedorPassword = "Fornecedor123!";

        var fornecedor = await userManager.FindByEmailAsync(fornecedorEmail);
        if (fornecedor is null)
        {
            fornecedor = new ApplicationUser
            {
                UserName = fornecedorEmail,
                Email = fornecedorEmail,
                Nome = "Fornecedor",
                Apelido = "Interno",
                Estado = "Ativo",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(fornecedor, fornecedorPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException("Erro a criar FornecedorInterno seed: " + errors);
            }
        }

        if (!await userManager.IsInRoleAsync(fornecedor, "Fornecedor"))
            await userManager.AddToRoleAsync(fornecedor, "Fornecedor");
        // Admin inicial
        var adminEmail = "admin@mymedia.pt";
        var adminPassword = "Admin123!";

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Nome = "Admin",
                Apelido = "MyMEDIA",
                Estado = "Ativo",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException("Erro a criar Admin seed: " + errors);
            }
        }

        if (!await userManager.IsInRoleAsync(admin, "Administrador"))
            await userManager.AddToRoleAsync(admin, "Administrador");
    }
}