using Delete_Push_Pull.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delete_Push_Pull
{
    class internalDaySetter
    {
        public DayOfWeek SelectedDay { get; set; }
    }
    class MainClass
    {
        private static internalDaySetter selectedDayInstance = new internalDaySetter();

        public static string GetDay()
        {
            return selectedDayInstance.SelectedDay.ToString();
        }
        public static void SetDay()
        {
            selectedDayInstance.SelectedDay = DateTime.Now.DayOfWeek;
        }

        [STAThread]
        public static void LoadMainClass()
        {
            ExportData(selectedDayInstance.SelectedDay);
        }
        public static void ShowDaySelectionDialog()
        {
            Form daySelectionForm = new Form();
            daySelectionForm.Text = "Select Day of the Week";
            daySelectionForm.Size = new System.Drawing.Size(200, 150);
            daySelectionForm.StartPosition = FormStartPosition.CenterScreen;

            Label label = new Label();
            label.Text = "Please select the day for export:";
            label.Location = new System.Drawing.Point(10, 10);
            daySelectionForm.Controls.Add(label);

            ComboBox dayComboBox = new ComboBox();
            dayComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            dayComboBox.Items.AddRange(Enum.GetNames(typeof(DayOfWeek)));
            dayComboBox.Location = new System.Drawing.Point(10, 40);
            daySelectionForm.Controls.Add(dayComboBox);

            // Set the default selected day to the current system day
            dayComboBox.SelectedIndex = (int)DateTime.Now.DayOfWeek;

            daySelectionForm.Controls.Add(dayComboBox);


            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new System.Drawing.Point(10, 70);
            okButton.Click += (sender, e) =>
            {

                DayOfWeek selectedDay = (DayOfWeek)dayComboBox.SelectedIndex;
                selectedDayInstance.SelectedDay = selectedDay;

                daySelectionForm.Close();

                //ExportData(selectedDayInstance);
            };
            daySelectionForm.Controls.Add(okButton);

            daySelectionForm.ShowDialog();
        }


        public static void ExportData(DayOfWeek selectedDay)
        {

            //Passes through directories
            string BackupDir = (string)Settings.Default["BackupDir"];
            string PushtoCloud = (string)Settings.Default["Local"];
            string PulltoLocal = (string)Settings.Default["Cloud"];
            string GenSheetsDir = (string)Settings.Default["GenSheets"];
            string LocalLabelsDir = (string)Settings.Default["DeleteDir"];

            //DeliveryRoutes.Delivery(PushtoCloud);
            ProductionHelp.ProductionHelperMain(selectedDay, GenSheetsDir);
            //DataValidation.CheckCSV(GenSheetsDir);

            if (DataValidation.CheckExcel(selectedDay, GenSheetsDir))
            {
                MessageBox.Show(selectedDay + " Excel Sheets Generated.");
            }
            else
            {
                MessageBox.Show("Excel Did not work.");

            }
            //GoogleAPI.GoogleCredientials(selectedDay);

        }

    }


    //---------------------------------//



    internal class DataValidation
    {


        public static bool CheckExcel(DayOfWeek selectedDay, string GenSheets)
        {
            // Check each method and return false if any method fails
            if (!ExcelConversions.GenerateMatrixReport(selectedDay, GenSheets))
                return false;
            
            if (!ExcelConversions.GenerateBreadSortedSheet(selectedDay, GenSheets))
                return false;
            if (!ExcelConversions.GeneratePartBakePastyCocktailReport(selectedDay, GenSheets))
                return false;
            if (!ExcelConversions.GenerateBreadReport(selectedDay, GenSheets))
                return false;
            if (!ExcelConversions.GenerateFrozenReport(selectedDay, GenSheets))
                return false;
            if (!ExcelConversions.GenerateBapReport(selectedDay, GenSheets))
                return false;
            if (!ExcelConversions.GenerateCakeReport(selectedDay, GenSheets))
                return false;
            if (!DeliveryRoutes.FilterAndOutputPriorityList(selectedDay))
                return false;
            //if (!Delivery.FilterAndOutputPriorityList(selectedDay))
            //    return false;
            //if (!ExcelConversions.OutputCustomerOrdersToExcel(selectedDay, GenSheets))
            //    return false;
            //If all methods executed successfully, return true
            return true;
        }


        public static bool CheckCSV(string GenSheets)
        {
            if (!CSVFiles.OutputProductsToCSV(GenSheets))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }



    
}
