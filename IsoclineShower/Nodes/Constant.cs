using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Constant : INode
    {
        private double value;

        public Constant(double value)
        {
            this.value = value;
        }

        public double Value(Dictionary<string, double> variables)
        {
            return value;
        }
    }
}
