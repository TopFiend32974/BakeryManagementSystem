namespace Delete_Push_Pull
{
    partial class setting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblBackup = new Label();
            button1 = new Button();
            lblPull = new Label();
            btnPullDir = new Button();
            lblPush = new Label();
            btnPushDir = new Button();
            lblShowDir = new Label();
            btnChangeDeleteDir = new Button();
            btnSheetGen = new Button();
            lblGenSheets = new Label();
            btnExcelFontChange = new Button();
            lblFontSizeDisplay = new Label();
            numInputExcelFont = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numInputExcelFont).BeginInit();
            SuspendLayout();
            // 
            // lblBackup
            // 
            lblBackup.AutoSize = true;
            lblBackup.Location = new Point(159, 107);
            lblBackup.Name = "lblBackup";
            lblBackup.Size = new Size(67, 15);
            lblBackup.TabIndex = 25;
            lblBackup.Text = "Backup Dir:";
            // 
            // button1
            // 
            button1.Location = new Point(25, 103);
            button1.Name = "button1";
            button1.Size = new Size(128, 23);
            button1.TabIndex = 24;
            button1.Text = "Backup Location";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // lblPull
            // 
            lblPull.AutoSize = true;
            lblPull.Location = new Point(150, 136);
            lblPull.Name = "lblPull";
            lblPull.Size = new Size(25, 15);
            lblPull.TabIndex = 23;
            lblPull.Text = "Dir:";
            // 
            // btnPullDir
            // 
            btnPullDir.Location = new Point(25, 132);
            btnPullDir.Name = "btnPullDir";
            btnPullDir.Size = new Size(119, 23);
            btnPullDir.TabIndex = 22;
            btnPullDir.Text = "Change Cloud Dir";
            btnPullDir.UseVisualStyleBackColor = true;
            btnPullDir.Click += btnPullDir_Click_1;
            // 
            // lblPush
            // 
            lblPush.AutoSize = true;
            lblPush.Location = new Point(150, 78);
            lblPush.Name = "lblPush";
            lblPush.Size = new Size(25, 15);
            lblPush.TabIndex = 21;
            lblPush.Text = "Dir:";
            // 
            // btnPushDir
            // 
            btnPushDir.Location = new Point(25, 74);
            btnPushDir.Name = "btnPushDir";
            btnPushDir.Size = new Size(119, 23);
            btnPushDir.TabIndex = 20;
            btnPushDir.Text = "Change Local Dir";
            btnPushDir.UseVisualStyleBackColor = true;
            btnPushDir.Click += btnPushDir_Click_1;
            // 
            // lblShowDir
            // 
            lblShowDir.AutoSize = true;
            lblShowDir.Location = new Point(150, 45);
            lblShowDir.Name = "lblShowDir";
            lblShowDir.Size = new Size(25, 15);
            lblShowDir.TabIndex = 19;
            lblShowDir.Text = "Dir:";
            // 
            // btnChangeDeleteDir
            // 
            btnChangeDeleteDir.Location = new Point(25, 41);
            btnChangeDeleteDir.Name = "btnChangeDeleteDir";
            btnChangeDeleteDir.Size = new Size(119, 23);
            btnChangeDeleteDir.TabIndex = 18;
            btnChangeDeleteDir.Text = "Change Delete Dir";
            btnChangeDeleteDir.UseVisualStyleBackColor = true;
            btnChangeDeleteDir.Click += btnChangeDeleteDir_Click;
            // 
            // btnSheetGen
            // 
            btnSheetGen.Location = new Point(25, 161);
            btnSheetGen.Name = "btnSheetGen";
            btnSheetGen.Size = new Size(140, 23);
            btnSheetGen.TabIndex = 26;
            btnSheetGen.Text = "Change Sheets-Gen Dir";
            btnSheetGen.UseVisualStyleBackColor = true;
            btnSheetGen.Click += btnSheetGen_Click;
            // 
            // lblGenSheets
            // 
            lblGenSheets.AutoSize = true;
            lblGenSheets.Location = new Point(172, 165);
            lblGenSheets.Name = "lblGenSheets";
            lblGenSheets.Size = new Size(22, 15);
            lblGenSheets.TabIndex = 27;
            lblGenSheets.Text = "Dir";
            // 
            // btnExcelFontChange
            // 
            btnExcelFontChange.Location = new Point(600, 74);
            btnExcelFontChange.Name = "btnExcelFontChange";
            btnExcelFontChange.Size = new Size(140, 23);
            btnExcelFontChange.TabIndex = 28;
            btnExcelFontChange.Text = "Change Excel Font Size";
            btnExcelFontChange.UseVisualStyleBackColor = true;
            btnExcelFontChange.Click += btnExcelFontChange_Click;
            // 
            // lblFontSizeDisplay
            // 
            lblFontSizeDisplay.AutoSize = true;
            lblFontSizeDisplay.Location = new Point(600, 111);
            lblFontSizeDisplay.Name = "lblFontSizeDisplay";
            lblFontSizeDisplay.Size = new Size(57, 15);
            lblFontSizeDisplay.TabIndex = 29;
            lblFontSizeDisplay.Text = "Font Size:";
            // 
            // numInputExcelFont
            // 
            numInputExcelFont.Location = new Point(610, 45);
            numInputExcelFont.Name = "numInputExcelFont";
            numInputExcelFont.Size = new Size(120, 23);
            numInputExcelFont.TabIndex = 31;
            // 
            // setting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(numInputExcelFont);
            Controls.Add(lblFontSizeDisplay);
            Controls.Add(btnExcelFontChange);
            Controls.Add(lblGenSheets);
            Controls.Add(btnSheetGen);
            Controls.Add(btnChangeDeleteDir);
            Controls.Add(lblBackup);
            Controls.Add(button1);
            Controls.Add(lblPull);
            Controls.Add(btnPullDir);
            Controls.Add(lblPush);
            Controls.Add(btnPushDir);
            Controls.Add(lblShowDir);
            Name = "setting";
            Text = "setting";
            Load += setting_Load_1;
            ((System.ComponentModel.ISupportInitialize)numInputExcelFont).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBackup;
        private Button button1;
        private Label lblPull;
        private Button btnPullDir;
        private Label lblPush;
        private Button btnPushDir;
        private Label lblShowDir;
        private Button btnChangeDeleteDir;
        private Button btnSheetGen;
        private Label lblGenSheets;
        private Button btnExcelFontChange;
        private Label lblFontSizeDisplay;
        private NumericUpDown numInputExcelFont;
    }
}