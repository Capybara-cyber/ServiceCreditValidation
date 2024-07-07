namespace ServiceCreditValidation.Models
{
    public class CreditRequest
    {
        public decimal CreditAmount { get; set; }
        public int Term { get; set; }
        public decimal PreExistingCreditAmount { get; set; }
    }
}
