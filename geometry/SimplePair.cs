using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    public class SimplePair
    {
        private object first;
        private object second;

        public SimplePair()
        {
        }

        public SimplePair(object @object, object object2)
        {
            this.first = @object;
            this.second = object2;
        }

        public SimplePair(SimplePair simplePair)
        {
            this.first = simplePair.first;
            this.second = simplePair.second;
        }

        public virtual object First
        {
            get
            {
                return this.first;
            }
            set
            {
                this.first = value;
            }
        }


        public virtual object Second
        {
            get
            {
                return this.second;
            }
            set
            {
                this.second = value;
            }
        }


        public override bool Equals(object @object)
        {
            if (this == @object)
            {
                return true;
            }
            if (!(@object is SimplePair))
            {
                return false;
            }
            SimplePair simplePair = (SimplePair)@object;
            return (this.first == null ? simplePair.first == null : this.first.Equals(simplePair.first)) && (this.second == null ? simplePair.second == null : this.second.Equals(simplePair.second));
        }

        public override int GetHashCode()
        {
            int n2 = this.first == null ? 0 : this.first.GetHashCode();
            n2 = 31 * n2 + (this.second == null ? 0 : this.second.GetHashCode());
            return n2;
        }

        //public override string ToString()
        //{
        //    return SimplePair.ToString(this);
        //}

        //public static string ToString(SimplePair simplePair)
        //{
        //    return SimplePair.ToString(typeof(SimplePair), simplePair);
        //}

        //public static string ToString(Type class_, SimplePair simplePair)
        //{
        //    return StringUtil.ToString(class_, (object)simplePair, false, "first", "second");
        //}
    }

}
