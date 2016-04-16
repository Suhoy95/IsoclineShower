using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Subtraction : INode
    {
        private INode first;
        private INode second;

        public Subtraction(INode first, INode second)
        {
            this.first = first;
            this.second = second;
        }

        public double Value(Dictionary<string, double> variables)
        {
            return first.Value(variables) - second.Value(variables);
        }
    }
}
