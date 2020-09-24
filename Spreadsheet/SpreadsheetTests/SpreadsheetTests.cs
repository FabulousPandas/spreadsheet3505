/// Author: Khris Thammavong
/// A set of tests to test the Spreadsheet class

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;

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

        

    }
}
