using ATM_Test.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATM_Test.Services
{
    public class WithdrawService : IWithdrawService
    {

        protected readonly APIDbContext _context;

        public WithdrawService(APIDbContext context)
        {
            _context = context;
        }

        private static Dictionary<uint, ulong> CalculatePossibleQuantities(List<BankNote> bankNotes, ulong sum)
        {
            Dictionary<uint, ulong> noteAndQuantityList = new();

            var modelsWhereQuantityNotZero = bankNotes.Where(dm => dm.Quantity > 0).OrderByDescending(dm => dm.Value);

            foreach (var dp in modelsWhereQuantityNotZero)
            {
                if (sum >= dp.Value)
                {
                    var usedAmount = sum / dp.Value;
                    if (usedAmount <= dp.Quantity) // we have enought
                    {
                        sum -= (usedAmount * dp.Value);
                        noteAndQuantityList.Add(dp.Value, usedAmount);
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

        public Dictionary<uint, ulong> Withdraw(ulong sum)
        {
            var denomationUnits = Denomation.GetAll<Denomation>().Select(d => d.Unit).ToList();

            var bankNotes = _context.Set<BankNote>().Where(bn => denomationUnits.Contains(bn.Value)).ToList();

            var possibleUnitAndQuantites = CalculatePossibleQuantities(bankNotes, sum);
            if (possibleUnitAndQuantites.Count > 0) // we have a possible solution
            {
                foreach (var unitAndQuantity in possibleUnitAndQuantites)
                {
                    var bankNote = bankNotes.FirstOrDefault(bn => bn.Value == unitAndQuantity.Key);
                    bankNote.Quantity -= unitAndQuantity.Value;
                }

                _context.SaveChanges();
            }

            return possibleUnitAndQuantites;
        }
    }
}
