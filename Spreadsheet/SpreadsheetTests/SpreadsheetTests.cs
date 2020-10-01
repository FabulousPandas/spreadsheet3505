/// Author: Khris Thammavong
/// A set of tests to test the Spreadsheet class

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Linq;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        /// ------------------
        /// Exception Handling
        /// ------------------

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsInvalid()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("46");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDoubleNameNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "2.0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDoubleNameInvalid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("*%^", "4.0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsStringNameNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "example");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsStringNameInvalid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("23547", "example");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContentsStringTextNull()
        {
            Spreadsheet s = new Spreadsheet();
            string text = null;
            s.SetContentsOfCell("valid123", text);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsFormulaNameNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "=1 + 1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsFormulaNameInvalid()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("1 + 1");
            s.SetContentsOfCell("&as&^", "=1 + 1");
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CheckCircularDependency()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a1 + 3");
        }

        /// -------------------------
        /// Empty spreadsheet testing
        /// -------------------------

        [TestMethod]
        public void TestEmptyGetCellContents()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("a1"));
        }

        [TestMethod]
        public void TestEmptyGetNamesOfAllNonemptyCells()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual(0, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void TestSettingCellEmpty()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "");
            Assert.AreEqual("", s.GetCellContents("a1"));
            Assert.AreEqual(0, s.GetNamesOfAllNonemptyCells().Count());
        }

        /// ---------------------------------------
        /// Setting and immediately clearing a cell
        /// ---------------------------------------

        [TestMethod]
        public void AddAndRemoveDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "2.0");
            s.SetContentsOfCell("a1", "");
            Assert.AreEqual("", s.GetCellContents("a1"));
            Assert.AreEqual(0, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddAndRemoveString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "words");
            s.SetContentsOfCell("a1", "");
            Assert.AreEqual("", s.GetCellContents("a1"));
            Assert.AreEqual(0, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddAndRemoveFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=6.7 + 93.135");
            s.SetContentsOfCell("a1", "");
            Assert.AreEqual("", s.GetCellContents("a1"));
            Assert.AreEqual(0, s.GetNamesOfAllNonemptyCells().Count());
        }

        /// --------------------
        /// Checking correctness
        /// --------------------
        
        [TestMethod]
        public void AddOneDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "2.0");
            Assert.AreEqual(2.0, (double) s.GetCellContents("a1"), 1e-9);
            Assert.AreEqual(1, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddOneString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("_7", "bunch of random letters");
            Assert.AreEqual("bunch of random letters", s.GetCellContents("_7"));
            Assert.AreEqual(1, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddOneFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("5 + 7.8");
            s.SetContentsOfCell("b_3", "=5 + 7.8");
            Assert.AreEqual(f, s.GetCellContents("b_3"));
            Assert.AreEqual(1, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddMultipleDoubles()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "2.0");
            s.SetContentsOfCell("b5", "7.4");
            s.SetContentsOfCell("__", "12.1");
            s.SetContentsOfCell("b5", "5.4");
            Assert.AreEqual(2.0, (double)s.GetCellContents("a1"), 1e-9);
            Assert.AreEqual(5.4, (double)s.GetCellContents("b5"), 1e-9);
            Assert.AreEqual(12.1, (double)s.GetCellContents("__"), 1e-9);
            Assert.AreEqual(3, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddMultipleStrings()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "ajhd");
            s.SetContentsOfCell("b5", "483 + 3");
            s.SetContentsOfCell("__", "aeiou8^%$#");
            s.SetContentsOfCell("b5", "23/6 + 34.3");
            Assert.AreEqual("ajhd", s.GetCellContents("a1"));
            Assert.AreEqual("23/6 + 34.3", s.GetCellContents("b5"));
            Assert.AreEqual("aeiou8^%$#", s.GetCellContents("__"));
            Assert.AreEqual(3, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddMultipleFormulas()
        {
            Spreadsheet s = new Spreadsheet();
            Formula a1 = new Formula("1 + b5");
            Formula __ = new Formula("2.3 * b5 + a1 - 3.5");
            Formula newb5 = new Formula("7.2 + 3.2");
            s.SetContentsOfCell("a1", "=1 + b5");
            s.SetContentsOfCell("b5", "=6.5 + 3.6");
            s.SetContentsOfCell("__", "=2.3 * b5 + a1 - 3.5");
            s.SetContentsOfCell("b5", "=7.2 + 3.2");
            Assert.AreEqual(a1, s.GetCellContents("a1"));
            Assert.AreEqual(newb5, s.GetCellContents("b5"));
            Assert.AreEqual(__, s.GetCellContents("__"));
            Assert.AreEqual(3, s.GetNamesOfAllNonemptyCells().Count());
        }

        /// <summary>
        /// Making sure that when removing a formula it doesn't create a "phantom" circular dependency
        /// </summary>
        [TestMethod]
        public void AddMultipleFormulas2()
        {
            Spreadsheet s = new Spreadsheet();
            Formula c12 = new Formula("m9 * 2.3");
            Formula m9 = new Formula("a6 - 3.5");
            s.SetContentsOfCell("a6", "=1 + c12");
            s.SetContentsOfCell("a6", "");
            s.SetContentsOfCell("c12", "=m9 * 2.3");
            s.SetContentsOfCell("m9", "=a6 - 3.5");
            Assert.AreEqual("", s.GetCellContents("a6"));
            Assert.AreEqual(c12, s.GetCellContents("c12"));
            Assert.AreEqual(m9, s.GetCellContents("m9"));
            Assert.AreEqual(2, s.GetNamesOfAllNonemptyCells().Count());
        }


    }
}
