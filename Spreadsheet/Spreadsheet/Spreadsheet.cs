/// Author: Khris Thammavong

using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SS
{
    /// <summary>
    /// Represents the state of a simple spreadsheet that consists of an infinite number of named cells
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Dictionary representing the mapping of a cell name to a cell
        /// </summary>
        private Dictionary<string, Cell> cells;
        
        /// <summary>
        /// A DependencyGraph to represent the dependencies between cells
        /// </summary>
        private DependencyGraph graph;

        /// <summary>
        /// Creates a new spreadsheet
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
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
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            if (name == null || !IsVar(name))
                throw new InvalidNameException();

            return null;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public override IList<string> SetCellContents(string name, double number)
        {
            if (name == null || !IsVar(name))
                throw new InvalidNameException();


            throw new NotImplementedException();
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public override IList<string> SetCellContents(string name, string text)
        {
            if (text == null)
                throw new ArgumentNullException();

            if (name == null || !IsVar(name))
                throw new InvalidNameException();



            throw new NotImplementedException();
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public override IList<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException();

            if (name == null || !IsVar(name))
                throw new InvalidNameException();



            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Represents a cell within a spreadsheet
        /// </summary>
        private class Cell
        {
            /// <summary>
            /// Property containing the contents of the cell
            /// </summary>
            public object Contents
            {
                get;
                set;
            }

            /// <summary>
            /// Creates a new sell with the contents of o
            /// </summary>
            /// <param name="o"></param>
            public Cell(object o)
            {
                Contents = 0;
            }
        }
    }
}
