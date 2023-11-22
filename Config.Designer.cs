namespace Delete_Push_Pull
{
    partial class Config
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
            textBoxAddProductID = new TextBox();
            buttonAddProductID = new Button();
            listBoxProductIDs = new ListBox();
            listBoxTrayList = new ListBox();
            buttonDeleteTray = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // textBoxAddProductID
            // 
            textBoxAddProductID.Location = new Point(255, 120);
            textBoxAddProductID.Name = "textBoxAddProductID";
            textBoxAddProductID.Size = new Size(100, 23);
            textBoxAddProductID.TabIndex = 0;
            // 
            // buttonAddProductID
            // 
            buttonAddProductID.Location = new Point(379, 120);
            buttonAddProductID.Name = "buttonAddProductID";
            buttonAddProductID.Size = new Size(137, 23);
            buttonAddProductID.TabIndex = 1;
            buttonAddProductID.Text = "Add Product ID";
            buttonAddProductID.UseVisualStyleBackColor = true;
            // 
            // listBoxProductIDs
            // 
            listBoxProductIDs.FormattingEnabled = true;
            listBoxProductIDs.ItemHeight = 15;
            listBoxProductIDs.Location = new Point(255, 209);
            listBoxProductIDs.Name = "listBoxProductIDs";
            listBoxProductIDs.Size = new Size(120, 94);
            listBoxProductIDs.TabIndex = 3;
            // 
            // listBoxTrayList
            // 
            listBoxTrayList.FormattingEnabled = true;
            listBoxTrayList.ItemHeight = 15;
            listBoxTrayList.Location = new Point(438, 209);
            listBoxTrayList.Name = "listBoxTrayList";
            listBoxTrayList.Size = new Size(120, 94);
            listBoxTrayList.TabIndex = 4;
            // 
            // buttonDeleteTray
            // 
            buttonDeleteTray.Location = new Point(456, 309);
            buttonDeleteTray.Name = "buttonDeleteTray";
            buttonDeleteTray.Size = new Size(75, 23);
            buttonDeleteTray.TabIndex = 5;
            buttonDeleteTray.Text = "Delete Product ID";
            buttonDeleteTray.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(249, 102);
            label1.Name = "label1";
            label1.Size = new Size(106, 15);
            label1.TabIndex = 6;
            label1.Text = "Input ID value here";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(255, 191);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 7;
            label2.Text = "List Names";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(438, 191);
            label3.Name = "label3";
            label3.Size = new Size(71, 15);
            label3.TabIndex = 8;
            label3.Text = "Product IDs:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(438, 335);
            label4.Name = "label4";
            label4.Size = new Size(134, 15);
            label4.TabIndex = 9;
            label4.Text = "Delete a selected ProdID";
            // 
            // Config
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(buttonDeleteTray);
            Controls.Add(listBoxTrayList);
            Controls.Add(listBoxProductIDs);
            Controls.Add(buttonAddProductID);
            Controls.Add(textBoxAddProductID);
            Name = "Config";
            Text = "Config";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxAddProductID;
        private Button buttonAddProductID;
        private ListBox listBoxProductIDs;
        private ListBox listBoxTrayList;
        private Button buttonDeleteTray;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}