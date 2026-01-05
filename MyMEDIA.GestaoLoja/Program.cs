using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMedia.Domain;
using MyMEDIA.GestaoLoja.Components;
using MyMEDIA.GestaoLoja.Components.Account;
using MyMEDIA.GestaoLoja.Services;

var builder = WebApplication.CreateBuilder(args);

// Razor Components (Blazor Server)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Auth state for Blazor
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<ProdutoBackofficeService>();
builder.Services.AddScoped<VendasBackofficeService>();
builder.Services.AddScoped<UserBackofficeService>();
builder.Services.AddScoped<CategoriaBackofficeService>();
builder.Services.AddScoped<ModoEntregaBackofficeService>();
// Authentication cookies (Identity)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

builder.Services.AddAuthorization();

// DbContext (do Domain) — mesma BD da API
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity (do Domain) + Roles
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    // IMPORTANTE: não uses migrations aqui se as migrations forem “controladas” pela API
    // app.UseMigrationsEndPoint();  // <- deixa comentado/removido
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// endpoints do Identity (/Account/...)
app.MapAdditionalIdentityEndpoints();
await MyMEDIA.GestaoLoja.Data.SeedData.EnsureSeedAsync(app.Services);
app.Run();