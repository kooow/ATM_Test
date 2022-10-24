using ATM_Test.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATM_Test.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class DepositService : IDepositService
    {

        protected readonly APIDbContext _context;

        public DepositService(APIDbContext context)
        {
            _context = context;
        }

        private List<DepositModel> GetDepositModels()
        {
            var denomationUnits = Denomation.GetAll<Denomation>().Select(d => d.Unit).ToList();

            var depositModels = _context.Set<DepositModel>().Where(dm => denomationUnits.Contains(dm.Unit)).ToList();

            return depositModels;
        }

        public void Deposit(Dictionary<string, uint> depositValues)
        {
           var depositModels = GetDepositModels();

            foreach (KeyValuePair<string, uint> denomAndQuantity in depositValues)
            {
                var unit = uint.Parse(denomAndQuantity.Key);
                var model = depositModels.FirstOrDefault(dm => dm.Unit == unit);
                // TODO: overflow check
                model.Quantity += denomAndQuantity.Value;
            }

            _context.SaveChanges();
        }

        public ulong CalculateTotal()
        {
            ulong total = 0;

            var depositModels = GetDepositModels();

            foreach (var depo in depositModels)
            {
                total += depo.Unit * depo.Quantity;
            }

            return total;
        }

    }

}