using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    class Variable : INode
    {
        private String name;

        public Variable(String name)
        {
            this.name = name;
        }

        public double Value(Dictionary<string, double> variables)
        {
            if (variables.ContainsKey(name))
                return variables[name];

            throw new Exception("Переданно недостаточно переменных");
        }
    }
}
