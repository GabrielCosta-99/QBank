using Microsoft.EntityFrameworkCore;
using QBankApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configuração para acessar o Azure Key Vault
string keyVaultUrl = "https://qbankchave.vault.azure.net/"; // URL do seu Key Vault
var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

// Buscando o segredo da string de conexão no Key Vault
KeyVaultSecret connectionStringSecret = client.GetSecret("ChaveQBank");
string connectionString = connectionStringSecret.Value;

// Adiciona o DbContext com a string de conexão para MySQL obtida do Key Vault
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, 
    ServerVersion.AutoDetect(connectionString)));

// Configura autenticação com JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Adiciona os serviços de controlador
builder.Services.AddControllers();

var app = builder.Build();

// Middleware de roteamento
app.UseRouting();

// Middleware de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Mapeia os endpoints dos controllers
app.MapControllers();

// Executa o aplicativo
app.Run();
