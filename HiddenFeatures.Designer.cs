namespace Delete_Push_Pull
{
    partial class HiddenFeatures
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
            btnCSV = new Button();
            btnTestingClass = new Button();
            btnChkDelivery = new Button();
            btnCustomerExcel = new Button();
            SuspendLayout();
            // 
            // btnCSV
            // 
            btnCSV.Location = new Point(257, 88);
            btnCSV.Name = "btnCSV";
            btnCSV.Size = new Size(146, 23);
            btnCSV.TabIndex = 0;
            btnCSV.Text = "CSV Product Output";
            btnCSV.UseVisualStyleBackColor = true;
            btnCSV.Click += btnCSV_Click;
            // 
            // btnTestingClass
            // 
            btnTestingClass.Location = new Point(257, 128);
            btnTestingClass.Name = "btnTestingClass";
            btnTestingClass.Size = new Size(212, 23);
            btnTestingClass.TabIndex = 1;
            btnTestingClass.Text = "Output TestingGround Class";
            btnTestingClass.UseVisualStyleBackColor = true;
            btnTestingClass.Click += btnTestingClass_Click;
            // 
            // btnChkDelivery
            // 
            btnChkDelivery.Location = new Point(257, 173);
            btnChkDelivery.Name = "btnChkDelivery";
            btnChkDelivery.Size = new Size(201, 23);
            btnChkDelivery.TabIndex = 2;
            btnChkDelivery.Text = "Excel Delivery";
            btnChkDelivery.UseVisualStyleBackColor = true;
            btnChkDelivery.Click += btnChkDelivery_Click;
            // 
            // btnCustomerExcel
            // 
            btnCustomerExcel.Location = new Point(257, 224);
            btnCustomerExcel.Name = "btnCustomerExcel";
            btnCustomerExcel.Size = new Size(206, 23);
            btnCustomerExcel.TabIndex = 3;
            btnCustomerExcel.Text = "Excel Customer output";
            btnCustomerExcel.UseVisualStyleBackColor = true;
            btnCustomerExcel.Click += btnCustomerExcel_Click;
            // 
            // HiddenFeatures
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCustomerExcel);
            Controls.Add(btnChkDelivery);
            Controls.Add(btnTestingClass);
            Controls.Add(btnCSV);
            Name = "HiddenFeatures";
            Text = "HiddenFeatures";
            ResumeLayout(false);
        }

        #endregion

        private Button btnCSV;
        private Button btnTestingClass;
        private Button btnChkDelivery;
        private Button btnCustomerExcel;
    }
}