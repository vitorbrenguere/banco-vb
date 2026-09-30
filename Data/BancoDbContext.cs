using banco_vb.Models;
using Microsoft.EntityFrameworkCore;

namespace banco_vb.Data;

public class BancoDbContext : DbContext
{
    public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options)
    {
    }

    // Esta propriedade vai virar a tabela "Contas" no SQLite
    public DbSet<Conta> Contas { get; set; }
}