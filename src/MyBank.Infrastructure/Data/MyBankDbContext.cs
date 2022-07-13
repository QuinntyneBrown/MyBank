using MyBank.Core;
using MyBank.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace MyBank.Infrastructure.Data
{
    public class MyBankDbContext: DbContext, IMyBankDbContext
    {
        public DbSet<Transaction> Transactions { get; private set; }
        public MyBankDbContext(DbContextOptions options)
            :base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyBankDbContext).Assembly);
        }
        
    }
}
