using ATM_Test.Models;
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

            foreach (KeyValuePair<string, uint> denomAndQuantity in depositValues)
            {
                var denom = uint.Parse(denomAndQuantity.Key);
                var model = bankNotes.FirstOrDefault(bn => bn.Value == denom);
                // TODO: overflow check
                model.Quantity += denomAndQuantity.Value;
            }

            _context.SaveChanges();
        }

        public ulong CalculateTotal()
        {
            ulong total = 0;

            var bankNotes = GetBankNotes();

            foreach (var note in bankNotes)
            {
                total += note.Value * note.Quantity;
            }

            return total;
        }
    }
}