using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Acos : INode
    {
        private INode value;

        public Acos(INode value)
        {
            this.value = value;
        }

        public double Value(Dictionary<string, double> variables)
        {
            if ((value.Value(variables) >= -1) && (value.Value(variables) <= 1))
                return Math.Acos(value.Value(variables));
            throw new Exception("Вне зоны определения");
        }
    }
}
