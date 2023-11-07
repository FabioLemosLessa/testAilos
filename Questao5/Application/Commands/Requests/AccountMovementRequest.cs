namespace Questao5.Application.Commands.Requests
{
    public class AccountMovementRequest
    {
        public string RequestId { get; set; }
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public string MovementType { get; set; }
    }
}
