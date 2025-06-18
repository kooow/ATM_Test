using System.Collections.Generic;

namespace ATM_Test.Services;

/// <summary>
/// Interface for deposit service
/// </summary>
public interface IDepositService
{
    /// <summary>
    /// Deposit a collection of bank notes into the ATM.
    /// </summary>
    /// <param name="depositValues">Deposit values</param>
    void Deposit(Dictionary<string, uint> depositValues);

    /// <summary>
    /// Calculate the total amount of money in the ATM based on the bank notes stored in the database.
    /// </summary>
    /// <returns>Total amount</returns>
    ulong CalculateTotal();
}