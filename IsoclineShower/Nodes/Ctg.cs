using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Ctg : INode
    {
        private INode value;

        public Ctg(INode value)
        {
            this.value = value;
        }

        public double Value(Dictionary<string, double> variables)
        {
            return 1 / Math.Tan(value.Value(variables));
        }
    }
}
