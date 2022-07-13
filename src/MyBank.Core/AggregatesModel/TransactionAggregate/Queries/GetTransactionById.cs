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

    public class GetTransactionByIdRequest: IRequest<GetTransactionByIdResponse>
    {
        public Guid TransactionId { get; set; }
    }
    public class GetTransactionByIdResponse: ResponseBase
    {
        public TransactionDto Transaction { get; set; }
    }
    public class GetTransactionByIdHandler: IRequestHandler<GetTransactionByIdRequest, GetTransactionByIdResponse>
    {
        private readonly IMyBankDbContext _context;
        private readonly ILogger<GetTransactionByIdHandler> _logger;
    
        public GetTransactionByIdHandler(IMyBankDbContext context, ILogger<GetTransactionByIdHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    
        public async Task<GetTransactionByIdResponse> Handle(GetTransactionByIdRequest request, CancellationToken cancellationToken)
        {
            return new () {
                Transaction = (await _context.Transactions.AsNoTracking().SingleOrDefaultAsync(x => x.TransactionId == request.TransactionId)).ToDto()
            };
        }
        
    }

}
