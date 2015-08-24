using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Product : INode
    {
        private List<INode> multipliers;

        public Product(List<INode> multipliers)
        {
            this.multipliers = multipliers;
        }

        public double Value(Dictionary<string, double> variables)
        {
            double result = 1.0;

            foreach (var multiplier in multipliers)
            {
                result *= multiplier.Value(variables);
            }

            return result;
        }
    }
}
