using System.Collections.Generic;

namespace Delete_Push_Pull
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            var dataLoader = new DataLoader("");
            if (!dataLoader.LoadAllData())
            {
                MessageBox.Show("Local Directory not set correctly. Select Local Dir and Restart before continue");
            }
            MainClass.SetDay();            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}