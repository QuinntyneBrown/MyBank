using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace MyBank.Core.Interfaces
{
    public interface IMyBankDbContext
    {
        DbSet<Transaction> Transactions { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
    }
}
