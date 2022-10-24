using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ATM_Test.Models
{
    public abstract class Enumeration : IComparable
    {
        public uint Key { get; private set; }

        protected Enumeration(uint key) => (Key) = (key);

        public override string ToString() => Key.ToString();

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
            var valueMatches = Key.Equals(otherValue.Key);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Key.CompareTo(((Enumeration)other).Key);

    }

}