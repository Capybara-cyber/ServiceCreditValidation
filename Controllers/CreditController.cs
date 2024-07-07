using Microsoft.AspNetCore.Mvc;
using ServiceCreditValidation.Models;
using ServiceCreditValidation.Services;
using ServiceCreditValidation.Validators;

[ApiController]
[Route("api/[controller]")]
public class CreditController : ControllerBase
{
    private readonly ICreditService _creditService;
    private readonly CreditRequestValidator _validator;

    public CreditController(ICreditService creditService, CreditRequestValidator validator)
    {
        _creditService = creditService;
        _validator = validator;
    }

    [HttpPost("decision")]
    public IActionResult GetCreditDecision([FromBody] CreditRequest request)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var response = _creditService.ProcessCreditApplication(request);
        return Ok(response);
    }
}
