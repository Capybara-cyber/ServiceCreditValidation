using FluentValidation;
using ServiceCreditValidation.Models;

namespace ServiceCreditValidation.Validators
{
    public class CreditRequestValidator : AbstractValidator<CreditRequest>
    {
        public CreditRequestValidator()
        {
            RuleFor(x => x.CreditAmount).GreaterThan(0).WithMessage("Credit amount must be greater than zero.");
            RuleFor(x => x.Term).GreaterThan(0).WithMessage("Term must be greater than zero.");
            RuleFor(x => x.PreExistingCreditAmount).GreaterThanOrEqualTo(0).WithMessage("Pre-existing credit amount cannot be negative.");
        }
    }
}
