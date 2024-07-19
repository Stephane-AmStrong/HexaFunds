using DataTransfertObjects;

using Microsoft.AspNetCore.Mvc;

using Services.Abstractions;

namespace WebApplicationDocker.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionsController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<TransactionResponse>> Get()
    {
        var transactionAccountsResponse = serviceManager.TransactionService.GetAll();

        return Ok(transactionAccountsResponse);
    }

    [HttpGet("statement")]
    public async Task<IActionResult> GetAccountStatement([FromQuery] AccountStatementQuery accountStatementQuery, CancellationToken cancellationToken)
    {
        var accountStatementResponse = await serviceManager.TransactionService.GetAccountStatementAsync(accountStatementQuery, cancellationToken);

        return Ok(accountStatementResponse);
    }

    [HttpGet("account/{accountId:guid}")]
    public async Task<ActionResult<AccountTransactionsResponse>> GetByAccountId(Guid accountId, CancellationToken cancellationToken)
    {
        var transactionAccountsResponse = await serviceManager.TransactionService.GetByAccountIdAsync(accountId, cancellationToken);

        return Ok(transactionAccountsResponse);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionResponse?>> Get(Guid id, CancellationToken cancellationToken)
    {
        var transactionAccountResponse = await serviceManager.TransactionService.GetByIdAsync(id, cancellationToken);

        return Ok(transactionAccountResponse);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionResponse?>> Post([FromBody] TransactionRequest transactionAccountRequest, CancellationToken cancellationToken)
    {
        var response = await serviceManager.TransactionService.CreateAsync(transactionAccountRequest, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }
}
