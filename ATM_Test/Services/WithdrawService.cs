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

        private static Dictionary<uint, ulong> CalculatePossibleQuantities(List<DepositModel> depositModels, ulong sum)
        {
            Dictionary<uint, ulong> noteAndQuantityList = new();

            var modelsWhereQuantityNotZero = depositModels.Where(dm => dm.Quantity > 0).OrderByDescending(dm => dm.Unit);

            foreach (var dp in modelsWhereQuantityNotZero)
            {
                if (sum >= dp.Unit)
                {
                    var usedAmount = sum / dp.Unit;
                    if (usedAmount <= dp.Quantity) // we have enought
                    {
                        sum -= (usedAmount * dp.Unit);
                        noteAndQuantityList.Add(dp.Unit, usedAmount);
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

            var depositModels = _context.Set<DepositModel>().Where(dm => denomationUnits.Contains(dm.Unit)).ToList();

            var possibleUnitAndQuantites = CalculatePossibleQuantities(depositModels, sum);
            if (possibleUnitAndQuantites.Count > 0) // we have a possible solution
            {
                foreach (var unitAndQuantity in possibleUnitAndQuantites)
                {
                    var dm = depositModels.FirstOrDefault(dm => dm.Unit == unitAndQuantity.Key);
                    dm.Quantity -= unitAndQuantity.Value;
                }

                _context.SaveChanges();
            }

            return possibleUnitAndQuantites;
        }
    }
}
