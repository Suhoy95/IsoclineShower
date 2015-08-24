using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IsoclineShower.Nodes;

namespace IsoclineShower
{
    public class FunctionParser
    {
        public static INode Parse(String expression)
        {
            return new Constant(1.0);
        }
    }
}
