using Microsoft.EntityFrameworkCore;


namespace Supply.Models
{
    public class SupplyDBContext : DbContext
    {
        public SupplyDBContext(DbContextOptions<SupplyDBContext> options) : base(options)
        {
        }

        public DbSet<Mercadoria> Mercadorias { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
    }
}
