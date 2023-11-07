using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Handlers
{
    public class AccountMovementHandler
    {
        public AccountMovementResponse Handle(AccountMovementRequest request)
        {
            var response = new AccountMovementResponse();

            if (!IsAccountValid(request.AccountId))
            {
                response.ErrorType = "INVALID_ACCOUNT";
                response.ErrorMessage = "Conta corrente inválida.";
                return response;
            }

            if (!IsAccountActive(request.AccountId))
            {
                response.ErrorType = "INACTIVE_ACCOUNT";
                response.ErrorMessage = "Conta corrente não está ativa.";
                return response;
            }

            if (request.Amount <= 0)
            {
                response.ErrorType = "INVALID_VALUE";
                response.ErrorMessage = "O valor deve ser positivo.";
                return response;
            }

            if (request.MovementType != "C" && request.MovementType != "D")
            {
                response.ErrorType = "INVALID_TYPE";
                response.ErrorMessage = "Tipo de movimento inválido. Use 'C' para Crédito ou 'D' para Débito.";
                return response;
            }

            using (var connection = new SqliteConnection(databaseConfig.Name))
            {
                connection.Open();

                var sql = "INSERT INTO MOVIMENTO (Id, AccountId, Amount, MovementType) VALUES (@Id, @AccountId, @Amount, @MovementType)";

                var parameters = new
                {
                    Id = response.MovementId,
                    AccountId = request.AccountId,
                    Amount = request.Amount,
                    MovementType = request.MovementType
                };

                connection.Execute(sql, parameters);
            }

            response.MovementId = GenerateMovementId();

            return response;
        }

        private bool IsAccountValid(string accountId)
        {
            using (var connection = new SqliteConnection(databaseConfig.Name))
            {
                connection.Open();

                var sql = "SELECT COUNT(*) FROM contacorrente WHERE idcontacorrente = @AccountId";
                var count = connection.ExecuteScalar<int>(sql, new { AccountId = accountId });

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        private bool IsAccountActive(string accountId)
        {
            using (var connection = new SqliteConnection(databaseConfig.Name))
            {
                connection.Open();

                var sql = "SELECT COUNT(*) FROM contacorrente WHERE idcontacorrente = @AccountId AND ativo = 1";
                var count = connection.ExecuteScalar<int>(sql, new { AccountId = accountId });

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        private string GenerateMovementId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
