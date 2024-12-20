// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using QBankApi.Models;

namespace QBankApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

           public DbSet<CadastroUsuario> CadastroUsuarios { get; set; }  
    }
}