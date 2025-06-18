using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM_Test.Models;

/// <summary>
/// Bankote model representing a currency note in the ATM system.
/// </summary>
[Table("BankNote")]
public class BankNote
{
    /// <summary>
    /// Value of the banknote, represented as a uint.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public uint Value { get; set; }

    /// <summary>
    /// Quantity of the banknote available in the ATM.
    /// </summary>
    public ulong Quantity { get; set; }
}