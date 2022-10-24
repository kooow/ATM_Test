
using ATM_Test.Models;
using System.Collections.Generic;

namespace ATM_Test.Services
{
    public interface IDepositService
    {
        void Deposit(Dictionary<string, uint> depositValues);

        ulong CalculateTotal();

    }

}