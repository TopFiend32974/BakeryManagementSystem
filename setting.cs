using Delete_Push_Pull.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Delete_Push_Pull
{
    public partial class setting : Form
    {
        public setting()
        {
            InitializeComponent();
        }
        private void setting_Load_1(object sender, EventArgs e)
        {
            //DirShow
            lblShowDir.Text = "Current Dir Selected: " + (string)Settings.Default["DeleteDir"];
            lblPush.Text = "Current Dir Selected: " + (string)Settings.Default["Local"];
            lblPull.Text = "Current Dir Selected: " + (string)Settings.Default["Cloud"];
            lblBackup.Text = "Current Dir Selected: " + (string)Settings.Default["BackupDir"];
            lblGenSheets.Text = "Current Dir Selected: " + (string)Settings.Default["GenSheets"];
            lblProdDir.Text = "Current Dir Selected: " + (string)Settings.Default["ProductionHelpDir"];
            decimal ExcelFontSize = (decimal)Settings.Default["ExcelFontSize"];
            lblFontSizeDisplay.Text = ExcelFontSize.ToString();
        }

        private string PromptForDirectory()
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // Prompt for title
                    string title = PromptForTitle();

                    // Combine title and path with a separator
                    return $"{title}|{folderBrowserDialog.SelectedPath}";
                }
            }

            return null;
        }
        private string PromptForTitle()
        {
            using (var titleForm = new TitleForm())
            {
                if (titleForm.ShowDialog() == DialogResult.OK)
                {
                    return titleForm.Title;
                }
            }

            return "Untitled"; // Default title if none provided
        }




        private void btnChangeDeleteDir_Click(object sender, EventArgs e)
        {
            //DeleteDir
            //This will change directory location saved onto settings

            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Settings.Default["DeleteDir"] = diag.SelectedPath.ToString();
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["DeleteDir"] = "";
            }
            lblShowDir.Text = "Current Dir Selected: " + (string)Settings.Default["DeleteDir"];

        }

        private void btnPushDir_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Settings.Default["Local"] = diag.SelectedPath.ToString();
                Settings.Default.Save();
                if (!loadAllDAta.ResetData())
                {
                    MessageBox.Show("Error: Could not load data");
                }
                else
                {
                    MessageBox.Show("Data Loaded");
                }

            }
            else
            {
                Settings.Default["Local"] = "";
            }
            lblPush.Text = "Current Dir Selected: " + (string)Settings.Default["Local"];
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Settings.Default["BackupDir"] = diag.SelectedPath.ToString();
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["BackupDir"] = "";
            }
            lblBackup.Text = "Current Dir Selected: " + (string)Settings.Default["BackupDir"];
        }

        private void btnPullDir_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                Settings.Default["Cloud"] = diag.SelectedPath.ToString();
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["Cloud"] = "";
            }
            lblPull.Text = "Current Dir Selected: " + (string)Settings.Default["Cloud"];
        }

        private void btnSheetGen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            OpenFileDialog ofd = new OpenFileDialog();


            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Settings.Default["GenSheets"] = diag.SelectedPath.ToString();
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["GenSheets"] = "";
            }
            lblGenSheets.Text = "Current Dir Selected: " + (string)Settings.Default["GenSheets"];
        }

        private void btnExcelFontChange_Click(object sender, EventArgs e)
        {
            decimal ExcelFontSize = (decimal)Settings.Default["ExcelFontSize"];
            //numInputExcelFont
            //lblFontSizeDisplay            
            Settings.Default["ExcelFontSize"] = numInputExcelFont.Value;
            Settings.Default.Save();
            lblFontSizeDisplay.Text = ExcelFontSize.ToString();
        }

        private void btnChangeProdDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            OpenFileDialog ofd = new OpenFileDialog();


            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Settings.Default["ProductionHelpDir"] = diag.SelectedPath.ToString();
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["ProductionHelpDir"] = "";
            }
            lblProdDir.Text = "Current Dir Selected: " + (string)Settings.Default["ProductionHelpDir"];
        }
    }
}
