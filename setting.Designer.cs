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
            this.lblBackup = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblPull = new System.Windows.Forms.Label();
            this.btnPullDir = new System.Windows.Forms.Button();
            this.lblPush = new System.Windows.Forms.Label();
            this.btnPushDir = new System.Windows.Forms.Button();
            this.lblShowDir = new System.Windows.Forms.Label();
            this.btnChangeDeleteDir = new System.Windows.Forms.Button();
            this.btnSheetGen = new System.Windows.Forms.Button();
            this.lblGenSheets = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblBackup
            // 
            this.lblBackup.AutoSize = true;
            this.lblBackup.Location = new System.Drawing.Point(159, 107);
            this.lblBackup.Name = "lblBackup";
            this.lblBackup.Size = new System.Drawing.Size(67, 15);
            this.lblBackup.TabIndex = 25;
            this.lblBackup.Text = "Backup Dir:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 24;
            this.button1.Text = "Backup Location";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // lblPull
            // 
            this.lblPull.AutoSize = true;
            this.lblPull.Location = new System.Drawing.Point(150, 136);
            this.lblPull.Name = "lblPull";
            this.lblPull.Size = new System.Drawing.Size(25, 15);
            this.lblPull.TabIndex = 23;
            this.lblPull.Text = "Dir:";
            // 
            // btnPullDir
            // 
            this.btnPullDir.Location = new System.Drawing.Point(25, 132);
            this.btnPullDir.Name = "btnPullDir";
            this.btnPullDir.Size = new System.Drawing.Size(119, 23);
            this.btnPullDir.TabIndex = 22;
            this.btnPullDir.Text = "Change Cloud Dir";
            this.btnPullDir.UseVisualStyleBackColor = true;
            this.btnPullDir.Click += new System.EventHandler(this.btnPullDir_Click_1);
            // 
            // lblPush
            // 
            this.lblPush.AutoSize = true;
            this.lblPush.Location = new System.Drawing.Point(150, 78);
            this.lblPush.Name = "lblPush";
            this.lblPush.Size = new System.Drawing.Size(25, 15);
            this.lblPush.TabIndex = 21;
            this.lblPush.Text = "Dir:";
            // 
            // btnPushDir
            // 
            this.btnPushDir.Location = new System.Drawing.Point(25, 74);
            this.btnPushDir.Name = "btnPushDir";
            this.btnPushDir.Size = new System.Drawing.Size(119, 23);
            this.btnPushDir.TabIndex = 20;
            this.btnPushDir.Text = "Change Local Dir";
            this.btnPushDir.UseVisualStyleBackColor = true;
            this.btnPushDir.Click += new System.EventHandler(this.btnPushDir_Click_1);
            // 
            // lblShowDir
            // 
            this.lblShowDir.AutoSize = true;
            this.lblShowDir.Location = new System.Drawing.Point(150, 45);
            this.lblShowDir.Name = "lblShowDir";
            this.lblShowDir.Size = new System.Drawing.Size(25, 15);
            this.lblShowDir.TabIndex = 19;
            this.lblShowDir.Text = "Dir:";
            // 
            // btnChangeDeleteDir
            // 
            this.btnChangeDeleteDir.Location = new System.Drawing.Point(25, 41);
            this.btnChangeDeleteDir.Name = "btnChangeDeleteDir";
            this.btnChangeDeleteDir.Size = new System.Drawing.Size(119, 23);
            this.btnChangeDeleteDir.TabIndex = 18;
            this.btnChangeDeleteDir.Text = "Change Delete Dir";
            this.btnChangeDeleteDir.UseVisualStyleBackColor = true;
            this.btnChangeDeleteDir.Click += new System.EventHandler(this.btnChangeDeleteDir_Click);
            // 
            // btnSheetGen
            // 
            this.btnSheetGen.Location = new System.Drawing.Point(25, 161);
            this.btnSheetGen.Name = "btnSheetGen";
            this.btnSheetGen.Size = new System.Drawing.Size(140, 23);
            this.btnSheetGen.TabIndex = 26;
            this.btnSheetGen.Text = "Change Sheets-Gen Dir";
            this.btnSheetGen.UseVisualStyleBackColor = true;
            this.btnSheetGen.Click += new System.EventHandler(this.btnSheetGen_Click);
            // 
            // lblGenSheets
            // 
            this.lblGenSheets.AutoSize = true;
            this.lblGenSheets.Location = new System.Drawing.Point(172, 165);
            this.lblGenSheets.Name = "lblGenSheets";
            this.lblGenSheets.Size = new System.Drawing.Size(22, 15);
            this.lblGenSheets.TabIndex = 27;
            this.lblGenSheets.Text = "Dir";
            // 
            // setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblGenSheets);
            this.Controls.Add(this.btnSheetGen);
            this.Controls.Add(this.btnChangeDeleteDir);
            this.Controls.Add(this.lblBackup);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblPull);
            this.Controls.Add(this.btnPullDir);
            this.Controls.Add(this.lblPush);
            this.Controls.Add(this.btnPushDir);
            this.Controls.Add(this.lblShowDir);
            this.Name = "setting";
            this.Text = "setting";
            this.Load += new System.EventHandler(this.setting_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}