using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ATM_Test.Models;

/// <summary>
/// Enumeration base class for defining a set of named constants with a unique unit value.
/// </summary>
public abstract class Enumeration : IComparable
{
    /// <summary>
    /// Unit value
    /// </summary>
    public uint Unit { get; private set; }

    protected Enumeration(uint unit) => (Unit) = (unit);

    /// <summary>
    /// Tostring method to return the string representation of the enumeration instance.
    /// </summary>
    /// <returns>ToString of the Unit</returns>
    public override string ToString() => Unit.ToString();

    /// <summary>
    /// Gets all instances of the enumeration type.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>All instances</returns>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                 .Select(f => f.GetValue(null))
                 .Cast<T>();

    /// <summary>
    /// Equals method to compare two Enumeration instances based on their unit value.
    /// </summary>
    /// <param name="obj">Another Enumeration objects</param>
    /// <returns>Equals or not</returns>
    public override bool Equals(object obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Unit.Equals(otherValue.Unit);

        return typeMatches && valueMatches;
    }

    /// <summary>
    /// Compares this enumeration instance with another based on their unit values.
    /// </summary>
    /// <param name="other">Other Enumeration object</param>
    /// <returns>Result of the comparsion</returns>
    public int CompareTo(object other) => Unit.CompareTo(((Enumeration)other).Unit);

    /// <summary>
    /// Hash code for the enumeration based on its unit value.
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        return Unit.GetHashCode();
    }
}