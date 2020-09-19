/// Testing class for testing Formula
/// Author: Khris Thammavong

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        [TestMethod]
        public void TestSimpleAddNoVariables()
        {
            Formula f = new Formula("1 + 1");
            Assert.AreEqual(2.0, f.Evaluate(s => 0));
        }
        
        [TestMethod]
        public void TestSimpleAddNoVariables2()
        {
            Formula f = new Formula("6.7 + 2.3");
            Assert.AreEqual(9.0, (double) f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void TestSimpleSubtractNoVariables()
        {
            Formula f = new Formula("5 - 3");
            Assert.AreEqual(2.0, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void TestSimpleDivideNoVariables()
        {
            Formula f = new Formula("5 / (3 + 2)");
            Assert.AreEqual(1.0, f.Evaluate(s => 0));
        }

        [TestMethod]
        public void TestOnlyOneValue()
        {
            Formula f = new Formula("6.7");
            Assert.AreEqual(6.7, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void TestOnlyOneVariable()
        {
            Formula f = new Formula("a1");
            Assert.AreEqual(5.0, (double) f.Evaluate(SimpleLookup), 1e-9);
        }

            [TestMethod]
        public void TestManyParentheses()
        {
            Formula f = new Formula("((((((((5.346436))))))))");
            Assert.AreEqual(5.346436, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void TestBiggerEquation()
        {
            Formula f = new Formula("45.0 - 19.0 / (20 + 3.5) * ((10.0 + 6) / 3.1) - (10.543 + 9e0) * (2.54e1 / 5) - (10.2 + 9.8 * (11.4 - 10))");
            double expected = 45.0 - 19.0 / (20 + 3.5) * ((10.0 + 6) / 3.1) - (10.543 + 9e0) * (2.54e1 / 5) - (10.2 + 9.8 * (11.4 - 10));
            Assert.AreEqual(expected, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void TestBiggerEquation2()
        {
            Formula f = new Formula("21.8 - 34e2 / 348 + (23 - (132 * 24)) + (8.45 + 2.3e-1)");
            double expected = 21.8 - 34e2 / 348 + (23 - (132 * 24)) + (8.45 + 2.3e-1);
            Assert.AreEqual(expected, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void TestDivideByZero()
        {
            Formula f = new Formula("2 / 0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
            Assert.AreEqual(((FormulaError)f.Evaluate(s => 0)).Reason, "Divide by 0");
        }

        [TestMethod]
        public void TestDivideByZero2()
        {
            Formula f = new Formula("2 / (0 + 0)");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
            Assert.AreEqual(((FormulaError)f.Evaluate(s => 0)).Reason, "Divide by 0");
        }

        [TestMethod]
        public void TestDivideByZeroVariable()
        {
            Formula f = new Formula("2 / a1");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
            Assert.AreEqual(((FormulaError)f.Evaluate(s => 0)).Reason, "Divide by 0");
        }

        [TestMethod]
        public void TestVariableLookupFailed()
        {
            Formula f = new Formula("a1 + g3");
            Assert.IsInstanceOfType(f.Evaluate(SimpleLookup), typeof(FormulaError));
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmptyFormula()
        {
            Formula f = new Formula("");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOnlyWhitespace()
        {
            Formula f = new Formula("         ");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidCharacters()
        {
            Formula f = new Formula("? + 23");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidCharacters2()
        {
            Formula f = new Formula("a_23 - 643.2 - 3#4");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNoOperatorBetweenNumbers()
        {
            Formula f = new Formula("1.4 5.6 3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOnlyOperators()
        {
            Formula f = new Formula("+ - /");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestTwoConsecutiveOperators()
        {
            Formula f = new Formula("2.4 + / 4.12");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOperatorThenClosedParenthesis()
        {
            Formula f = new Formula("(1 + ) 2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestUnmatchedLeftParenthesis()
        {
            Formula f = new Formula("(1 + 4 - 3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestUnmatchedRightParenthesis()
        {
            Formula f = new Formula("5.8 - 34) * 2.24245");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNothingWihinParentheses()
        {
            Formula f = new Formula("() + 3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNoOperatorAfterParenthesis()
        {
            Formula f = new Formula("(1 + 3) 3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConsecutiveParentheses()
        {
            Formula f = new Formula("(1 + 3) (3 + 4)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestStartWithOperator()
        {
            Formula f = new Formula("/ 3 + 1.57");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestStartWithClosedParenthesis()
        {
            Formula f = new Formula(") 23 + 43");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestUnbalancedParentheses()
        {
            Formula f = new Formula("((((1 + 3) - 5) * 3)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOperatorAtEnd()
        {
            Formula f = new Formula("6 - 4.3 / 23e-1 + ");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidVariable()
        {
            Formula f = new Formula("1a + 3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFailValidator()
        {
            Formula f = new Formula("a + 3", s => s.ToUpper(), LowercaseValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestVariableInvalidAfterNormalizing()
        {
            Formula f = new Formula("a + 3", s => "1n", LowercaseValidator);
        }

        [TestMethod]
        public void TestGetVariables()
        {
            Formula f = new Formula("(f1 + 0.45) - _3 / e2 * (26 - ee_2 / __e_23e)");
            HashSet<string> variables = new HashSet<string>() {"f1", "_3", "e2", "ee_2", "__e_23e"};
            foreach(string s in f.GetVariables())
            {
                Assert.IsTrue(variables.Contains(s));
            }
            
        }

        [TestMethod]
        public void TestGetVariablesNoVariables()
        {
            Formula f = new Formula("1.0 + 5 / 3 - (2e1 + 0.45134)");
            Assert.IsTrue(f.GetVariables().Count() == 0);
        }

        [TestMethod]
        public void TestToStringNoVariables()
        {
            Formula f = new Formula("1.0 + 5 / 3 - (2e1 + 0.45134)");
            Assert.AreEqual("1+5/3-(20+0.45134)", f.ToString());
        }

        [TestMethod]
        public void TestToStringWithVariables()
        {
            Formula f = new Formula("4.5e3 + a2 / 6.624 - (e_1e + 0.45134e2)");
            Assert.AreEqual("4500+a2/6.624-(e_1e+45.134)", f.ToString());
        }

        [TestMethod]
        public void TestToStringWithNormalizedVariables()
        {
            Formula f = new Formula("1.0 + a2 / 3 - (e_1e + 0.45134)", s => s.ToUpper(), s => true);
            Assert.AreEqual("1+A2/3-(E_1E+0.45134)", f.ToString());
        }

        /// Testing equals method (taken from comment on equals method)
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        [TestMethod]
        public void TestEquals()
        {
            Formula f1 = new Formula("x1+y2", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("X1  +  Y2");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod]
        public void TestEquals2()
        {
            Formula f1 = new Formula("x1+y2");
            Formula f2 = new Formula("X1+Y2");
            Assert.IsFalse(f1.Equals(f2));
        }

        [TestMethod]
        public void TestEquals3()
        {
            Formula f1 = new Formula("x1+y2");
            Formula f2 = new Formula("y2+x1");
            Assert.IsFalse(f1.Equals(f2));
        }

        [TestMethod]
        public void TestEquals4()
        {
            Formula f1 = new Formula("2.0 + x7");
            Formula f2 = new Formula("2.000 + x7");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod]
        public void TestEqualsWrongType()
        {
            Formula f1 = new Formula("2.0 + x7");
            Stack<string> stack = new Stack<string>();
            Assert.IsFalse(f1.Equals(stack));
        }

        [TestMethod]
        public void TestOperatorEquals()
        {
            Formula f1 = new Formula("x1+y2", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("X1  +  Y2");
            Assert.IsTrue(f1 == f2);
            Assert.IsFalse(f1 != f2);
        }

        [TestMethod]
        public void TestOperatorEquals2()
        {
            Formula f1 = new Formula("x1+y2");
            Formula f2 = new Formula("X1+Y2");
            Assert.IsFalse(f1 == f2);
            Assert.IsTrue(f1 != f2);
        }

        [TestMethod]
        public void TestOperatorEquals3()
        {
            Formula f1 = new Formula("x1+y2");
            Formula f2 = new Formula("y2+x1");
            Assert.IsFalse(f1 == f2);
            Assert.IsTrue(f1 != f2);
        }

        [TestMethod]
        public void TestOperatorEquals4()
        {
            Formula f1 = new Formula("2.0 + x7");
            Formula f2 = new Formula("2.000 + x7");
            Assert.IsTrue(f1 == f2);
            Assert.IsFalse(f1 != f2);
        }

        [TestMethod]
        public void TestEqualsNull()
        {
            Formula f1 = new Formula("5.6 + 4.4");
            Formula f2 = null;
            Assert.IsFalse(f1.Equals(f2));
        }
        [TestMethod]
        public void TestOperatorEqualsNull()
        {
            Formula f1 = null;
            Formula f2 = null;
            Formula f3 = new Formula("5.6 + 4.4");
            Assert.IsTrue(f1 == f2);
            Assert.IsFalse(f1 == f3);
            Assert.IsTrue(f2 == f1);
            Assert.IsFalse(f2 == f3);
            Assert.IsFalse(f3 == f1);
            Assert.IsFalse(f3 == f2);
        }

        [TestMethod]
        public void TestOperatorNotEqualsNull()
        {
            Formula f1 = null;
            Formula f2 = null;
            Formula f3 = new Formula("5.6 + 4.4");
            Assert.IsFalse(f1 != f2);
            Assert.IsTrue(f1 != f3);
            Assert.IsFalse(f2 != f1);
            Assert.IsTrue(f2 != f3);
            Assert.IsTrue(f3 != f1);
            Assert.IsTrue(f3 != f2);
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            Formula f1 = new Formula("2.0 + x7");
            Formula f2 = new Formula("2.000 + x7");
            Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
        }


        /// <summary>
        /// Simple Lookup Method with only two variables
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        private static double SimpleLookup(string var)
        {
            if (var == "a1")
                return 5.0;
            else if (var == "b4")
                return 12.0;
            else
                throw new ArgumentException();
        }

        private bool LowercaseValidator(string s)
        {
            foreach (char c in s)
                if (char.IsUpper(c))
                    return false;
            return true;
        }

    }
}
