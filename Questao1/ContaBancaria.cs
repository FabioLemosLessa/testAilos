using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        public int NumeroConta { get; private set; }
        public string TitularConta { get; private set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numeroConta, string titularConta, double depositoInicial = 0.0)
        {
            NumeroConta = numeroConta;
            TitularConta = titularConta;
            Deposito(depositoInicial);
        }

        public void Deposito(double quantia)
        {
            if (quantia > 0)
            {
                Saldo += quantia;
            }
        }

        public void Saque(double quantia)
        {
            Saldo -= quantia + 3.50;
        }

        public override string ToString()
        {
            return "Conta " + NumeroConta + ", Titular: " + TitularConta + ", Saldo: $ " + Saldo.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}