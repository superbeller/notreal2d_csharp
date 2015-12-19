using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    public class Pair : SimplePair
    {
        public Pair()
        {
        }

        public Pair(IComparable comparable, IComparable comparable2)
            : base(comparable, comparable2)
        {
        }

        public Pair(SimplePair simplePair)
            : base(simplePair)
        {
        }

        public int CompareTo(Pair pair)
        {
            int n2;
            if (this.First != pair.First)
            {
                if (this.First == null)
                {
                    return -1;
                }
                if (pair.First == null)
                {
                    return 1;
                }
                n2 = ((IComparable)this.First).CompareTo(pair.First);
                if (n2 != 0)
                {
                    return n2;
                }
            }
            if (this.Second != pair.Second)
            {
                if (this.Second == null)
                {
                    return -1;
                }
                if (pair.Second == null)
                {
                    return 1;
                }
                n2 = ((IComparable)this.Second).CompareTo(pair.Second);
                if (n2 != 0)
                {
                    return n2;
                }
            }
            return 0;
        }

        public virtual bool Equals(IComparable comparable, IComparable comparable2)
        {
            return (this.First == null ? comparable == null : ((IComparable)this.First).Equals(comparable)) && (this.Second == null ? comparable2 == null : ((IComparable)this.Second).Equals(comparable2));
        }

        public override bool Equals(object @object)
        {
            return base.Equals(@object);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //public override string ToString()
        //{
        //    return Pair.ToString(this);
        //}

        //public static string ToString(Pair pair)
        //{
        //    return Pair.ToString(typeof(Pair), pair);
        //}
    }
}
