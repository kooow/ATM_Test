using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ATM_Test.Models
{
    public abstract class Enumeration : IComparable
    {
        public uint Unit { get; private set; }

        protected Enumeration(uint unit) => (Unit) = (unit);

        public override string ToString() => Unit.ToString();

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();

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

        public int CompareTo(object other) => Unit.CompareTo(((Enumeration)other).Unit);
    }
}