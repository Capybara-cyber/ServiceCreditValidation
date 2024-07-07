using ServiceCreditValidation.Models;

namespace ServiceCreditValidation.Services
{
    public class CreditService : ICreditService
    {   
        //Map the debt rules
        private static readonly Dictionary<Func<decimal, bool>, decimal> InterestRateMap = new()
        {
        { debt => debt < 20000, 3m },
        { debt => debt < 40000, 4m },
        { debt => debt < 60000, 5m },
        { debt => debt >= 60000, 6m }
        };

        public CreditResponse ProcessCreditApplication(CreditRequest request)
        {
            var response = new CreditResponse();

            if (request.CreditAmount < 2000 || request.CreditAmount > 69000)
            {
                response.Decision = false;
                response.InterestRate = null;
                return response;
            }

            response.Decision = true;
            var totalFutureDebt = request.CreditAmount + request.PreExistingCreditAmount;

            foreach (var entry in InterestRateMap)
            {
                if (entry.Key(totalFutureDebt))
                {
                    response.InterestRate = entry.Value;
                    break;
                }
            }

            return response;
        }
    }
}
