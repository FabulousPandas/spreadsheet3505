﻿/// Author: Khris Thammavong

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
        /// Dictionary representing the mapping of a cell name to a cell.
        /// Only represents non-empty cells.
        /// </summary>
        private Dictionary<string, Cell> cells;
        
        /// <summary>
        /// A DependencyGraph to represent the dependencies between cells.
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
        /// Returns true if a given string is in the valid variable form.
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        private static bool IsVar(string var)
        {
            string pattern = @"^[a-zA-Z_][a-zA-Z_\d]*$";
            return Regex.IsMatch(var, pattern);
        }

        public override object GetCellContents(string name)
        {
            if (name == null || !IsVar(name))
                throw new InvalidNameException();
            if (!cells.ContainsKey(name))
                return "";
            return cells[name];
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cells.Keys;
        }

        public override IList<string> SetCellContents(string name, double number)
        {
            if (name == null || !IsVar(name))
                throw new InvalidNameException();


            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, string text)
        {
            if (text == null)
                throw new ArgumentNullException();

            if (name == null || !IsVar(name))
                throw new InvalidNameException();



            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException();

            if (name == null || !IsVar(name))
                throw new InvalidNameException();



            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return graph.GetDependents(name);
        }

        /// <summary>
        /// Represents a cell within a spreadsheet.
        /// </summary>
        private class Cell
        {
            /// <summary>
            /// Property containing the contents of the cell.
            /// </summary>
            public object Contents
            {
                get;
                set;
            }

            /// <summary>
            /// Creates a new sell with the contents of contents.
            /// </summary>
            /// <param name="o"></param>
            public Cell(object contents)
            {
                Contents = contents;
            }
        }
    }
}
