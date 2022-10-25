using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM_Test.Models
{
    [Table("BankNote")]
    public class BankNote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public uint Value { get; set; }

        public ulong Quantity { get; set; }
    }

}