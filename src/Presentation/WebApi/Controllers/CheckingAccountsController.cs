using DataTransfertObjects;

using Microsoft.AspNetCore.Mvc;

using Services.Abstractions;

namespace WebApplicationDocker.Controllers;

[ApiController]
[Route("api/checkingaccounts")]
public class CheckingAccountsController(ICheckingAccountService checkingAccountService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CheckingAccountResponse>> Get()
    {
        var checkingAccountsResponse = checkingAccountService.GetAll();

        return Ok(checkingAccountsResponse);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CheckingAccountResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        var checkingAccountResponse = await checkingAccountService.GetByIdAsync(id, cancellationToken);

        return Ok(checkingAccountResponse);
    }

    [HttpPost]
    public async Task<ActionResult<CheckingAccountResponse>> Post([FromBody] CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        var response = await checkingAccountService.CreateAsync(checkingAccountRequest, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await checkingAccountService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] CheckingAccountRequest checkingAccountRequest, CancellationToken cancellationToken)
    {
        await checkingAccountService.UpdateAsync(id, checkingAccountRequest, cancellationToken);

        return NoContent();
    }
}
