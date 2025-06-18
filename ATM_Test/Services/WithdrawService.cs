using ATM_Test.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATM_Test.Services;

/// <summary>
/// WithdrawService is responsible for handling withdrawal operations from the ATM.
/// </summary>
public class WithdrawService : IWithdrawService
{
    protected readonly APIDbContext m_context;

    /// <summary>
    /// Constructor for WithdrawService.
    /// </summary>
    /// <param name="context">Database context</param>
    public WithdrawService(APIDbContext context)
    {
        m_context = context;
    }

    private static Dictionary<uint, ulong> CalculatePossibleQuantities(List<BankNote> bankNotes, ulong sum)
    {
        Dictionary<uint, ulong> noteAndQuantityList = new();

        var modelsWhereQuantityNotZero = bankNotes.Where(dm => dm.Quantity > 0).OrderByDescending(dm => dm.Value);

        foreach (var bankNote in modelsWhereQuantityNotZero)
        {
            if (sum >= bankNote.Value)
            {
                var usedAmount = sum / bankNote.Value;
                if (usedAmount <= bankNote.Quantity) // we have enought
                {
                    sum -= (usedAmount * bankNote.Value);
                    noteAndQuantityList.Add(bankNote.Value, usedAmount);
                }

                if (sum == 0) break;
            }
        }
        
        if (sum != 0)
        {
            return new Dictionary<uint, ulong>();
        }

        return noteAndQuantityList;
    }

    /// <summary>
    /// Withdraws a specified sum from the ATM, returning the denominations and quantities of bank notes used.
    /// </summary>
    /// <param name="sum">Sum amount</param>
    /// <returns>Denominations and quantities of bank notes used</returns>
    public Dictionary<uint, ulong> Withdraw(ulong sum)
    {
        var denomationUnits = Denomation.GetAll<Denomation>().Select(d => d.Unit).ToList();

        var bankNotes = m_context.Set<BankNote>().Where(bn => denomationUnits.Contains(bn.Value)).ToList();

        var possibleUnitAndQuantites = CalculatePossibleQuantities(bankNotes, sum);
        if (possibleUnitAndQuantites.Count > 0) // we have a possible solution
        {
            foreach (var unitAndQuantity in possibleUnitAndQuantites)
            {
                var bankNote = bankNotes.FirstOrDefault(bn => bn.Value.Equals(unitAndQuantity.Key));
                bankNote.Quantity -= unitAndQuantity.Value;
            }

            m_context.SaveChanges();
        }

        return possibleUnitAndQuantites;
    }
}
