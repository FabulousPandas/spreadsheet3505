/// Author: Khris Thammavong
/// A set of tests to test the Spreadsheet class

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.ComponentModel;
using System.Linq;
using System.Xml;

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
        public void GetCellValueNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellValue(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValueInvalid()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellValue("46");
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
            s.SetContentsOfCell("&as&^", "=1 + 1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellNameInvalid()
        {
            Spreadsheet s = new Spreadsheet(s=>false,s=>s,"default");
            s.SetContentsOfCell("a1", "6");
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CheckCircularDependency()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a1 + 3");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void AlwaysFalseValidator()
        {
            Spreadsheet s = new Spreadsheet(s => false, s => s, "default");
            s.SetContentsOfCell("a1", "2");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NormalizerMakesInvalidName()
        {
            Spreadsheet s = new Spreadsheet(LowercaseValidator, s => s.ToUpper(), "default");
            s.SetContentsOfCell("a1", "2");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NormalizerMakesInvalidName2()
        {
            Spreadsheet s = new Spreadsheet(s=>true, s => "0", "default");
            s.SetContentsOfCell("a1", "2");
        }


        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNoVersionInFile()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            using (XmlWriter writer = XmlWriter.Create("noversion.xml", settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("spreadsheet");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "b1");
                writer.WriteElementString("contents", "1.4");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            Spreadsheet s = new Spreadsheet("noversion.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestEmptyFilePath()
        {
            Spreadsheet s = new Spreadsheet("", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void NullFilePath()
        {
            Spreadsheet s = new Spreadsheet(null, s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNonexistantFile()
        {
            Spreadsheet s = new Spreadsheet("notarealfile.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidFilePath()
        {
            Spreadsheet s = new Spreadsheet("/random/file/path.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidFileType()
        {
            Spreadsheet s = new Spreadsheet("nottherighttype.txt", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidXmlFile()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            using (XmlWriter writer = XmlWriter.Create("fakexmlfile.xml", settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "default");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "b1");
                writer.WriteElementString("contents", "1.4");
                writer.WriteElementString("notanactualelement", "&");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            Spreadsheet s = new Spreadsheet("fakexmlfile.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void VersionMismatch()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            using (XmlWriter writer = XmlWriter.Create("differentversions.xml", settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "default");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "b1");
                writer.WriteElementString("contents", "1.4");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            Spreadsheet s = new Spreadsheet("differentversions.xml", s => true, s => s, "notdefault");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidVarInFile()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            using (XmlWriter writer = XmlWriter.Create("invalidvariable.xml", settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "default");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "23");
                writer.WriteElementString("contents", "1.4");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            Spreadsheet s = new Spreadsheet("invalidvariable.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestCircularDependencyInFile()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            using (XmlWriter writer = XmlWriter.Create("circulardependency.xml", settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "default");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "a1");
                writer.WriteElementString("contents", "=a1 + 3");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            Spreadsheet s = new Spreadsheet("circulardependency.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSaveBadFilePath()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "2.4");
            s.SetContentsOfCell("b3", "random string");
            s.SetContentsOfCell("b4", "6.6");
            s.SetContentsOfCell("c2", "=a1 + b4");
            s.Save("/not/a/directory/file.xml");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidXmlGetVersion()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            using (XmlWriter writer = XmlWriter.Create("empty.xml", settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("notspreadsheet");

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            Spreadsheet s = new Spreadsheet();
            s.GetSavedVersion("empty.xml");
        }

        /// -------------------------
        /// Empty spreadsheet testing
        /// -------------------------

        [TestMethod]
        public void TestEmptyGetCellContents()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("a1"));
            Assert.AreEqual("", s.GetCellValue("a1"));
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
            Assert.AreEqual("", s.GetCellValue("a1"));
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
            Assert.AreEqual("", s.GetCellValue("a1"));
            Assert.AreEqual(0, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddAndRemoveString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "words");
            s.SetContentsOfCell("a1", "");
            Assert.AreEqual("", s.GetCellContents("a1"));
            Assert.AreEqual("", s.GetCellValue("a1"));
            Assert.AreEqual(0, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddAndRemoveFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=6.7 + 93.135");
            s.SetContentsOfCell("a1", "");
            Assert.AreEqual("", s.GetCellContents("a1"));
            Assert.AreEqual("", s.GetCellValue("a1"));
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
            Assert.AreEqual(2.0, (double)s.GetCellContents("a1"), 1e-9);
            Assert.AreEqual(2.0, (double)s.GetCellValue("a1"), 1e-9);
            Assert.AreEqual(1, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddOneString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("_7", "bunch of random letters");
            Assert.AreEqual("bunch of random letters", s.GetCellContents("_7"));
            Assert.AreEqual("bunch of random letters", s.GetCellValue("_7"));
            Assert.AreEqual(1, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void AddOneFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("5 + 7.8");
            s.SetContentsOfCell("b_3", "=5 + 7.8");
            Assert.AreEqual(f, s.GetCellContents("b_3"));
            Assert.AreEqual(5 + 7.8, s.GetCellValue("b_3"));
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
            Assert.AreEqual(2.0, (double)s.GetCellValue("a1"), 1e-9);
            Assert.AreEqual(5.4, (double)s.GetCellValue("b5"), 1e-9);
            Assert.AreEqual(12.1, (double)s.GetCellValue("__"), 1e-9);
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
            Assert.AreEqual("ajhd", s.GetCellValue("a1"));
            Assert.AreEqual("23/6 + 34.3", s.GetCellValue("b5"));
            Assert.AreEqual("aeiou8^%$#", s.GetCellValue("__"));
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
            Assert.AreEqual(1 + 7.2 + 3.2, (double)s.GetCellValue("a1"), 1e-9);
            Assert.AreEqual(7.2 + 3.2, (double)s.GetCellValue("b5"), 1e-9);
            Assert.AreEqual(2.3 * (7.2 + 3.2) + (1 + 7.2 + 3.2) - 3.5, (double)s.GetCellValue("__"), 1e-9);
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
            s.SetContentsOfCell("a6", "6");
            s.SetContentsOfCell("c12", "=m9 * 2.3");
            s.SetContentsOfCell("m9", "=a6 - 3.5");
            Assert.AreEqual(6, (double)s.GetCellContents("a6"), 1e-9);
            Assert.AreEqual(c12, s.GetCellContents("c12"));
            Assert.AreEqual(m9, s.GetCellContents("m9"));
            Assert.AreEqual(3, s.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        public void TestChanged()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.IsFalse(s.Changed);
            s.SetContentsOfCell("a1", "=1+1");
            Assert.IsTrue(s.Changed);
        }

        /// -----------------
        /// Save file testing
        /// -----------------
        [TestMethod]
        public void TestSaveAndLoad()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "2.4");
            s.SetContentsOfCell("b3", "random string");
            s.SetContentsOfCell("b4", "6.6");
            s.SetContentsOfCell("c2", "=a1 + b4");
            s.Save("savefile.xml");

            Spreadsheet s2 = new Spreadsheet("savefile.xml", s => true, s => s, "default");
            Assert.AreEqual(4, s2.GetNamesOfAllNonemptyCells().Count());
            Assert.AreEqual(2.4, (double)s2.GetCellContents("a1"), 1e-9);
            Assert.AreEqual("random string", s2.GetCellContents("b3"));
            Assert.AreEqual(6.6, (double)s2.GetCellContents("b4"), 1e-9);
            Assert.AreEqual(new Formula("a1 + b4"), s2.GetCellContents("c2"));
        }

        [TestMethod]
        public void TestLoadFromFile()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            using (XmlWriter writer = XmlWriter.Create("testloadfile.xml", settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "default");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "b1");
                writer.WriteElementString("contents", "1.4");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "g6");
                writer.WriteElementString("contents", "abcde");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "c2");
                writer.WriteElementString("contents", "=b1 + 5");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            Spreadsheet s = new Spreadsheet("testloadfile.xml", s => true, s => s, "default");
            Assert.AreEqual(3, s.GetNamesOfAllNonemptyCells().Count());
            Assert.AreEqual(1.4, (double)s.GetCellContents("b1"), 1e-9);
            Assert.AreEqual("abcde", s.GetCellContents("g6"));
            Assert.AreEqual(new Formula("b1 + 5"), s.GetCellContents("c2"));
        }


        [TestMethod]
        public void TestGetSavedVersion()
        {
            Spreadsheet s = new Spreadsheet();
            s.Save("savefile2.xml");
            Assert.AreEqual("default", s.GetSavedVersion("savefile2.xml"));

            Spreadsheet s2 = new Spreadsheet(s=>true, s=>s, "other version");
            s2.SetContentsOfCell("a1", "2.4");
            s2.SetContentsOfCell("b3", "random string");
            s2.SetContentsOfCell("b4", "6.6");
            s2.SetContentsOfCell("c2", "=a1 + b4");
            s2.Save("savefile3.xml");
            Assert.AreEqual("other version", s2.GetSavedVersion("savefile3.xml"));
            Assert.AreNotEqual("other version", s2.GetSavedVersion("savefile2.xml"));
        }

        /// ------------
        /// Stress Tests
        /// ------------

        [TestMethod]
        [Timeout(4000)]
        public void StressTest()
        {
            Spreadsheet s = new Spreadsheet();
            for(int i = 0; i < 1000; i++)
            {
                s.SetContentsOfCell("a" + i, i.ToString());
            }
            for (int i = 0; i < 1000; i++)
            {
                Assert.AreEqual((double) i, s.GetCellContents("a" + i));
                Assert.AreEqual((double)i, s.GetCellValue("a" + i));
            }
        }

        [TestMethod]
        [Timeout(4000)]
        public void StressTest2()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 0; i < 1000; i++)
            {
                s.SetContentsOfCell("a" + i, i.ToString());
            }
            s.Save("stresssave.xml");
            Spreadsheet s2 = new Spreadsheet("stresssave.xml", s => true, s => s, "default");
        }

        /// <summary>
        /// Returns true only if all the characters in s are lowercase
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool LowercaseValidator(string s)
        {
            foreach (char c in s)
                if (char.IsUpper(c))
                    return false;
            return true;
        }
    }
}
