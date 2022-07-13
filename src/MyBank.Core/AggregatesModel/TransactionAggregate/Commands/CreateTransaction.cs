using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MyBank.Core;
using MyBank.Core.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyBank.Core
{

    public class CreateTransactionValidator: AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionValidator()
        {
            RuleFor(request => request.Transaction).NotNull();
            RuleFor(request => request.Transaction).SetValidator(new TransactionValidator());
        }
    
    }
    public class CreateTransactionRequest: IRequest<CreateTransactionResponse>
    {
        public TransactionDto Transaction { get; set; }
    }
    public class CreateTransactionResponse: ResponseBase
    {
        public TransactionDto Transaction { get; set; }
    }
    public class CreateTransactionHandler: IRequestHandler<CreateTransactionRequest, CreateTransactionResponse>
    {
        private readonly IMyBankDbContext _context;
        private readonly ILogger<CreateTransactionHandler> _logger;
    
        public CreateTransactionHandler(IMyBankDbContext context, ILogger<CreateTransactionHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    
        public async Task<CreateTransactionResponse> Handle(CreateTransactionRequest request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction();
            
            _context.Transactions.Add(transaction);
            
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
