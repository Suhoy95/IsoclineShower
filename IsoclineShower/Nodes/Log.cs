using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Log : INode
    {
        private INode first;
        private INode second;

        public Log(INode first, INode second)
        {
            this.first = first;
            this.second = second;
        }

        public double Value(Dictionary<string, double> variables)
        {
            var value = first.Value(variables);
            var newBase = second.Value(variables);
            if (newBase < 0)
                throw new Exception("Отрицательное основание");
            if (newBase == 1)
                throw new Exception("Основание равно единице");
            if (value < 0)
                throw new Exception("Логарифм от отрицательного числа");
            
            return Math.Log(first.Value(variables), second.Value(variables));
        }
    }
}
