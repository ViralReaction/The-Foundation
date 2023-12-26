using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    public static class SPTuples
    {
        public static class SPTuple
        {
            public static SPTuples.SPTuple<T1, T2> Create<T1, T2>(T1 first, T2 second) => new SPTuples.SPTuple<T1, T2>(first, second);

            public static SPTuples.SPTuple<T1, T2, T3> Create<T1, T2, T3>(T1 first, T2 second, T3 third) => new SPTuples.SPTuple<T1, T2, T3>(first, second, third);
        }

        public class SPTuple<T1, T2>
        {
            private static readonly IEqualityComparer<T1> FirstComparer = (IEqualityComparer<T1>)EqualityComparer<T1>.Default;
            private static readonly IEqualityComparer<T2> SecondComparer = (IEqualityComparer<T2>)EqualityComparer<T2>.Default;

            public T1 First { get; set; }

            public T2 Second { get; set; }

            public SPTuple(T1 first, T2 second)
            {
                this.First = first;
                this.Second = second;
            }

            public static bool operator ==(SPTuples.SPTuple<T1, T2> o1, SPTuples.SPTuple<T1, T2> o2) => o1.Equals((object)o2);

            public static bool operator !=(SPTuples.SPTuple<T1, T2> o1, SPTuples.SPTuple<T1, T2> o2) => !(o1 == o2);

            public override int GetHashCode()
            {
                int hashCode = 0;
                if ((object)this.First != null)
                    hashCode = SPTuples.SPTuple<T1, T2>.FirstComparer.GetHashCode(this.First);
                if ((object)this.Second != null)
                    hashCode = (hashCode << 5) + hashCode ^ SPTuples.SPTuple<T1, T2>.SecondComparer.GetHashCode(this.Second);
                return hashCode;
            }

            public override bool Equals(object o)
            {
                SPTuples.SPTuple<T1, T2> spTuple = o as SPTuples.SPTuple<T1, T2>;
                return (object)spTuple != null && SPTuples.SPTuple<T1, T2>.FirstComparer.Equals(this.First, spTuple.First) && SPTuples.SPTuple<T1, T2>.SecondComparer.Equals(this.Second, spTuple.Second);
            }
        }

        public class SPTuple<T1, T2, T3> : SPTuples.SPTuple<T1, T2>
        {
            private static readonly IEqualityComparer<T1> FirstComparer = (IEqualityComparer<T1>)EqualityComparer<T1>.Default;
            private static readonly IEqualityComparer<T2> SecondComparer = (IEqualityComparer<T2>)EqualityComparer<T2>.Default;
            private static readonly IEqualityComparer<T3> ThirdComparer = (IEqualityComparer<T3>)EqualityComparer<T3>.Default;

            public T3 Third { get; set; }

            public SPTuple(T1 first, T2 second, T3 third)
              : base(first, second)
            {
                this.Third = third;
            }

            public static bool operator ==(
              SPTuples.SPTuple<T1, T2, T3> o1,
              SPTuples.SPTuple<T1, T2, T3> o2)
            {
                return o1.Equals((object)o2);
            }

            public static bool operator !=(
              SPTuples.SPTuple<T1, T2, T3> o1,
              SPTuples.SPTuple<T1, T2, T3> o2)
            {
                return !(o1 == o2);
            }

            public override int GetHashCode()
            {
                int hashCode = 0;
                if ((object)this.First != null)
                    hashCode = SPTuples.SPTuple<T1, T2, T3>.FirstComparer.GetHashCode(this.First);
                if ((object)this.Second != null)
                    hashCode = (hashCode << 3) + hashCode ^ SPTuples.SPTuple<T1, T2, T3>.SecondComparer.GetHashCode(this.Second);
                if ((object)this.Third != null)
                    hashCode = (hashCode << 5) + hashCode ^ SPTuples.SPTuple<T1, T2, T3>.ThirdComparer.GetHashCode(this.Third);
                return hashCode;
            }

            public override bool Equals(object o)
            {
                SPTuples.SPTuple<T1, T2, T3> spTuple = o as SPTuples.SPTuple<T1, T2, T3>;
                return (object)spTuple != null && SPTuples.SPTuple<T1, T2, T3>.FirstComparer.Equals(this.First, spTuple.First) && SPTuples.SPTuple<T1, T2, T3>.SecondComparer.Equals(this.Second, spTuple.Second) && SPTuples.SPTuple<T1, T2, T3>.ThirdComparer.Equals(this.Third, spTuple.Third);
            }
        }

        public struct SPTuple2<T1, T2> : IEquatable<SPTuples.SPTuple2<T1, T2>>
        {
            private T1 first;
            private T2 second;

            public SPTuple2(T1 first, T2 second)
            {
                this.first = first;
                this.second = second;
            }

            public T1 First
            {
                get => this.first;
                set
                {
                    if ((object)value != null)
                        this.first = value;
                    Log.Error("Tried to assign value of different type to Tuple of type " + (object)typeof(T1), false);
                }
            }

            public T2 Second
            {
                get => this.second;
                set
                {
                    if ((object)value != null)
                        this.second = value;
                    Log.Error("Tried to assign value of different type to Tuple of type " + (object)typeof(T2), false);
                }
            }

            public override int GetHashCode()
            {
                int hashCode = 0;
                if ((object)this.First != null)
                    hashCode = EqualityComparer<T1>.Default.GetHashCode(this.First);
                if ((object)this.Second != null)
                    hashCode = (hashCode << 3) + hashCode ^ EqualityComparer<T2>.Default.GetHashCode(this.Second);
                return hashCode;
            }

            public override bool Equals(object obj)
            {
                int num;
                switch (obj)
                {
                    case SPTuples.SPTuple2<T1, T2> other1 when this.Equals(other1):
                        num = 1;
                        break;
                    case Pair<T1, T2> other2:
                        num = this.Equals(other2) ? 1 : 0;
                        break;
                    default:
                        num = 0;
                        break;
                }
                return num != 0;
            }

            public bool Equals(SPTuples.SPTuple2<T1, T2> other) => EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);

            public bool Equals(Pair<T1, T2> other) => EqualityComparer<T1>.Default.Equals(this.first, other.First) && EqualityComparer<T2>.Default.Equals(this.second, other.Second);

            public static bool operator ==(SPTuples.SPTuple2<T1, T2> lhs, SPTuples.SPTuple2<T1, T2> rhs) => lhs.Equals(rhs);

            public static bool operator !=(SPTuples.SPTuple2<T1, T2> lhs, SPTuples.SPTuple2<T1, T2> rhs) => !(lhs == rhs);

            public static bool operator ==(Pair<T1, T2> lhs, SPTuples.SPTuple2<T1, T2> rhs) => lhs.Equals((object)rhs);

            public static bool operator !=(Pair<T1, T2> lhs, SPTuples.SPTuple2<T1, T2> rhs) => !(lhs == rhs);
        }
    }
}