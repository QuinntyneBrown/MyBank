using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using MyBank.Core;
using MyBank.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyBank.Core
{

    public class GetTransactionsRequest: IRequest<GetTransactionsResponse> { }
    public class GetTransactionsResponse: ResponseBase
    {
        public List<TransactionDto> Transactions { get; set; }
    }
    public class GetTransactionsHandler: IRequestHandler<GetTransactionsRequest, GetTransactionsResponse>
    {
        private readonly IMyBankDbContext _context;
        private readonly ILogger<GetTransactionsHandler> _logger;
    
        public GetTransactionsHandler(IMyBankDbContext context, ILogger<GetTransactionsHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    
        public async Task<GetTransactionsResponse> Handle(GetTransactionsRequest request, CancellationToken cancellationToken)
        {
            return new () {
                Transactions = await _context.Transactions.AsNoTracking().ToDtosAsync(cancellationToken)
            };
        }
        
    }

}
