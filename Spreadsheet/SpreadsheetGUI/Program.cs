using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Keeps track of how many top-level forms are running
    /// Code taken from demo program.cs
    /// </summary>
    class SpreadsheetApplication : ApplicationContext
    {
        // Number of open forms
        private int formCount = 0;

        // Singleton ApplicationContext
        private static SpreadsheetApplication appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetApplication()
        {
        }

        /// <summary>
        /// Returns the one SpreadsheetApplication.
        /// </summary>
        public static SpreadsheetApplication getAppContext()
        {
            if (appContext == null)
            {
                appContext = new SpreadsheetApplication();
            }
            return appContext;
        }

        /// <summary>
        /// Runs the form
        /// </summary>
        public void RunForm(Form form)
        {
            // One more form is running
            formCount++;

            // When this form closes, we want to find out
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }

    }
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SpreadsheetApplication sheetApp = SpreadsheetApplication.getAppContext();
            sheetApp.RunForm(new SpreadsheetForm());
            Application.Run(sheetApp);
        }
    }
}
