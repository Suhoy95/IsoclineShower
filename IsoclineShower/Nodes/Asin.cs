using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Asin : INode
    {
        private INode value;

        public Asin(INode value)
        {
            this.value = value;
        }

        public double Value(Dictionary<string, double> variables)
        {
            if ((value.Value(variables) >= -1) && (value.Value(variables) <= 1))
                return Math.Asin(value.Value(variables));
            throw new Exception("Вне зоны определения");
        }
    }
}
