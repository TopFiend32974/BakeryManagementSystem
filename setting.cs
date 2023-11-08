using Delete_Push_Pull.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            lblShowDir.Text = "Current Dir Selected: " + (string)Settings.Default["DeleteDir"];
            lblPush.Text = "Current Dir Selected: " + (string)Settings.Default["PushDir"];
            lblPull.Text = "Current Dir Selected: " + (string)Settings.Default["PullDir"];
            lblBackup.Text = "Current Dir Selected: " + (string)Settings.Default["BackupDir"];
            lblGenSheets.Text = "Current Dir Selected: " + (string)Settings.Default["GenSheets"];
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
                Settings.Default["PushDir"] = diag.SelectedPath.ToString();
                Settings.Default.Save();

                Data dataInstance = Data.GetInstance();
                dataInstance.ClearAllData();
                var dataLoader = new DataLoader("");

                if (!dataLoader.LoadAllData()){
                    MessageBox.Show("Push Dir Updated. \n LoadAllData FAILED");
                }
                MessageBox.Show("Push Dir Updated. \n LoadAllData Loaded Successfully.");

            }
            else
            {
                Settings.Default["PushDir"] = "";
            }
            lblPush.Text = "Current Dir Selected: " + (string)Settings.Default["PushDir"];
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
                Settings.Default["PullDir"] = diag.SelectedPath.ToString();
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["PullDir"] = "";
            }
            lblPull.Text = "Current Dir Selected: " + (string)Settings.Default["PullDir"];
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
    }
}
