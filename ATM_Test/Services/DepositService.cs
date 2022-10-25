using ATM_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM_Test.Services
{
    public class DepositService : IDepositService
    {
        protected readonly APIDbContext _context;

        public DepositService(APIDbContext context)
        {
            _context = context;
        }

        private List<BankNote> GetBankNotes()
        {
            var denomationUnits = Denomation.GetAll<Denomation>().Select(d => d.Unit).ToList();
            var bankNotes = _context.Set<BankNote>().Where(bn => denomationUnits.Contains(bn.Value)).ToList();
            return bankNotes;
        }

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

            _context.SaveChanges();
        }

        public ulong CalculateTotal()
        {
            ulong total = 0;

            var bankNotes = GetBankNotes();
            bankNotes.ForEach(bn => total += bn.Value * bn.Quantity);

            return total;
        }
    }
}