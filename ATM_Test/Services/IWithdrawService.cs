using System.Collections.Generic;

namespace ATM_Test.Services;

/// <summary>
/// WithdrawService is responsible for handling withdrawal operations from the ATM.
/// </summary>
public interface IWithdrawService
{
    /// <summary>
    /// Withdraws a specified sum from the ATM.
    /// </summary>
    /// <param name="sum">Sum amount</param>
    /// <returns>Denominations and quantities of bank notes used<</returns>
    Dictionary<uint, ulong> Withdraw(ulong sum);
}
