namespace Delete_Push_Pull
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            btnDelete = new Button();
            btnOpenFile = new Button();
            lblConsole = new Label();
            btnPush = new Button();
            btnPull = new Button();
            btnOpenPush = new Button();
            btnOpenPull = new Button();
            menuStrip1 = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem1 = new ToolStripMenuItem();
            recoveryToolStripMenuItem = new ToolStripMenuItem();
            btnGenGoogleSheets = new Button();
            lblGenLabels = new Button();
            btnOpenGoogleDir = new Button();
            btnPrintSheets = new Button();
            btnSelectDay = new Button();
            lblDaySelected = new Label();
            btnProductionHelper = new Button();
            refreshDataLoaderToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Vladimir Script", 26F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(255, 9);
            label1.Name = "label1";
            label1.Size = new Size(262, 42);
            label1.TabIndex = 1;
            label1.Text = "Westcountry Bakery";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(114, 192);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(99, 41);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Delete Labels";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnOpenFile
            // 
            btnOpenFile.Location = new Point(114, 239);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(129, 24);
            btnOpenFile.TabIndex = 4;
            btnOpenFile.Text = "Open Label Folder";
            btnOpenFile.UseVisualStyleBackColor = true;
            btnOpenFile.Click += btnOpenFile_Click;
            // 
            // lblConsole
            // 
            lblConsole.AutoSize = true;
            lblConsole.Location = new Point(372, 112);
            lblConsole.Name = "lblConsole";
            lblConsole.Size = new Size(22, 15);
            lblConsole.TabIndex = 7;
            lblConsole.Text = "---";
            lblConsole.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPush
            // 
            btnPush.Location = new Point(350, 195);
            btnPush.Name = "btnPush";
            btnPush.Size = new Size(75, 23);
            btnPush.TabIndex = 8;
            btnPush.Text = "Push";
            btnPush.UseVisualStyleBackColor = true;
            btnPush.Click += btnPush_Click;
            // 
            // btnPull
            // 
            btnPull.Location = new Point(580, 195);
            btnPull.Name = "btnPull";
            btnPull.Size = new Size(75, 23);
            btnPull.TabIndex = 9;
            btnPull.Text = "Pull";
            btnPull.UseVisualStyleBackColor = true;
            btnPull.Click += btnPull_Click;
            // 
            // btnOpenPush
            // 
            btnOpenPush.Location = new Point(327, 224);
            btnOpenPush.Name = "btnOpenPush";
            btnOpenPush.Size = new Size(129, 24);
            btnOpenPush.TabIndex = 10;
            btnOpenPush.Text = "Open Local WCB";
            btnOpenPush.UseVisualStyleBackColor = true;
            btnOpenPush.Click += btnOpenPush_Click;
            // 
            // btnOpenPull
            // 
            btnOpenPull.Location = new Point(554, 224);
            btnOpenPull.Name = "btnOpenPull";
            btnOpenPull.Size = new Size(129, 24);
            btnOpenPull.TabIndex = 13;
            btnOpenPull.Text = "Open Cloud WCB";
            btnOpenPull.UseVisualStyleBackColor = true;
            btnOpenPull.Click += btnOpenPull_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, refreshDataLoaderToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 18;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { settingsToolStripMenuItem1, recoveryToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(50, 20);
            settingsToolStripMenuItem.Text = "Menu";
            // 
            // settingsToolStripMenuItem1
            // 
            settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            settingsToolStripMenuItem1.Size = new Size(122, 22);
            settingsToolStripMenuItem1.Text = "Settings";
            settingsToolStripMenuItem1.Click += settingsToolStripMenuItem1_Click;
            // 
            // recoveryToolStripMenuItem
            // 
            recoveryToolStripMenuItem.Name = "recoveryToolStripMenuItem";
            recoveryToolStripMenuItem.Size = new Size(122, 22);
            recoveryToolStripMenuItem.Text = "Recovery";
            recoveryToolStripMenuItem.Click += recoveryToolStripMenuItem_Click;
            // 
            // btnGenGoogleSheets
            // 
            btnGenGoogleSheets.Location = new Point(398, 355);
            btnGenGoogleSheets.Name = "btnGenGoogleSheets";
            btnGenGoogleSheets.Size = new Size(167, 23);
            btnGenGoogleSheets.TabIndex = 19;
            btnGenGoogleSheets.Text = "Generate Google Sheets";
            btnGenGoogleSheets.UseVisualStyleBackColor = true;
            btnGenGoogleSheets.Click += btnGenGoogleSheetsActual_Click;
            // 
            // lblGenLabels
            // 
            lblGenLabels.Location = new Point(114, 163);
            lblGenLabels.Name = "lblGenLabels";
            lblGenLabels.Size = new Size(115, 23);
            lblGenLabels.TabIndex = 20;
            lblGenLabels.Text = "Gen Labels";
            lblGenLabels.UseVisualStyleBackColor = true;
            lblGenLabels.Click += lblGenLabels_Click;
            // 
            // btnOpenGoogleDir
            // 
            btnOpenGoogleDir.Location = new Point(327, 388);
            btnOpenGoogleDir.Name = "btnOpenGoogleDir";
            btnOpenGoogleDir.Size = new Size(129, 24);
            btnOpenGoogleDir.TabIndex = 21;
            btnOpenGoogleDir.Text = "Open Google Drive";
            btnOpenGoogleDir.UseVisualStyleBackColor = true;
            btnOpenGoogleDir.Click += btnOpenGoogleDir_Click;
            // 
            // btnPrintSheets
            // 
            btnPrintSheets.Location = new Point(347, 418);
            btnPrintSheets.Name = "btnPrintSheets";
            btnPrintSheets.Size = new Size(78, 23);
            btnPrintSheets.TabIndex = 22;
            btnPrintSheets.Text = "Print Sheets";
            btnPrintSheets.UseVisualStyleBackColor = true;
            btnPrintSheets.Click += btnPrintSheets_Click;
            // 
            // btnSelectDay
            // 
            btnSelectDay.Location = new Point(673, 69);
            btnSelectDay.Name = "btnSelectDay";
            btnSelectDay.Size = new Size(78, 23);
            btnSelectDay.TabIndex = 23;
            btnSelectDay.Text = "Select Day";
            btnSelectDay.UseVisualStyleBackColor = true;
            btnSelectDay.Click += btnSelectDay_Click;
            // 
            // lblDaySelected
            // 
            lblDaySelected.AutoSize = true;
            lblDaySelected.Location = new Point(651, 51);
            lblDaySelected.Name = "lblDaySelected";
            lblDaySelected.Size = new Size(126, 15);
            lblDaySelected.TabIndex = 24;
            lblDaySelected.Text = "Current Day is: Sunday";
            lblDaySelected.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnProductionHelper
            // 
            btnProductionHelper.Location = new Point(225, 355);
            btnProductionHelper.Name = "btnProductionHelper";
            btnProductionHelper.Size = new Size(167, 23);
            btnProductionHelper.TabIndex = 25;
            btnProductionHelper.Text = "Generate Production Helper";
            btnProductionHelper.UseVisualStyleBackColor = true;
            btnProductionHelper.Click += btnProductionHelper_Click;
            // 
            // refreshDataLoaderToolStripMenuItem
            // 
            refreshDataLoaderToolStripMenuItem.Name = "refreshDataLoaderToolStripMenuItem";
            refreshDataLoaderToolStripMenuItem.Size = new Size(121, 20);
            refreshDataLoaderToolStripMenuItem.Text = "Refresh DataLoader";
            refreshDataLoaderToolStripMenuItem.Click += refreshDataLoaderToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnProductionHelper);
            Controls.Add(lblDaySelected);
            Controls.Add(btnSelectDay);
            Controls.Add(btnPrintSheets);
            Controls.Add(btnOpenGoogleDir);
            Controls.Add(lblGenLabels);
            Controls.Add(btnGenGoogleSheets);
            Controls.Add(btnOpenPull);
            Controls.Add(btnOpenPush);
            Controls.Add(btnPull);
            Controls.Add(btnPush);
            Controls.Add(lblConsole);
            Controls.Add(btnOpenFile);
            Controls.Add(btnDelete);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Westcountry Bakery";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnDelete;
        private Button btnOpenFile;
        private Label lblConsole;
        private Button btnPush;
        private Button btnPull;
        private Button btnOpenPush;
        private Button btnOpenPull;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem1;
        private ToolStripMenuItem recoveryToolStripMenuItem;
        private Button btnGenGoogleSheets;
        private Button lblGenLabels;
        private Button btnOpenGoogleDir;
        private Button btnPrintSheets;
        private Button btnSelectDay;
        public Label lblDaySelected;
        private Button btnProductionHelper;
        private ToolStripMenuItem refreshDataLoaderToolStripMenuItem;
    }
}