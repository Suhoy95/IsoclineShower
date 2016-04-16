using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Actg : INode
    {
        private INode value;

        public Actg(INode value)
        {
            this.value = value;
        }

        public double Value(Dictionary<string, double> variables)
        {
            return -Math.Atan(value.Value(variables)) + Math.PI / 2;
        }
    }
}
