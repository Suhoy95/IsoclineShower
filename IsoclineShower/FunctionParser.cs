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

        public static Func<double, double> CreateFX(String experssion)
        {
            var node = Parse(experssion);
            return (double x) =>
            {
                var dict = new Dictionary<String, double>() { { "x", x } };
                return node.Value(dict);
            };
        }

        public static Func<double, double, double> CreateFXY(String experssion)
        {
            var node = Parse(experssion);
            return (double x, double y) =>
                {
                    var dict = new Dictionary<String, double>() { { "x", x }, { "y", y } };
                    return node.Value(dict);
                };
        }

        public static INode Parse(String expression)
        {
            bool isUnaryMinus = true;
            var statements = new Stack<INode>();
            var operations = new Stack<String>();

            for (var i = 0; i < expression.Length;)
            {
                if (IsOperation(expression, i))
                {
                    var operation = GetOperation(expression, ref i);
                    if ((operation == "-") && isUnaryMinus)
                    {
                        operations.Push("-?");
                        continue;
                    }
                    while (operations.Any() && Priority[operations.Peek()] >= Priority[operation])
                    {
                        var operationName = operations.Pop();
                        statements.Push(CreateOperation(operationName, statements));
                    }
                    operations.Push(operation);
                }
                else if (Char.IsDigit(expression[i]))
                {
                    statements.Push(GetNumber(expression, ref i));
                }
                else if (Char.IsLetter(expression[i]))
                {
                    if (IsConstant(expression, i))
                    {
                        var constantName = GetConst(expression, ref i);
                        statements.Push(CreateConstant(constantName));
                    }
                    else
                    {
                        statements.Push(GetVariable(expression, ref i));
                    }
                }
                else if (expression[i] == '(')
                {
                    operations.Push("(");
                    isUnaryMinus = true;
                    i++;
                    continue;
                }
                else if (expression[i] == ')')
                {
                    while (operations.Peek() != "(")
                    {
                        var operationName = operations.Pop();
                        statements.Push(CreateOperation(operationName, statements));
                    }
                    operations.Pop();
                    i++;
                }
                else if (expression[i] == ',')
                {
                    while (operations.Peek() != "(")
                    {
                        var operationName = operations.Pop();
                        statements.Push(CreateOperation(operationName, statements));
                    }
                    i++;
                }
                else
                    i++;

                isUnaryMinus = false;
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

        private static INode CreateConstant(string constantName)
        {
            switch (constantName)
            {
                case "e":
                    return new Constant(Math.E);
                case "pi":
                    return new Constant(Math.PI);
            }
            throw new Exception("Wrong constant");
        }

        private static INode CreateOperation(string operationName, Stack<INode> statements)
        {
            switch (operationName)
            {
                case "+":
                    return new Sum(statements.Pop(), statements.Pop());
                case "*":
                    return new Multiplication(statements.Pop(), statements.Pop());
                case "/":
                    var divider = statements.Pop();
                    return new Segmentation(statements.Pop(), divider);
                case "-":
                    var subtrhend = statements.Pop();
                    return new Subtraction(statements.Pop(), subtrhend);
                case "-?":
                    return new UnaryMinus(statements.Pop());
                case "sin":
                    return new Sin(statements.Pop());
                case "cos":
                    return new Cos(statements.Pop());
                case "tg":
                    return new Tg(statements.Pop());
                case "asin":
                    return new Asin(statements.Pop());
                case "acos":
                    return new Acos(statements.Pop());
                case "atg":
                    return new Atg(statements.Pop());
                case "ctg":
                    return new Ctg(statements.Pop());
                case "actg":
                    return new Actg(statements.Pop());
                case "^":
                    var degree = statements.Pop();
                    return new Pow(statements.Pop(), degree);
                case "log":
                    var newBase = statements.Pop();
                    return new Log(statements.Pop(), newBase);
            }

            throw  new Exception("Wrong operation");
        }

        private static String[] consts = { "e", "pi" };

        private static String[] operators = { "+", "-", "*", "/", 
                                              "sin", "cos", "tg", "ctg",
                                              "asin", "acos", "atg", "actg",  
                                              "^", "log"};

        private static bool IsConstant(String expression, int i)
        {
            return consts.Any(cnst => i + cnst.Length <= expression.Length &&
                                String.Equals(expression.Substring(i, cnst.Length), cnst));
        }
        private static bool IsOperation(String expression, int i)
        {
            return operators.Any(op => i + op.Length <= expression.Length &&
                                String.Equals(expression.Substring(i, op.Length), op));
        }

        private static String GetConst(String expression, ref int i)
        {
            var position = i;
            var result = consts.First(cnst => position + cnst.Length <= expression.Length && 
                                              String.Equals(expression.Substring(position, cnst.Length), cnst));

            i += result.Length;

            return result;
        }

        private static String GetOperation(String expression, ref int i)
        {
            var position = i;
            var result = operators.First(op => position + op.Length <= expression.Length && String.Equals(expression.Substring(position, op.Length), op));
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

        private static Dictionary<string, int> Priority = 
            new Dictionary<string, int> { {"+", 0}, {"*", 1}, {"-", 0}, {"/", 1}, 
                                          {"(", -1}, {"-?", 3}, {"^", 2}, {"sin", 4},
                                          {"cos", 4},{"tg", 4}, {"ctg", 4}, {"asin", 4},
                                          {"acos", 4}, {"atg", 4}, {"actg", 4}, {"log", 4} };
    }
}