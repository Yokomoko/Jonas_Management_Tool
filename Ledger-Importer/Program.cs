﻿using System;
using System.Windows.Forms;

namespace Jonas_Sage_Importer
{
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
            Application.Run(new RadForm1());
            //Testing New Giby Stuff
            //Application.Run(new CsvImporter.JonasCsvImporter());
        }
    }
}
