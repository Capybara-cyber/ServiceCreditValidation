namespace ServiceCreditValidation.Models
{
    public class CreditResponse
    {
        public bool Decision { get; set; }
        public decimal? InterestRate { get; set; }
    }
}
