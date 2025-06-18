using ATM_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM_Test.Services;

/// <summary>
/// DepositService is responsible for handling deposit operations into the ATM's bank notes.
/// </summary>
public class DepositService : IDepositService
{
    protected readonly APIDbContext m_context;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context">Database context</param>
    public DepositService(APIDbContext context)
    {
        m_context = context;
    }

    private List<BankNote> GetBankNotes()
    {
        var denomationUnits = Denomation.GetAll<Denomation>().Select(d => d.Unit).ToList();
        var bankNotes = m_context.Set<BankNote>().Where(bn => denomationUnits.Contains(bn.Value)).ToList();
        return bankNotes;
    }

    /// <summary>
    /// Deposits the specified values into the ATM's bank notes.
    /// </summary>
    /// <param name="depositValues">Deposit values</param>
    public void Deposit(Dictionary<string, uint> depositValues)
    {
        var bankNotes = GetBankNotes();

        try
        {
            foreach (KeyValuePair<string, uint> denomAndQuantity in depositValues)
            {
                var denom = uint.Parse(denomAndQuantity.Key);
                var model = bankNotes.FirstOrDefault(bn => bn.Value == denom);
                checked
                {
                    model.Quantity += denomAndQuantity.Value;
                }
            }
        }
        catch (OverflowException)
        {
            throw;
        }

        m_context.SaveChanges();
    }

    /// <summary>
    /// Calculates the total amount of money in the ATM based on the bank notes and their quantities.
    /// </summary>
    /// <returns>Total amount</returns>
    public ulong CalculateTotal()
    {
        ulong total = 0;

        var bankNotes = GetBankNotes();
        bankNotes.ForEach(bn => total += bn.Value * bn.Quantity);

        return total;
    }
}