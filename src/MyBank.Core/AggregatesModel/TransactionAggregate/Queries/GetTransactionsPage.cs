using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBank.Core;
using MyBank.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyBank.Core
{

    public class GetTransactionsPageRequest: IRequest<GetTransactionsPageResponse>
    {
        public int PageSize { get; set; }
        public int Index { get; set; }
    }
    public class GetTransactionsPageResponse: ResponseBase
    {
        public int Length { get; set; }
        public List<TransactionDto> Entities { get; set; }
    }
    public class GetTransactionsPageHandler: IRequestHandler<GetTransactionsPageRequest, GetTransactionsPageResponse>
    {
        private readonly IMyBankDbContext _context;
        private readonly ILogger<GetTransactionsPageHandler> _logger;
    
        public GetTransactionsPageHandler(IMyBankDbContext context, ILogger<GetTransactionsPageHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    
        public async Task<GetTransactionsPageResponse> Handle(GetTransactionsPageRequest request, CancellationToken cancellationToken)
        {
            var query = from transaction in _context.Transactions
                select transaction;
            
            var length = await _context.Transactions.AsNoTracking().CountAsync();
            
            var transactions = await query.Page(request.Index, request.PageSize).AsNoTracking()
                .Select(x => x.ToDto()).ToListAsync();
            
            return new ()
            {
                Length = length,
                Entities = transactions
            };
        }
        
    }

}
