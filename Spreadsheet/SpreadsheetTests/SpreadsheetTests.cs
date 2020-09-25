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
            s.SetCellContents(null, 2.0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDoubleNameInvalid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("*%^", 4.0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsStringNameNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, "example");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsStringNameInvalid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("23547", "example");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContentsStringTextNull()
        {
            Spreadsheet s = new Spreadsheet();
            string text = null;
            s.SetCellContents("valid123", text);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsFormulaNameNull()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("1 + 1");
            s.SetCellContents(null, f);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsFormulaNameInvalid()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("1 + 1");
            s.SetCellContents("&as&^", f);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContentsFormulaFormulaNull()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = null;
            s.SetCellContents("a5", f);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CheckCircularDependency()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("a1 + 3");
            s.SetCellContents("a1", f);
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

        /// ---------------------------------------
        /// Setting and immediately clearing a cell
        /// ---------------------------------------

        [TestMethod]
        public void AddAndRemoveDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("a1", 2.0);
            s.SetCellContents("a1", "");
            Assert.AreEqual("", s.GetCellContents("a1"));
            Assert.AreEqual(0, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddAndRemoveString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("a1", "words");
            s.SetCellContents("a1", "");
            Assert.AreEqual("", s.GetCellContents("a1"));
            Assert.AreEqual(0, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddAndRemoveFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("6.7 + 93.135");
            s.SetCellContents("a1", f);
            s.SetCellContents("a1", "");
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
            s.SetCellContents("a1", 2.0);
            Assert.AreEqual(2.0, (double) s.GetCellContents("a1"), 1e-9);
            Assert.AreEqual(1, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddOneString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("_7", "bunch of random letters");
            Assert.AreEqual("bunch of random letters", s.GetCellContents("_7"));
            Assert.AreEqual(1, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddOneFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("5 + 7.8");
            s.SetCellContents("b_3", f);
            Assert.AreEqual(f, s.GetCellContents("b_3"));
            Assert.AreEqual(1, s.GetNamesOfAllNonemptyCells().Count());
        }


    }
}
