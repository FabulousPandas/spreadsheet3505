/// Testing class for testing Formula
/// Author: Khris Thammavong

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        [TestMethod]
        public void TestSimpleNoVariables()
        {
            Formula f = new Formula("1 + 1");
            Assert.AreEqual(2.0, f.Evaluate(s => 0));
        }
        
        [TestMethod]
        public void TestSimpleNoVariables2()
        {
            Formula f = new Formula("6.7 + 2.3");
            Assert.AreEqual(9.0, (double) f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void TestOnlyOneToken()
        {
            Formula f = new Formula("6.7");
            Assert.AreEqual(6.7, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void TestManyParentheses()
        {
            Formula f = new Formula("((((((((5.346436))))))))");
            Assert.AreEqual(5.346436, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void TestSimpleWithVariable()
        {
            Formula f = new Formula("45 - 19 / (20 + 3) * ((10 + 6) / 3) - (10 + 9) * (25 / 5) - (10 + 10 * (11 - 10))");
            double expected = 45 - 19 / (20 + 3) * ((10 + 6) / 3) - (10 + 9) * (25 / 5) - (10 + 10 * (11 - 10));
            Assert.AreEqual(expected, (double)f.Evaluate(s => 0), 1e-9);
        }

        [TestMethod]
        public void TestDivideByZero()
        {
            Formula f = new Formula("2 / 0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
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


        private static int SimpleLookup(string var)
        {
            if (var == "a1")
                return 5;
            else if (var == "b4")
                return 12;
            else
                throw new ArgumentException();
        }

        private string UppercaseNormalizer(string s)
        {
            return s.ToUpper();
        }

        private string LowercaseNormalizer(string s)
        {
            return s.ToLower();
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
