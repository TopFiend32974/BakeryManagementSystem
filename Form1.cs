using System.Diagnostics;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using Delete_Push_Pull.Properties;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data;
using Microsoft.Office.Interop.Access.Dao;
using CsvHelper;
using CsvHelper.Configuration;
using System.Data.Common;
using System.Globalization;
using System.Text;
//using Microsoft.Office.Interop.Excel;


namespace Delete_Push_Pull
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (Directory.GetFiles((string)Settings.Default["DeleteDir"], "*.mdb").Length == 0)
            {
                lblConsole.Text = $"There are no labels in folder.";
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo((string)Settings.Default["DeleteDir"]);
                FileInfo[] files = di.GetFiles("*.mdb")
                                     .Where(p => p.Extension == ".mdb").ToArray();
                foreach (FileInfo file in files)
                    try
                    {
                        file.Attributes = FileAttributes.Normal;
                        File.Delete(file.FullName);
                        lblConsole.Text = $"Labels deleted.";
                    }
                    catch
                    {

                        lblConsole.Text = $"An error has occoured.";
                    }
            }

        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", (string)Settings.Default["DeleteDir"]);
        }



        private void btnPush_Click(object sender, EventArgs e)
        {
            DateTime tempDate = DateTime.Now;
            string backupName = tempDate.ToString("yy-MM-dd");

            string sourcePath = (string)Settings.Default["Local"];
            string targetPath = (string)Settings.Default["Cloud"];
            string BackupPath = (string)Settings.Default["BackupDir"];


            string targetPathNamed = targetPath + @"\" + backupName;
            string BackupPathNamed = BackupPath + @"\" + backupName;

            if (System.IO.Directory.GetFiles((string)Settings.Default["DeleteDir"], "*.mdb").Length != 0)
            {
                DialogResult dialogResult = MessageBox.Show("Your Label Folder is Not Empty.", "Warning");
            }
            else
            {

                DialogResult dialogResult = MessageBox.Show("Are you sure you want to Push?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {

                        System.IO.DirectoryInfo di = new DirectoryInfo((string)Settings.Default["Cloud"]);

                        bool isDir = Directory.Exists(BackupPathNamed);
                        if (!isDir)
                        {
                            try
                            {
                                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                                {
                                    Directory.CreateDirectory(dirPath.Replace(sourcePath, BackupPathNamed));
                                }
                                foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                                {
                                    File.Copy(newPath, newPath.Replace(sourcePath, BackupPathNamed), true);
                                }


                                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                                {
                                    Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPathNamed));
                                }
                                foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                                {
                                    File.Copy(newPath, newPath.Replace(sourcePath, targetPathNamed), true);
                                }
                                lblConsole.Text = "Successfully Pushed.";
                            }
                            catch
                            {
                                lblConsole.Text = "Backup dir did not work";
                            }
                        }
                        else
                        {
                            backupName = tempDate.ToString("yy-MM-dd  HH-mm-sstt");
                            BackupPathNamed = BackupPath + @"\" + backupName;
                            targetPathNamed = targetPath + @"\" + backupName;
                            try
                            {
                                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                                {
                                    Directory.CreateDirectory(dirPath.Replace(sourcePath, BackupPathNamed));
                                }
                                foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                                {
                                    File.Copy(newPath, newPath.Replace(sourcePath, BackupPathNamed), true);
                                }


                                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                                {
                                    Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPathNamed));
                                }
                                foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                                {
                                    File.Copy(newPath, newPath.Replace(sourcePath, targetPathNamed), true);
                                }
                                lblConsole.Text = "Successfully Pushed.";
                            }
                            catch
                            {
                                lblConsole.Text = "Backup dir did not work";
                            }
                        }


                    }
                    catch
                    {
                        lblConsole.Text = "Push has been unsuccessfully moved";
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }

            }
        }


        private void btnPull_Click(object sender, EventArgs e)
        {

            string sourcePath = new DirectoryInfo((string)Settings.Default["Cloud"]).GetDirectories().OrderByDescending(d => d.LastWriteTimeUtc).First().ToString();
            string targetPath = (string)Settings.Default["Local"];

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to pull from'" + sourcePath + "' ?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {

                    System.IO.DirectoryInfo di = new DirectoryInfo((string)Settings.Default["Local"]);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                    }

                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                    {
                        File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                    }

                    lblConsole.Text = "Pull has successfully moved.";
                }
                catch
                {
                    lblConsole.Text = "Pull has unsuccessfully moved.";
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

        }

        private void btnOpenPush_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", (string)Settings.Default["Local"]);
        }

        private void btnOpenPull_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", (string)Settings.Default["Cloud"]);
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setting setting = new setting();
            setting.Show();
        }

        private void recoveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sourcePath = new DirectoryInfo((string)Settings.Default["BackupDir"]).GetDirectories().OrderByDescending(d => d.LastWriteTimeUtc).First().ToString();
            string targetPath = (string)Settings.Default["Local"];

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to recover from'" + sourcePath + "' ?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {

                    System.IO.DirectoryInfo di = new DirectoryInfo((string)Settings.Default["Local"]);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                    }

                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                    {
                        File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                    }

                    lblConsole.Text = "Pull has successfully moved.";
                }
                catch
                {
                    lblConsole.Text = "Pull has unsuccessfully moved.";
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }


        private void lblGenLabels_Click(object sender, EventArgs e)
        {
            string batchFilePath = (string)Settings.Default["Local"] + @"\genlabels.bat";
            string batchFileDir = (string)Settings.Default["Local"];

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = batchFilePath;
            startInfo.WorkingDirectory = batchFileDir;

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }


        }



        private void btnGenGoogleSheetsActual_Click(object sender, EventArgs e)
        {
            MainClass.LoadMainClass();
        }
        private void btnProductionHelper_Click(object sender, EventArgs e)
        {
            MainClass.LoadProductionHelper();

        }


        private void btnOpenGoogleDir_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", (string)Settings.Default["GenSheets"]);
        }

        private void btnPrintSheets_Click(object sender, EventArgs e)
        {
            SheetSelectionForm print = new SheetSelectionForm();
            print.Show();
        }
        private void btnSelectDay_Click(object sender, EventArgs e)
        {
            MainClass.ShowDaySelectionDialog();
            //return a day value for lbl 
            string selectedDay = MainClass.GetDay();
            lblDaySelected.Text = "Day Selected: " + selectedDay;

        }


    }


}