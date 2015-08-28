using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Pow : INode
    {
        private INode first;
        private INode second;

        public Pow(INode first, INode second)
        {
            this.first = first;
            this.second = second;
        }

        public double Value(Dictionary<string, double> variables)
        {
            var newBase = first.Value(variables);
            if (newBase >= 0)
                return Math.Pow(first.Value(variables), second.Value(variables));
            throw new Exception("Отрицательное основание");
        }
    }
}
