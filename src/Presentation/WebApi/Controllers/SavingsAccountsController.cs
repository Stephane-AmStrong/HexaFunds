using DataTransfertObjects;

using Microsoft.AspNetCore.Mvc;

using Services.Abstractions;

namespace WebApplicationDocker.Controllers;

[ApiController]
[Route("api/savingsaccounts")]
public class SavingsAccountsController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<SavingsAccountResponse>> Get()
    {
        var savingsAccountsResponse = serviceManager.SavingsAccountService.GetAll();

        return Ok(savingsAccountsResponse);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var savingsAccountResponse = await serviceManager.SavingsAccountService.GetByIdAsync(id, cancellationToken);

        return Ok(savingsAccountResponse);
    }

    [HttpPost]
    public async Task<ActionResult<SavingsAccountResponse>> Post([FromBody] SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken)
    {
        var response = await serviceManager.SavingsAccountService.CreateAsync(savingsAccountRequest, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await serviceManager.SavingsAccountService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] SavingsAccountRequest savingsAccountRequest, CancellationToken cancellationToken)
    {
        await serviceManager.SavingsAccountService.UpdateAsync(id, savingsAccountRequest, cancellationToken);

        return NoContent();
    }
}
