using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FontAwesome.Net
{
    public abstract class Enumeration
        : IComparable
    {
        public int Id { get; }
        public string Name { get; private set; }

        protected Enumeration(int id, string name)
            => (Id, Name) = (id, name);

        public override string ToString()
            => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
            => typeof(T)
                .GetFields(BindingFlags.Public |
                           BindingFlags.Static |
                           BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();

        public bool Equals(Enumeration other)
        {
            if (other is null)
                return false;

            if (GetType() != other.GetType())
                return false;

            return Id == other.Id;
        }

        public override bool Equals(object obj)
            => Equals(obj as Enumeration);

        public override int GetHashCode()
            => (GetType().ToString() + Id).GetHashCode();

        public int CompareTo(object other) 
            => Id.CompareTo(((Enumeration)other).Id);
    }
}
