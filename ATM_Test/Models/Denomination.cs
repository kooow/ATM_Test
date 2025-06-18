namespace ATM_Test.Models;

/// <summary>
/// Denomination class representing different banknote denominations.
/// </summary>
public class Denomation : Enumeration
{
    public static readonly Denomation Thousand = new(1000);
    public static readonly Denomation TwoThousand = new(2000);
    public static readonly Denomation FiveThousand = new(5000);
    public static readonly Denomation TenThousand = new(10000);
    public static readonly Denomation TwentyThousand = new(20000);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="key">Key</param>
    public Denomation(uint key) : base(key) { }
}