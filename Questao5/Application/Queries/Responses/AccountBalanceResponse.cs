namespace Questao5.Application.Queries.Responses
{
    public class AccountBalanceResponse
    {
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public decimal CurrentBalance { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
