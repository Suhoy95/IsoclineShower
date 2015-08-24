using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IsoclineShower.Nodes;

namespace IsoclineShower
{
    public class FunctionParser
    {
        // Принцып перебора символов будет как перевод в польскую запись
        // http://e-maxx.ru/algo/expressions_parsing
        //

        public static INode Parse(String expression)
        {
            var statements = new List<INode>();

            for (var i = 0; i < expression.Length; i++)
            {
                if (Char.IsDigit(expression[i]))
                {
                    statements.Add(GetNumber(expression, ref i));
                }
                else if(Char.IsLetter(expression[i]))
                {
                    statements.Add(GetVariable(expression, ref i));
                }

            }

            return statements.Last();
        }

        

        private static Constant GetNumber(String expression, ref int i)
        {
            int position = i;
            while (i < expression.Length && 
                    (Char.IsDigit(expression[i]) || expression[i] == ','))
                i++;

            var number = expression.Substring(position, i - position);

            return new Constant(double.Parse(number));
        }

        private static Variable GetVariable(string expression, ref int i)
        {
            int position = i;
            while (i < expression.Length && Char.IsLetter(expression[i]) )
                i++;

            var variableName = expression.Substring(position, i - position);

            return new Variable(variableName);
        }
    }
}
