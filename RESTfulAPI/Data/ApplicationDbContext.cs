using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Data;
using RESTfulAPI.Entities;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<ModoEntrega> ModosEntrega { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Carrinho> Carrinhos { get; set; }
    public DbSet<Favorito> Favoritos { get; set; }
    public DbSet<Encomenda> Encomendas { get; set; }
    public DbSet<DetalheEncomenda> DetalhesEncomenda { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Relações
        builder.Entity<Produto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Produtos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Produto>()
            .HasOne(p => p.ModoEntrega)
            .WithMany(m => m.Produtos)
            .HasForeignKey(p => p.ModoEntregaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Produto>()
            .HasOne(p => p.Fornecedor)
            .WithMany()
            .HasForeignKey(p => p.FornecedorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Carrinho>()
            .HasOne(c => c.Cliente)
            .WithMany()
            .HasForeignKey(c => c.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Carrinho>()
            .HasOne(c => c.Produto)
            .WithMany(p => p.Carrinhos)
            .HasForeignKey(c => c.ProdutoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Favorito>()
            .HasOne(f => f.Cliente)
            .WithMany()
            .HasForeignKey(f => f.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Favorito>()
            .HasOne(f => f.Produto)
            .WithMany(p => p.Favoritos)
            .HasForeignKey(f => f.ProdutoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<DetalheEncomenda>()
            .HasOne(d => d.Encomenda)
            .WithMany(e => e.DetalhesEncomenda)
            .HasForeignKey(d => d.EncomendaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<DetalheEncomenda>()
            .HasOne(d => d.Produto)
            .WithMany(p => p.DetalhesEncomenda)
            .HasForeignKey(d => d.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}