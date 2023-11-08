using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
//using Microsoft.Office.Interop.Excel;


namespace Delete_Push_Pull
{
    public partial class SheetSelectionForm : Form
    {

        private Label lblExcelFiles;
        private Label lblSelectedFile;
        private CheckedListBox checkedListBoxSheets;
        private Button btnLoadExcelFiles;
        private ListBox lbExcelFiles;
        private Button btnPrintSelectedSheets;
        private Button btnConfirm;





        private List<string> excelFiles = new List<string>();
        private List<string> selectedSheets = new List<string>();

        public SheetSelectionForm()
        {
            InitializeComponent();
        }

        private void btnLoadExcelFiles_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    excelFiles.Clear();
                    excelFiles.AddRange(openFileDialog.FileNames);

                    // Display the selected Excel files in a ListBox
                    lbExcelFiles.DataSource = excelFiles;
                }
            }
        }

        private void btnPrintSelectedSheets_Click(object sender, EventArgs e)
        {
            if (lbExcelFiles.SelectedItem == null)
            {
                MessageBox.Show("Select an Excel file to print sheets.", "Information");
                return;
            }

            // Prompt the user for sheet selection
            var selectedFile = lbExcelFiles.SelectedItem.ToString();
            selectedSheets.Clear();

            // Add logic here to load the selected sheets into the CheckedListBox control
            checkedListBoxSheets.Items.Clear();

            // Example: Populate checkedListBoxSheets with sheet names from the selected Excel file.
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var excelWorkbook = excelApp.Workbooks.Open(selectedFile);
            foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in excelWorkbook.Sheets)
            {
                checkedListBoxSheets.Items.Add(sheet.Name);
            }
            excelWorkbook.Close();
            excelApp.Quit();

            lblSelectedFile.Text = $"Selected File: {selectedFile}";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (lbExcelFiles.SelectedItem == null)
            {
                MessageBox.Show("Select an Excel file to print sheets.", "Information");
                return;
            }

            var selectedFile = lbExcelFiles.SelectedItem.ToString();
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(selectedFile);

            if (checkedListBoxSheets.CheckedItems.Count == 0)
            {
                MessageBox.Show("Select at least one worksheet to print.", "Information");
            }
            else
            {
                foreach (string sheetName in checkedListBoxSheets.CheckedItems)
                {
                    Microsoft.Office.Interop.Excel.Worksheet sheet = excelWorkbook.Sheets[sheetName];

                    // Generate a file name based on the sheet name
                    string fileName = $"{selectedFile}_{sheetName}.pdf"; // You can change the file format if needed

                    // Print the worksheet and set the file name
                    sheet.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, fileName);
                }
            }

            excelWorkbook.Close();
            excelApp.Quit();
            Marshal.ReleaseComObject(excelWorkbook);
            Marshal.ReleaseComObject(excelApp);

            lblSelectedFile.Text = $"Selected File: {selectedFile}";
            MessageBox.Show("PDF file printed to SheetsDir");
        }


        public List<string> GetSelectedSheets()
        {
            return selectedSheets;
        }
















        private void InitializeComponent()
        {
            lblExcelFiles = new Label();
            lblSelectedFile = new Label();
            checkedListBoxSheets = new CheckedListBox();
            btnLoadExcelFiles = new Button();
            lbExcelFiles = new ListBox();
            btnPrintSelectedSheets = new Button();
            btnConfirm = new Button();
            SuspendLayout();
            // 
            // lblExcelFiles
            // 
            lblExcelFiles.AutoSize = true;
            lblExcelFiles.Location = new Point(289, 9);
            lblExcelFiles.Name = "lblExcelFiles";
            lblExcelFiles.Size = new Size(65, 15);
            lblExcelFiles.TabIndex = 0;
            lblExcelFiles.Text = "Excel Files?";
            // 
            // lblSelectedFile
            // 
            lblSelectedFile.AutoSize = true;
            lblSelectedFile.Location = new Point(537, 9);
            lblSelectedFile.Name = "lblSelectedFile";
            lblSelectedFile.Size = new Size(77, 15);
            lblSelectedFile.TabIndex = 1;
            lblSelectedFile.Text = "Selected File?";
            // 
            // checkedListBoxSheets
            // 
            checkedListBoxSheets.FormattingEnabled = true;
            checkedListBoxSheets.Location = new Point(276, 208);
            checkedListBoxSheets.Name = "checkedListBoxSheets";
            checkedListBoxSheets.Size = new Size(349, 94);
            checkedListBoxSheets.TabIndex = 2;
            // 
            // btnLoadExcelFiles
            // 
            btnLoadExcelFiles.Location = new Point(202, 42);
            btnLoadExcelFiles.Name = "btnLoadExcelFiles";
            btnLoadExcelFiles.Size = new Size(125, 23);
            btnLoadExcelFiles.TabIndex = 3;
            btnLoadExcelFiles.Text = "Load Excel Files";
            btnLoadExcelFiles.UseVisualStyleBackColor = true;
            btnLoadExcelFiles.Click += btnLoadExcelFiles_Click;
            // 
            // lbExcelFiles
            // 
            lbExcelFiles.FormattingEnabled = true;
            lbExcelFiles.ItemHeight = 15;
            lbExcelFiles.Location = new Point(226, 95);
            lbExcelFiles.Name = "lbExcelFiles";
            lbExcelFiles.Size = new Size(463, 94);
            lbExcelFiles.TabIndex = 4;
            // 
            // btnPrintSelectedSheets
            // 
            btnPrintSelectedSheets.Location = new Point(493, 42);
            btnPrintSelectedSheets.Name = "btnPrintSelectedSheets";
            btnPrintSelectedSheets.Size = new Size(161, 23);
            btnPrintSelectedSheets.TabIndex = 5;
            btnPrintSelectedSheets.Text = "Show Sheets";
            btnPrintSelectedSheets.UseVisualStyleBackColor = true;
            btnPrintSelectedSheets.Click += btnPrintSelectedSheets_Click;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(420, 325);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(75, 23);
            btnConfirm.TabIndex = 6;
            btnConfirm.Text = "Print";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // SheetSelectionForm
            // 
            ClientSize = new Size(932, 353);
            Controls.Add(btnConfirm);
            Controls.Add(btnPrintSelectedSheets);
            Controls.Add(lbExcelFiles);
            Controls.Add(btnLoadExcelFiles);
            Controls.Add(checkedListBoxSheets);
            Controls.Add(lblSelectedFile);
            Controls.Add(lblExcelFiles);
            Name = "SheetSelectionForm";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
