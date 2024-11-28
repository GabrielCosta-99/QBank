using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QBankApi.Data;
using QBankApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace QBankApi.Controllers  
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Método de teste
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Controller está funcionando!");
        }

        // Método de login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            // Verificar se as credenciais estão completas
            if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Senha))
            {
                return BadRequest("Email e senha são obrigatórios.");
            }

            // Procurar o usuário no banco de dados
            var user = await _context.Set<CadastroUsuario>()
                                      .FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Senha == loginRequest.Senha);

            if (user == null)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            // Validar e obter a chave JWT
            string jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("A chave JWT não está configurada.");
            var key = Encoding.UTF8.GetBytes(jwtKey);

            // Gerar Token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("UserId", user.Id.ToString()) // Opcional: adiciona o ID do usuário no token
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        // Classe auxiliar para os dados de login
        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Senha { get; set; } = string.Empty;
        }
    }
}
