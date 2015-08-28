using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Sin : INode
    {
        private INode value;

        public Sin(INode value)
        {
            this.value = value;
        }

        public double Value(Dictionary<string, double> variables)
        {
            return Math.Sin(value.Value(variables));
        }
    }
}
