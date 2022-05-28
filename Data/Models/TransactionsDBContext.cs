using Microsoft.EntityFrameworkCore;

namespace Data.Models
{
    public class TransactionsDBContext : DbContext
    {
        public TransactionsDBContext(DbContextOptions<TransactionsDBContext> context)
        : base(context)
        { }

       
        public DbSet<tbTransaction> TbTransactions { get; set; }
       
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
        }
    }
}
