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

    public class UpdateTransactionValidator: AbstractValidator<UpdateTransactionRequest>
    {
        public UpdateTransactionValidator()
        {
            RuleFor(request => request.Transaction).NotNull();
            RuleFor(request => request.Transaction).SetValidator(new TransactionValidator());
        }
    
    }
    public class UpdateTransactionRequest: IRequest<UpdateTransactionResponse>
    {
        public TransactionDto Transaction { get; set; }
    }
    public class UpdateTransactionResponse: ResponseBase
    {
        public TransactionDto Transaction { get; set; }
    }
    public class UpdateTransactionHandler: IRequestHandler<UpdateTransactionRequest, UpdateTransactionResponse>
    {
        private readonly IMyBankDbContext _context;
        private readonly ILogger<UpdateTransactionHandler> _logger;
    
        public UpdateTransactionHandler(IMyBankDbContext context, ILogger<UpdateTransactionHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    
        public async Task<UpdateTransactionResponse> Handle(UpdateTransactionRequest request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions.SingleAsync(x => x.TransactionId == request.Transaction.TransactionId);
            
            transaction.Name = request.Transaction.Name;
            transaction.Amount = request.Transaction.Amount;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return new ()
            {
                Transaction = transaction.ToDto()
            };
        }
        
    }

}
