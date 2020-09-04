using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// Evaluates integer infix expressions written using standard infix notation
    /// </summary>
    public static class Evaluator
    {
        //Delegate for describing how the values for variables are looked up
        public delegate int Lookup(String v);

        /// <summary>
        /// Evaluates the given integer infix expression and returns the result.
        /// Throws an ArgumentException if the given expression is invalid.
        /// </summary>
        /// <param name="exp">The expression to be evaluated</param>
        /// <param name="variableEvaluator">The method used to assign values to variables</param>
        /// <returns></returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            string[] tokens = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            Stack<int> values = new Stack<int>();
            Stack<char> operators = new Stack<char>();

            foreach(string s in tokens)
            {
                string trimmed = s.Trim(); //ignoring whitespace
                if (trimmed.Length == 0) 
                    continue;

                int value;

                if(int.TryParse(trimmed, out value))
                {
                    ValueCase(value, values, operators);
                }
                else if(IsVar(trimmed))
                {
                    ValueCase(variableEvaluator(trimmed), values, operators);
                }
                else
                {
                    OperatorCase(trimmed[0], values, operators);
                }
            }

            if(operators.Count == 0)
            {
                if (values.Count == 1)
                    return values.Pop();
                else
                    throw new ArgumentException();
            }
            else
            {
                if (operators.IsOnTop<char>('+') || operators.IsOnTop<char>('-'))
                {
                    if (values.Count != 2)
                        throw new ArgumentException();
                    int val2 = values.Pop();
                    int val1 = values.Pop();
                    return SimpleEvaluate(val1, val2, operators.Pop());
                }
                else
                    throw new ArgumentException();
            }
        }
        /// <summary>
        /// Returns true if a given string is in the valid variable form
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        static bool IsVar(string var)
        {
            string pattern = "^[a-zA-Z]+[0-9]+$";
            return Regex.IsMatch(var, pattern);
        }

        /// <summary>
        /// Given two values and an operator it will evaluate the expression of value1 operator value2.
        /// If the operator is not a valid operator (+-/*) or the expression tries to divide by 0 the function throws an ArgumentException().
        /// </summary>
        /// <param name="val1">First value</param>
        /// <param name="val2">Second value</param>
        /// <param name="op">Operator</param>
        /// <returns></returns>
        static int SimpleEvaluate(int val1, int val2, char op)
        {
            switch(op)
            {
                case '+':
                    return val1 + val2;
                case '-':
                    return val1 - val2;
                case '*':
                    return val1 * val2;
                case '/':
                    if (val2 == 0)
                        throw new ArgumentException();
                    return val1 / val2;
                default:
                    throw new ArgumentException();
            }
        }
        /// <summary>
        /// Helper method for handling the value cases of evaluating infix expressions
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <param name="operators"></param>
        static void ValueCase(int value, Stack<int> values, Stack<char> operators)
        {
            if (operators.IsOnTop<char>('*') || operators.IsOnTop<char>('/'))
            {
                if (values.Count == 0)
                    throw new ArgumentException("");

                values.Push(SimpleEvaluate(value, values.Pop(), operators.Pop()));
            }
            else
                values.Push(value);
        }

        static void OperatorCase(char op, Stack<int> values, Stack<char> operators)
        {
            switch (op)
            {
                case '+':
                case '-':
                    if (operators.IsOnTop<char>('+') || operators.IsOnTop<char>('-'))
                    {
                        if (values.Count < 2)
                            throw new ArgumentException();
                        int val2 = values.Pop();
                        int val1 = values.Pop();
                        values.Push(SimpleEvaluate(val1, val2, operators.Pop()));
                    }
                    operators.Push(op);
                    break;
                case '*':
                case '/':
                case '(':
                    operators.Push(op);
                    break;
                case ')':
                    if (operators.IsOnTop<char>('+') || operators.IsOnTop<char>('-'))
                    {
                        if (values.Count < 2)
                            throw new ArgumentException();
                        int val2 = values.Pop();
                        int val1 = values.Pop();
                        values.Push(SimpleEvaluate(val1, val2, operators.Pop()));
                    }
                    if (operators.IsOnTop<char>('('))
                        operators.Pop();
                    else
                        throw new ArgumentException();
                    if (operators.IsOnTop<char>('*') || operators.IsOnTop<char>('/'))
                    {
                        if (values.Count < 2)
                            throw new ArgumentException("");

                        int val2 = values.Pop();
                        int val1 = values.Pop();
                        values.Push(SimpleEvaluate(val1, val2, operators.Pop()));
                    }
                    break;
            }
        }
    }
    /// <summary>
    /// Extension class for adding the IsOnTop method to the Stack class
    /// </summary>
    static class StackExtension
    {
        /// <summary>
        /// Returns whether or not a given value is on the top of the stack the method is called on.
        /// Returns false if the stack is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsOnTop<T>(this Stack<T> s, T c)
        {
            if (s.Count < 1)
                return false;
            return s.Peek().Equals(c);
        }
    }
}
