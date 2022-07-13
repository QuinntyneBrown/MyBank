using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MyBank.Core;
using MediatR;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace MyBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class TransactionController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(IMediator mediator, ILogger<TransactionController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [SwaggerOperation(
            Summary = "Get Transaction by id.",
            Description = @"Get Transaction by id."
        )]
        [HttpGet("{transactionId:guid}", Name = "getTransactionById")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GetTransactionByIdResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetTransactionByIdResponse>> GetById([FromRoute]Guid transactionId, CancellationToken cancellationToken)
        {
            var request = new GetTransactionByIdRequest() { TransactionId = transactionId };
        
            var response = await _mediator.Send(request, cancellationToken);
        
            if (response.Transaction == null)
            {
                return new NotFoundObjectResult(request.TransactionId);
            }
        
            return response;
        }
        
        [SwaggerOperation(
            Summary = "Get Transactions.",
            Description = @"Get Transactions."
        )]
        [HttpGet(Name = "getTransactions")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GetTransactionsResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetTransactionsResponse>> Get(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetTransactionsRequest(), cancellationToken);
        }
        
        [SwaggerOperation(
            Summary = "Create Transaction.",
            Description = @"Create Transaction."
        )]
        [HttpPost(Name = "createTransaction")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CreateTransactionResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CreateTransactionResponse>> Create([FromBody]CreateTransactionRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "----- Sending command: {CommandName}: ({@Command})",
                nameof(CreateTransactionRequest),
                request);
        
            return await _mediator.Send(request, cancellationToken);
        }
        
        [SwaggerOperation(
            Summary = "Get Transaction Page.",
            Description = @"Get Transaction Page."
        )]
        [HttpGet("page/{pageSize}/{index}", Name = "getTransactionsPage")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GetTransactionsPageResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetTransactionsPageResponse>> Page([FromRoute]int pageSize, [FromRoute]int index, CancellationToken cancellationToken)
        {
            var request = new GetTransactionsPageRequest { Index = index, PageSize = pageSize };
        
            return await _mediator.Send(request, cancellationToken);
        }
        
        [SwaggerOperation(
            Summary = "Update Transaction.",
            Description = @"Update Transaction."
        )]
        [HttpPut(Name = "updateTransaction")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UpdateTransactionResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateTransactionResponse>> Update([FromBody]UpdateTransactionRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                nameof(UpdateTransactionRequest),
                nameof(request.Transaction.TransactionId),
                request.Transaction.TransactionId,
                request);
        
            return await _mediator.Send(request, cancellationToken);
        }
        
        [SwaggerOperation(
            Summary = "Delete Transaction.",
            Description = @"Delete Transaction."
        )]
        [HttpDelete("{transactionId:guid}", Name = "removeTransaction")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RemoveTransactionResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<RemoveTransactionResponse>> Remove([FromRoute]Guid transactionId, CancellationToken cancellationToken)
        {
            var request = new RemoveTransactionRequest() { TransactionId = transactionId };
        
            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                nameof(RemoveTransactionRequest),
                nameof(request.TransactionId),
                request.TransactionId,
                request);
        
            return await _mediator.Send(request, cancellationToken);
        }
        
    }
}
