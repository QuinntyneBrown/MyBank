using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBank.Core;
using MyBank.Core.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyBank.Core
{

    public class RemoveTransactionRequest: IRequest<RemoveTransactionResponse>
    {
        public Guid TransactionId { get; set; }
    }
    public class RemoveTransactionResponse: ResponseBase
    {
        public TransactionDto Transaction { get; set; }
    }
    public class RemoveTransactionHandler: IRequestHandler<RemoveTransactionRequest, RemoveTransactionResponse>
    {
        private readonly IMyBankDbContext _context;
        private readonly ILogger<RemoveTransactionHandler> _logger;
    
        public RemoveTransactionHandler(IMyBankDbContext context, ILogger<RemoveTransactionHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    
        public async Task<RemoveTransactionResponse> Handle(RemoveTransactionRequest request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions.FindAsync(request.TransactionId);
            
            _context.Transactions.Remove(transaction);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return new ()
            {
                Transaction = transaction.ToDto()
            };
        }
        
    }

}
