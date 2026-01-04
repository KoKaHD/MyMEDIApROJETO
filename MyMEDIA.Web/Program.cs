using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using MyMEDIA.Web.Components;
using RCLAPI.Extensions;
using RCLAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Serviços Server-Side Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

/*// HttpClient para API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!)
});*/

// Serviços RCL
builder.Services.AddRCLApiServices(builder.Configuration["ApiBaseUrl"]!);
/*builder.Services.AddScoped<TokenService>();*/

var app = builder.Build();

// Pipeline
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();