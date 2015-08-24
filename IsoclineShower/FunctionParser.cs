using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
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
            var statements = new Stack<INode>();
            var operations = new Stack<String>();

            for (var i = 0; i < expression.Length; i++)
            {
                if (IsOperation(expression, i))
                {
                    operations.Push(GetOperation(expression, ref i));
                }
                else if (Char.IsDigit(expression[i]))
                {
                    statements.Push(GetNumber(expression, ref i));
                }
                else if(Char.IsLetter(expression[i]))
                {
                    statements.Push(GetVariable(expression, ref i));
                }
            }

            

            return DeployStack(statements, operations);
        }

        private static INode DeployStack(Stack<INode> statements, Stack<string> operations)
        {
            while (operations.Any())
            {
                var operationName = operations.Pop();
                statements.Push(CreateOperation(operationName, statements));
            }

            if(statements.Count() == 1)
                return statements.Pop();

            throw new Exception("Too few operators");
        }

        private static INode CreateOperation(string operationName, Stack<INode> statements)
        {
            switch (operationName)
            {
                case "+":
                    return new Sum(statements.Pop(), statements.Pop());
            }

            throw  new Exception("Wrong operation");
        }

        private static String[] operators = new[] { "+", "-", "*", "/"};

        private static bool IsOperation(String expression, int i)
        {
            return operators.Any(op => 
                                String.Equals(expression.Substring(i, op.Length), op));
        }

        private static String GetOperation(String expression, ref int i)
        {
            var position = i;
            var result = operators.First(op => String.Equals(expression.Substring(position, op.Length), op));

            i += result.Length;

            return result;
        }

        private static Constant GetNumber(String expression, ref int i)
        {
            int position = i;
            while (i < expression.Length && 
                    (Char.IsDigit(expression[i]) || expression[i] == '.'))
                i++;

            var number = expression.Substring(position, i - position);

            return new Constant(double.Parse(number, CultureInfo.InvariantCulture));
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
