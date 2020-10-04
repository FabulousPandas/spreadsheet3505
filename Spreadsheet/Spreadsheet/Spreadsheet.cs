/// Author: Khris Thammavong

using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Changed = false;
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
            Changed = false;
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
            Changed = false;
        }

        public override bool Changed { get; protected set; }

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
            if (name == null || !IsVar(name))
                throw new InvalidNameException();
            if (!cells.ContainsKey(name))
                return "";
            return cells[name].Value;
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

        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if (content == null)
                throw new ArgumentNullException();

            if (name == null || !IsVar(name))
                throw new InvalidNameException();
            
            name = Normalize(name);

            if (!IsVar(name) || !IsValid(name))
                throw new InvalidNameException();

            Changed = true;

            List<string> list;

            if(double.TryParse(content, out double result))
            {
                list = new List<string>(SetCellContents(name, result));
            }
            else if(content == "")
            {
                list = new List<string>(SetCellContents(name, content));
            }
            else if(content[0] == '=')
            {
                Formula formula = new Formula(content.Substring(1), Normalize, IsValid);
                list = new List<string>(SetCellContents(name, formula));
            }
            else
            {
                list = new List<string>(SetCellContents(name, content));
            }
            
            for(int i = 1; i < list.Count; i++) //reevaluates all the items except the first in the list since the value was already evaluated for the current cell
            {
                string s = list[i];
                cells[s].Reevaluate();
            }

            return list;
        }

        protected override IList<string> SetCellContents(string name, double number)
        {
            Cell cell = new Cell(number, VariableLookup);
            cells[name] = cell;
            graph.ReplaceDependees(name, new List<string>());

            return new List<string>(GetCellsToRecalculate(name));
        }

        protected override IList<string> SetCellContents(string name, string text)
        {
            if(text == "") //basically a removal
            {
                if(cells.ContainsKey(name))
                    cells.Remove(name);
                graph.ReplaceDependees(name, new List<string>());
            }
            else
            {
                Cell cell = new Cell(text, VariableLookup);
                cells[name] = cell;
            }

            return new List<string>(GetCellsToRecalculate(name));
        }

        protected override IList<string> SetCellContents(string name, Formula formula)
        {
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

            Cell cell = new Cell(formula, VariableLookup);
            
            cells[name] = cell;

            return list;
        }

        /// <summary>
        /// Returns the value of the cell
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        private double VariableLookup(string variable)
        {
            if(cells.ContainsKey(variable))
            {
                if (cells[variable].Contents is double || cells[variable].Contents is Formula)
                {
                    return (double) cells[variable].Value;
                }
            }

            throw new ArgumentException();
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
            /// Property containing the value of the cell.
            /// </summary>
            public object Value
            { get; private set; }

            /// <summary>
            /// Holds the lookup delegate for looking up a variable in a formula
            /// </summary>
            private Func<string, double> lookUp;

            /// <summary>
            /// Creates a new cell with the contents of contents and value of the evaluated contents.
            /// </summary>
            /// <param name="o"></param>
            public Cell(object contents, Func<string, double> lookup)
            {
                Contents = contents;
                lookUp = lookup;
                if (contents is double || contents is string)
                    Value = contents;
                else
                    Value = ((Formula)contents).Evaluate(lookup);
            }

            /// <summary>
            /// Reevaluates the value of the cell if the contents are a formula
            /// </summary>
            public void Reevaluate()
            {
                if(Contents is Formula)
                    Value = ((Formula) Contents).Evaluate(lookUp);
            }
        }
    }
}
