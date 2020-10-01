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
        /// Creates a new spreadsheet with no validity conditions and no normalizer with version "default"
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
        }

        /// <summary>
        /// Creates a new spreadsheet with the given validator, normalizer, and version
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
        }

        /// <summary>
        /// Constructs a spreadsheet from the save file found at the given filePath with the given validator, normalizer, and version
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
        }

        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

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
            return cells[name].Contents;
        }

        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cells.Keys;
        }

        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }

        protected override IList<string> SetCellContents(string name, double number)
        {
            if (name == null || !IsVar(name))
                throw new InvalidNameException();
            Cell cell = new Cell(number);
            cells[name] = cell;
            graph.ReplaceDependees(name, new List<string>());

            return new List<string>(GetCellsToRecalculate(name));
        }

        protected override IList<string> SetCellContents(string name, string text)
        {
            if (text == null)
                throw new ArgumentNullException();

            if (name == null || !IsVar(name))
                throw new InvalidNameException();

            if(text == "") //basically a removal
            {
                if(cells.ContainsKey(name))
                    cells.Remove(name);
                graph.ReplaceDependees(name, new List<string>());
            }
            else
            {
                Cell cell = new Cell(text);
                cells[name] = cell;
            }

            return new List<string>(GetCellsToRecalculate(name));
        }

        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException();

            if (name == null || !IsVar(name))
                throw new InvalidNameException();
            
            foreach (string variable in formula.GetVariables())
                graph.AddDependency(variable, name);

            List<string> list = new List<string>();

            try
            {
                list = new List<string> (GetCellsToRecalculate(name));
            }
            catch (CircularException)
            {
                foreach (string variable in formula.GetVariables())
                    graph.RemoveDependency(variable, name);
                throw new CircularException();
            }

            Cell cell = new Cell(formula);
            cells[name] = cell;

            return list;
        }

        public override IList<string> SetContentsOfCell(string name, string content)
        {
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
            { get; private set;}

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
