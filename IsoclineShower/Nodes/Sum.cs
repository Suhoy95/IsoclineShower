using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Sum : INode
    {
        private INode first;
        private INode second;

        public Sum(INode first, INode second)
        {
            this.first = first;
            this.second = second;
        }

        public double Value(Dictionary<string, double> variables)
        {
            return first.Value(variables) + second.Value(variables);
        }
    }
}
