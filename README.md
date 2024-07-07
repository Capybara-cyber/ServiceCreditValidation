Credit Validation API

This API provides an endpoint to submit a credit data and receive a decision along with an optional interest rate.

Endpoint
The default endpoint for making a POST request is:
https://localhost:8000/api/credit/decision

Request
CreditRequest

Represents the data required to submit a credit application.


public class CreditRequest
{

    /// <summary>
    /// The amount of applied amount.
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// the term (repayment in months)
    /// </summary>
    public int Term { get; set; }

    /// <summary>
    /// The amount of pre-existing credit amount.
    /// </summary>
    public decimal PreExistingCreditAmount { get; set; }
}

Example:
{
  "CreditAmount": 25000,
  "Term": 24,
  "PreExistingCreditAmount": 10000
}

Response
CreditResponse

Represents the decision and interest rate (if applicable) for the submitted credit request.

public class CreditResponse
{

    /// <summary>
    /// Indicates whether the credit application is approved (true) or rejected (false).
    /// </summary>
    public bool Decision { get; set; }

    /// <summary>
    /// The interest rate offered for the approved credit application.
    /// </summary>
    public decimal? InterestRate { get; set; }
}

Example:
{
  "Decision": true,
  "InterestRate": 4.5
}

Validation
CreditRequestValidator

Validates the CreditRequest data before processing.

public CreditRequestValidator()
{

    RuleFor(x => x.CreditAmount).GreaterThan(0).WithMessage("Credit amount must be greater than zero.");
    RuleFor(x => x.Term).GreaterThan(0).WithMessage("Term must be greater than zero.");
    RuleFor(x => x.PreExistingCreditAmount).GreaterThanOrEqualTo(0).WithMessage("Pre-existing credit amount cannot be negative.");
}
