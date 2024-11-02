// Program.cs
using Microsoft.EntityFrameworkCore;
using QBankApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o DbContext com a string de conexão para MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
    
//Adiciona os serviços de controlador(Registra os controllers para que o ASP.NET 
//Core possa localizar e configurar automaticamente as rotas.)

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();// Configura o roteamento e mapeia as rotas definidas nos controllers, sem necessidade do Swagger.
});


// Demais configurações...
app.Run();