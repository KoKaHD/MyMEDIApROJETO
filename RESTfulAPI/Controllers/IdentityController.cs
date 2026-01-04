using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RESTfulAPI.Data;
using RESTfulAPI.Data.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RESTfulAPI.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;

    public IdentityController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
    }


    [Authorize]
    [HttpGet("teste-auth")]
    public IActionResult TesteAuth() => Ok("OK autenticado");
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
            return BadRequest("Email já existe.");

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,

            Nome = dto.Nome,
            Apelido = dto.Apelido,
            DataNascimento = dto.DataNascimento,
            NIF = dto.NIF,

            Estado = "Pendente"
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // Roles (opcional)
        var role = dto.Role is "Fornecedor" ? "Fornecedor" : "Cliente";

        if (!await _roleManager.RoleExistsAsync(role))
            await _roleManager.CreateAsync(new IdentityRole(role));

        await _userManager.AddToRoleAsync(user, role);

        // opcional: estado pendente/ativo pode ser uma Claim ou campo no user
        // await _userManager.AddClaimAsync(user, new Claim("Estado", "Pendente"));

        return Ok("Registo criado.");
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized("Credenciais inválidas.");

        var ok = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!ok)
            return Unauthorized("Credenciais inválidas.");

        if (user.Estado != "Ativo")
            return StatusCode(StatusCodes.Status403Forbidden,
                "Conta pendente. Aguarde ativação por um funcionário/administrador.");

        var token = await GenerateJwtAsync(user);
        return Ok(token);
    }

    private async Task<AuthResponseDto> GenerateJwtAsync(ApplicationUser user)
    {
        var key = _config["JWT:Key"] ?? throw new Exception("JWT:Key em falta");
        var issuer = _config["JWT:Issuer"] ?? throw new Exception("JWT:Issuer em falta");
        var audience = _config["JWT:Audience"] ?? throw new Exception("JWT:Audience em falta");

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? user.Email ?? user.Id)
        };

        // roles no token
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddHours(2);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            ExpiresAtUtc = expires
        };
    }
}