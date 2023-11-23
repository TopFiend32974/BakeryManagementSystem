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
    public partial class HiddenFeatures : Form
    {
        public HiddenFeatures()
        {
            InitializeComponent();
        }

        private void btnCSV_Click(object sender, EventArgs e)
        {
            string GenSheets = (string)Settings.Default["GenSheets"];
            if (!DataValidation.CheckCSV(GenSheets))
            {
                MessageBox.Show("CSV Died");
            }
            MessageBox.Show("CSV Created");

        }

        private void btnTestingClass_Click(object sender, EventArgs e)
        {
            DayOfWeek selectedDay = MainClass.selectedDayInstance.SelectedDay;
            testingGrounds.GenProductsTotal(selectedDay);
        }

        private void btnChkDelivery_Click(object sender, EventArgs e)
        {
            DayOfWeek selectedDay = MainClass.selectedDayInstance.SelectedDay;

            if (!DataValidation.CheckDelviery(selectedDay))
            {
                MessageBox.Show("Delivery Died");
            }
            MessageBox.Show("Delivery Created");
        }

        private void btnCustomerExcel_Click(object sender, EventArgs e)
        {
            DayOfWeek selectedDay = MainClass.selectedDayInstance.SelectedDay;
            string GenSheets = (string)Settings.Default["GenSheets"];

            if (!DataValidation.CheckCustomerOutput(GenSheets, selectedDay))
            {
                MessageBox.Show("Customer Excel Died");
            }
            MessageBox.Show("Customer Excel Created");
        }

        private void btnListReturnResult_Click(object sender, EventArgs e)
        {
            DayOfWeek selectedDay = MainClass.selectedDayInstance.SelectedDay;
            //testingGrounds.WriteOrganizedProducts(selectedDay);

            List<CombinedProduct> combinedProducts = testingGrounds.GetCombinedProducts(selectedDay);
            testingGrounds.WriteCombinedProductsToFile(combinedProducts);
        }
    }
}
