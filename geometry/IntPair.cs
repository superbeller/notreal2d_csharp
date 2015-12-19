using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    public class IntPair : Pair
    {
        public IntPair()
        {
        }

        public IntPair(int? n2, int? n3)
            : base(n2, n3)
        {
        }

        public IntPair(SimplePair simplePair)
            : base(simplePair)
        {
        }
    }

}
