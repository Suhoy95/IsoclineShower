using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower.Nodes
{
    public interface INode
    {
        double Value(Dictionary<String, double> variables = null);
    }
}
