using System.Collections.Generic;

namespace ATM_Test.Services
{
    public interface IWithdrawService
    {
        Dictionary<uint, ulong> Withdraw(ulong sum);
    }
}
