using Microsoft.EntityFrameworkCore;
using QBankApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// o Application Insights
builder.Services.AddApplicationInsightsTelemetry();

// Adiciona configuração do appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configuração para acessar o Azure Key Vault
string keyVaultUrl = "https://qbankchave.vault.azure.net/"; // URL do Key Vault
string connectionString;

try
{
    // Tenta buscar a string de conexão no Key Vault
    var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
    KeyVaultSecret connectionStringSecret = client.GetSecret("ChaveQBank");
    connectionString = connectionStringSecret.Value;
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao acessar o Key Vault: {ex.Message}");
    throw new InvalidOperationException("Não foi possível obter a string de conexão do Key Vault.");
}

// Configuração do DbContext com a string de conexão
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configura autenticação com JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Validações e configuração de JWT
    string jwtKey = builder.Configuration["Jwt:Key"];
    string jwtIssuer = builder.Configuration["Jwt:Issuer"];
    string jwtAudience = builder.Configuration["Jwt:Audience"];

    if (string.IsNullOrWhiteSpace(jwtKey))
        throw new InvalidOperationException("A chave JWT ('Jwt:Key') não está configurada.");
    if (string.IsNullOrWhiteSpace(jwtIssuer))
        throw new InvalidOperationException("O emissor JWT ('Jwt:Issuer') não está configurado.");
    if (string.IsNullOrWhiteSpace(jwtAudience))
        throw new InvalidOperationException("A audiência JWT ('Jwt:Audience') não está configurada.");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite qualquer origem
              .AllowAnyMethod()  // Permite qualquer método HTTP (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader(); // Permite qualquer cabeçalho
    });
});

// Adiciona os serviços de controlador
builder.Services.AddControllers();

var app = builder.Build();

// Configurações do ambiente
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}


// Configuração de ambiente
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Aplica a política de CORS antes do roteamento e autenticação
app.UseCors("AllowAll");

// Middleware de roteamento
app.UseRouting();

// Middleware para redirecionar todas as requisições HTTP para HTTPS
app.UseHttpsRedirection();

// Middleware de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Mapeia os endpoints dos controllers
app.MapControllers();

// Executa o aplicativo
app.Run();
