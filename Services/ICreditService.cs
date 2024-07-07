using ServiceCreditValidation.Models;

namespace ServiceCreditValidation.Services
{
    public interface ICreditService
    {
        CreditResponse ProcessCreditApplication(CreditRequest request);
    }

}
