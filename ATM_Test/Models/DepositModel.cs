using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM_Test.Models
{
    [Table("DepositModel")]
    public class DepositModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public uint Unit { get; set; }

        public ulong Quantity { get; set; }

    }

}