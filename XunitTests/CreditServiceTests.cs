using FluentValidation.TestHelper;
using ServiceCreditValidation.Services;
using ServiceCreditValidation.Validators;
using ServiceCreditValidation.Models;
using Xunit;

namespace ServiceCreditValidation.XunitTests
{
    public class CreditRequestValidatorTests
    {
        private readonly ICreditService _creditService;
        private readonly CreditRequestValidator _validator;

        public CreditRequestValidatorTests()
        {
            _creditService = new CreditService();
            _validator = new CreditRequestValidator();
        }

        [Theory]
        [InlineData(5000, 12, 0)] 
        [InlineData(2000, 24, 1000)]
        public void Should_PassValidation(decimal creditAmount, int term, decimal preExistingCreditAmount)
        {
            var request = new CreditRequest { CreditAmount = creditAmount, Term = term, PreExistingCreditAmount = preExistingCreditAmount };
            var result = _validator.TestValidate(request);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(0, 12, 0, "Credit amount must be greater than zero.")]
        [InlineData(5000, 0, 0, "Term must be greater than zero.")]
        [InlineData(2000, 24, -1000, "Pre-existing credit amount cannot be negative.")]
        public void Should_FailValidation(decimal creditAmount, int term, decimal preExistingCreditAmount, string expectedErrorMessage)
        {
            var request = new CreditRequest { CreditAmount = creditAmount, Term = term, PreExistingCreditAmount = preExistingCreditAmount };
            var result = _validator.TestValidate(request);
            Assert.False(result.IsValid);
            Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Should_FailValidation_When_CreditAmount_Is_Zero()
        {
            var request = new CreditRequest { CreditAmount = 0, Term = 12, PreExistingCreditAmount = 5000 };
            var result = _validator.TestValidate(request);
            Assert.False(result.IsValid);
            Assert.Equal("Credit amount must be greater than zero.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Should_FailValidation_When_Term_Is_Zero()
        {
            var request = new CreditRequest { CreditAmount = 5000, Term = 0, PreExistingCreditAmount = 5000 };
            var result = _validator.TestValidate(request);
            Assert.False(result.IsValid);
            Assert.Equal("Term must be greater than zero.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Should_PassValidation_When_PreExistingCreditAmount_Is_Negative()
        {
            var request = new CreditRequest { CreditAmount = 5000, Term = 12, PreExistingCreditAmount = -1000 };
            var result = _validator.TestValidate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Should_PassValidation_When_All_Properties_Are_Valid()
        {
            var request = new CreditRequest { CreditAmount = 25000, Term = 24, PreExistingCreditAmount = 10000 };
            var result = _validator.TestValidate(request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Reject_CreditAmount_LessThan_2000()
        {
            var request = new CreditRequest { CreditAmount = 1500, Term = 12, PreExistingCreditAmount = 5000 };
            var response = _creditService.ProcessCreditApplication(request);
            Assert.False(response.Decision);
            Assert.Null(response.InterestRate);
        }

        [Fact]
        public void Should_Approve_CreditAmount_Between_2000_And_69000()
        {
            var request = new CreditRequest { CreditAmount = 25000, Term = 24, PreExistingCreditAmount = 10000 };
            var response = _creditService.ProcessCreditApplication(request);
            Assert.True(response.Decision);
            Assert.Equal(4m, response.InterestRate);
        }

        [Fact]
        public void Should_Reject_CreditAmount_GreaterThan_69000()
        {
            var request = new CreditRequest { CreditAmount = 70000, Term = 36, PreExistingCreditAmount = 10000 };
            var response = _creditService.ProcessCreditApplication(request);
            Assert.False(response.Decision);
            Assert.Null(response.InterestRate);
        }

        [Fact]
        public void Should_Return_Correct_InterestRate_BasedOn_TotalFutureDebt()
        {
            var request = new CreditRequest { CreditAmount = 30000, Term = 18, PreExistingCreditAmount = 35000 };
            var response = _creditService.ProcessCreditApplication(request);
            Assert.True(response.Decision);
            Assert.Equal(6m, response.InterestRate);
        }

        [Fact]
        public void Should_Return_3_Percent_InterestRate_For_TotalFutureDebt_Less_Than_20000()
        {
            var request = new CreditRequest { CreditAmount = 10000, Term = 12, PreExistingCreditAmount = 5000 };
            var response = _creditService.ProcessCreditApplication(request);
            Assert.True(response.Decision);
            Assert.Equal(3m, response.InterestRate);
        }

        [Fact]
        public void Should_Return_4_Percent_InterestRate_For_TotalFutureDebt_Between_20000_And_39999()
        {
            var request = new CreditRequest { CreditAmount = 20000, Term = 24, PreExistingCreditAmount = 10000 };
            var response = _creditService.ProcessCreditApplication(request);
            Assert.True(response.Decision);
            Assert.Equal(4m, response.InterestRate);
        }

        [Fact]
        public void Should_Return_5_Percent_InterestRate_For_TotalFutureDebt_Between_40000_And_59999()
        {
            var request = new CreditRequest { CreditAmount = 40000, Term = 36, PreExistingCreditAmount = 20000 };
            var response = _creditService.ProcessCreditApplication(request);
            Assert.True(response.Decision);
            Assert.Equal(6m, response.InterestRate);
        }

        [Fact]
        public void Should_Return_6_Percent_InterestRate_For_TotalFutureDebt_Equal_To_Or_Greater_Than_60000()
        {
            var request = new CreditRequest { CreditAmount = 60000, Term = 48, PreExistingCreditAmount = 30000 };
            var response = _creditService.ProcessCreditApplication(request);
            Assert.True(response.Decision);
            Assert.Equal(6m, response.InterestRate);
        }
    }
}
