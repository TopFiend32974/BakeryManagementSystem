using Delete_Push_Pull.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            string GenSheetsDir = (string)Settings.Default["GenSheets"];
            DayOfWeek selectedDay = selectedDayInstance.SelectedDay;
            DeleteSection.DeleteFiles(GenSheetsDir);            
            if (DataValidation.CheckExcel(selectedDay, GenSheetsDir))
            {
                //open folder location
                Process.Start("explorer.exe", GenSheetsDir);
            }          
            
            //DataValidation.CheckDelviery(selectedDay);
            //testingGrounds.GenProductsTotal(selectedDay);
            //DataValidation.CheckCSV(GenSheetsDir);
        }
        public static void LoadProductionHelper()
        {
            string GenProductionDir = (string)Settings.Default["ProductionHelpDir"];
            DayOfWeek selectedDay = selectedDayInstance.SelectedDay;
            //DeleteSection.DeleteFiles(GenProductionDir);

            if (DataValidation.CheckProductionHelper(selectedDay, GenProductionDir)){
                //open file location with file explorer
                Process.Start("explorer.exe", GenProductionDir);
            }
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
    }

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
            //if (!ExcelConversions.OutputCustomerOrdersToExcel(selectedDay, GenSheets))
            //    return false;
            return true;
        }


        public static bool CheckCSV(string GenSheets)
        {
            if (!CSVFiles.OutputProductsToCSV(GenSheets))
                return false;
            return true;
        }
        
        public static bool CheckProductionHelper(DayOfWeek selectedDay, string GenProd)
        {
            if (!ProductionHelp.ProductionHelperMain(selectedDay, GenProd))
                return false;
            
            return true;
        }

        public static bool CheckDelviery(DayOfWeek selectedDay)
        {
            if (!Delivery.FilterAndOutputPriorityList(selectedDay))
                return false; 

            return true;

        }
    }



    
}
