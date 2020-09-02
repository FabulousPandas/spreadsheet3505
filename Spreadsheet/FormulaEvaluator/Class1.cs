using System;

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
        /// Evaluates the given integer infix expression and returns the result
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="variableEvaluator"></param>
        /// <returns></returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {


            return 0;
        }

    }
}
