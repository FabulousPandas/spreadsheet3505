// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

/// Author: Khris Thammavong

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private List<string> tokens;
        private HashSet<string> variables;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(string formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            tokens = GetTokens(formula).ToList();
            variables = new HashSet<string>();

            if (tokens.Count == 0)
                throw new FormulaFormatException("Formula cannot be empty");

            string operatorPattern = @"[\+\-*/\)\(]";

            if(!IsVar(tokens[0]) && !double.TryParse(tokens[0], out _) && !(tokens[0] == "(")) // starting token rule
                throw new FormulaFormatException("Beginning of formula is invalid");
            if (!IsVar(tokens[tokens.Count - 1]) && !double.TryParse(tokens[tokens.Count - 1], out _) && !(tokens[tokens.Count-1] == ")")) // ending token rule
                throw new FormulaFormatException("Ending of formula is invalid");

            int leftParenthesisCount = 0;
            int rightParenthesisCount = 0;
            bool valOrParenthesis = false; // whether a value (number or variable) or a opening parenthesis is needed next for the formula to be syntactically correct
            bool opOrParenthesis = false; // whether an operator or a closing parenthesis is needed next for the formula to be syntactically correct

            for(int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i];
                if (rightParenthesisCount > leftParenthesisCount)
                    throw new FormulaFormatException("Unbalanced parentheses");

                if (IsVar(token))
                {
                    valOrParenthesis = false;

                    if (opOrParenthesis)
                        throw new FormulaFormatException("Syntax error");

                    opOrParenthesis = true;

                    if (!IsVar(token))
                        throw new FormulaFormatException("Variable is invalid after normalizing");
                    else
                    {
                        token = normalize(token);
                        variables.Add(token);
                    }

                    if (!isValid(token))
                        throw new FormulaFormatException("Variable does not pass validator");

                }
                else if(double.TryParse(token, out _))
                {
                    valOrParenthesis = false;

                    if (opOrParenthesis)
                        throw new FormulaFormatException("Syntax error");

                    opOrParenthesis = true;
                }
                else if(Regex.IsMatch(token, operatorPattern))
                {
                    if (valOrParenthesis && token != "(")
                        throw new FormulaFormatException("Syntax error");

                    if(token == "(")
                    {
                        if (opOrParenthesis)
                            throw new FormulaFormatException("Syntax error");

                        leftParenthesisCount++;
                        valOrParenthesis = true;
                    }
                    else if(token == ")")
                    {
                        rightParenthesisCount++;
                        opOrParenthesis = true;
                    }    
                    else
                    {
                        opOrParenthesis = false;
                        valOrParenthesis = true;
                    }
                }
                else
                {
                    throw new FormulaFormatException("Invalid symbol found in formula");
                }
            }

            if (leftParenthesisCount != rightParenthesisCount)
                throw new FormulaFormatException("Unbalanced parentheses");
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> values = new Stack<double>();
            Stack<char> operators = new Stack<char>();

            foreach (string s in tokens)
            {
                if (double.TryParse(s, out double value)) //case for numbers
                {
                    if(!ValueCase(value, values, operators))
                        return new FormulaError("Divide by 0");
                }
                else if (IsVar(s)) //case for variables
                {
                    try
                    {
                        if (!ValueCase(lookup(s), values, operators))
                            return new FormulaError("Divide by 0");
                    }
                    catch(ArgumentException)
                    {
                        return new FormulaError("Could not find value for a variable");
                    }
                }
                else //case for operators and other invalid symbols/strings
                {
                    if (!OperatorCase(s[0], values, operators))
                        return new FormulaError("Divide by 0");
                }
            }

            if (operators.Count == 0)
            {
                return values.Pop();
            }
            else
            {
                double val2 = values.Pop();
                double val1 = values.Pop();
                return SimpleEvaluate(val1, val2, operators.Pop());
            }
        }

        /// <summary>
        /// Returns true if a given string is in the valid variable form
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        private static bool IsVar(string var)
        {
            string pattern = @"^[a-zA-Z_][a-zA-Z_\d]*$";
            return Regex.IsMatch(var, pattern);
        }

        /// <summary>
        /// Given two values and an operator it will evaluate the expression of value1 operator value2.
        /// If the operator is not a valid operator (+-/*) or the expression tries to divide by 0 the function returns a FormulaError
        /// </summary>
        /// <param name="val1">First value</param>
        /// <param name="val2">Second value</param>
        /// <param name="op">Operator</param>
        /// <returns></returns>
        static object SimpleEvaluate(double val1, double val2, char op)
        {
            switch (op)
            {
                case '+':
                    return val1 + val2;
                case '-':
                    return val1 - val2;
                case '*':
                    return val1 * val2;
                default:
                    if (val2 == 0)
                        return new FormulaError("Divide by 0");
                    return val1 / val2;
            }
        }

        /// <summary>
        /// Given a values stack and an operators stack it will evaluate the infix expression composed of the two top values and the top operator and push it back into the values stack.
        /// If the operator is not a valid operator (+-/*) or the expression tries to divide by 0 the function throws an ArgumentException().
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operators"></param>
        /// <returns></returns>
        static bool SimpleEvaluate(Stack<double> values, Stack<char> operators)
        {
            double val2 = values.Pop();
            double val1 = values.Pop();
            char op = operators.Pop();
            if (typeof(FormulaError).IsInstanceOfType(SimpleEvaluate(val1, val2, op)))
            {
                return false;
            }
            else
            {
                values.Push((double)SimpleEvaluate(val1, val2, op));
                return true;
            }
        }

        /// <summary>
        /// Helper method for handling the value cases of evaluating infix expressions
        /// Returns true if there was no error, returns false otherwise
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <param name="operators"></param>
        static bool ValueCase(double value, Stack<double> values, Stack<char> operators)
        {
            if (operators.IsOnTop<char>('*') || operators.IsOnTop<char>('/'))
            {
                double otherValue = values.Pop();
                char op = operators.Pop();
                if (typeof(FormulaError).IsInstanceOfType(SimpleEvaluate(otherValue, value, op)))
                {
                    return false;
                }
                else
                {
                    values.Push((double)SimpleEvaluate(otherValue, value, op));
                    return true;
                }
            }
            else
            {
                values.Push(value);
                return true;
            }
        }

        /// <summary>
        /// Helper method for handling the operator cases of evaluating infix expressions
        /// Returns true if there was no error, returns false otherwise
        /// </summary>
        /// <param name="op"></param>
        /// <param name="values"></param>
        /// <param name="operators"></param>
        static bool OperatorCase(char op, Stack<double> values, Stack<char> operators)
        {
            bool noError = true;
            switch (op)
            {
                case '+':
                case '-':
                    if (operators.IsOnTop<char>('+') || operators.IsOnTop<char>('-'))
                    {
                        noError = SimpleEvaluate(values, operators);
                    }
                    operators.Push(op);
                    return noError;
                case '*':
                case '/':
                case '(':
                    operators.Push(op);
                    return true;
                default:
                    if (operators.IsOnTop<char>('+') || operators.IsOnTop<char>('-'))
                    {
                        noError = SimpleEvaluate(values, operators);
                    }
                    if (operators.IsOnTop<char>('('))
                        operators.Pop();

                    if (!noError)
                        return false;

                    if (operators.IsOnTop<char>('*') || operators.IsOnTop<char>('/'))
                    {
                        noError = SimpleEvaluate(values, operators);
                    }

                    return noError;
            }
        }
    

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<string> GetVariables()
        {
            return new HashSet<string>(variables);
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return null;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(string formula)
        {
            // Patterns for individual tokens
            string lpPattern = @"\(";
            string rpPattern = @"\)";
            string opPattern = @"[\+\-*/]";
            string varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            string spacePattern = @"\s+";

            // Overall pattern
            string pattern = string.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (string s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(string reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }

    /// <summary>
    /// Extension class for adding the IsOnTop method
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

