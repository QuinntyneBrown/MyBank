using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace MyBank.Core
{
    public static class TransactionExtensions
    {
        public static TransactionDto ToDto(this Transaction transaction)
        {
            return new ()
            {
                TransactionId = transaction.TransactionId,
                Name = transaction.Name,
                Amount = transaction.Amount,
            };
        }
        
        public static async Task<List<TransactionDto>> ToDtosAsync(this IQueryable<Transaction> transactions, CancellationToken cancellationToken)
        {
            return await transactions.Select(x => x.ToDto()).ToListAsync(cancellationToken);
        }
        
        public static List<TransactionDto> ToDtos(this IEnumerable<Transaction> transactions)
        {
            return transactions.Select(x => x.ToDto()).ToList();
        }
        
    }
}
