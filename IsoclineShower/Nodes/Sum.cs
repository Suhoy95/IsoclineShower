using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Sum : INode
    {
        private List<INode> summands;

        public Sum(List<INode> summands)
        {
            this.summands = summands;
        }

        public double Value(Dictionary<string, double> variables)
        {
            double result = 0.0;

            foreach (var summand in summands)
            {
                result += summand.Value(variables);
            }

            return result;
            //return summands.Sum(summand => summand.Value(variables));
        }
    }
}
