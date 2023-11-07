using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Handlers
{
    public class AccountBalanceHandler
    {
        public AccountBalanceResponse Handle(AccountBalanceRequest request)
        {
            if (!IsAccountValid(request.AccountId))
            {
                return new AccountBalanceResponse { ErrorType = "INVALID_ACCOUNT", ErrorMessage = "Conta corrente inválida." };
            }

            if (!IsAccountActive(request.AccountId))
            {
                return new AccountBalanceResponse { ErrorType = "INACTIVE_ACCOUNT", ErrorMessage = "Conta corrente não está ativa." };
            }

            decimal currentBalance = CalculateCurrentBalance(request.AccountId);

            return new AccountBalanceResponse
            {
                AccountNumber = request.AccountId,
                AccountHolderName = "Nome do Titular",
                ResponseDateTime = DateTime.Now,
                CurrentBalance = currentBalance
            };
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

        private decimal CalculateCurrentBalance(string accountId)
        {
            using (var connection = new SqliteConnection(databaseConfig.Name))
            {
                connection.Open();

                // Consulta para obter as movimentações da conta corrente
                var sql = "SELECT valor, tipomovimento FROM movimento WHERE idcontacorrente = @AccountId";
                var movements = connection.Query<(decimal Valor, string TipoMovimento)>(sql, new { AccountId = accountId });

                decimal currentBalance = 0.00m;

                foreach (var movement in movements)
                {
                    if (movement.TipoMovimento == "C")
                    {
                        currentBalance += movement.Valor; // Crédito
                    }
                    else if (movement.TipoMovimento == "D")
                    {
                        currentBalance -= movement.Valor; // Débito
                    }
                }

                return currentBalance;
            }
        }
    }
}