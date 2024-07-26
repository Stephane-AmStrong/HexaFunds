using DataTransfertObjects;

using Microsoft.AspNetCore.Mvc;

using Services.Abstractions;

namespace WebApplicationDocker.Controllers;

[ApiController]
[Route("api/savingsaccounts")]
public class SavingsAccountsController(ISavingsAccountService savingsAccountService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<SavingsAccountResponse>> Get()
    {
        var savingsAccountsResponse = savingsAccountService.GetAll();

        return Ok(savingsAccountsResponse);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var savingsAccountResponse = await savingsAccountService.GetByIdAsync(id, cancellationToken);

        return Ok(savingsAccountResponse);
    }

    [HttpPost]
    public async Task<ActionResult<SavingsAccountResponse>> Post([FromBody] SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken)
    {
        var response = await savingsAccountService.CreateAsync(savingsAccountRequest, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await savingsAccountService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken)
    {
        await savingsAccountService.UpdateAsync(id, savingsAccountRequest, cancellationToken);

        return NoContent();
    }
}
