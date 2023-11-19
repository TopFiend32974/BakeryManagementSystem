using Delete_Push_Pull.Properties;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;

namespace Delete_Push_Pull
{
    class Delivery
    {
        public static bool CheckDeliveryExists()
        {
            try
            {
                string filePath = (string)Settings.Default["GenSheets"] + @$"\Delivery Route.xlsx";

                // Check if the file already exists
                if (!File.Exists(filePath))
                {
                    // Create a new Excel file
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage())
                    {
                        // Add a new worksheet named "Customer Order Template"
                        var worksheet = package.Workbook.Worksheets.Add("Customer Order Template");

                        // Set the header for Column A and Column K
                        worksheet.Cells["A1"].Value = "Input";
                        worksheet.Cells["K1"].Value = "Output";
                        worksheet.Cells[3,4].Value = "Note:";
                        worksheet.Cells[4,4].Value = "List Customer Route from first drop to last in ascending order.";
                        worksheet.Cells[5,4].Value = "i.e(first drop, next drop … last drop)";
                        worksheet.Cells[6,4].Value = "Column A = input";
                        worksheet.Cells[7,4].Value = "Column K = output";
                        // Save the new Excel file
                        package.SaveAs(new FileInfo(filePath));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
        }


        public static bool FilterAndOutputPriorityList(DayOfWeek selectedDay)
        {
            try
            {
                if (!CheckDeliveryExists())
                {
                    MessageBox.Show("How Did that happen? - ERROR CREATING DELIVERY EXCEL SHEET");
                    return false;
                }
                else{
                    // Load the existing Excel file
                    string filePath = (string)Settings.Default["GenSheets"] + @"\Delivery Route.xlsx";

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var customers = Data.GetInstance().GetCustomers();
                        var ordersByDay = new List<Order>();

                        // Filter orders for the selected day
                        foreach (var customer in customers)
                        {
                            var customerOrders = Data.GetInstance().GetOrders(selectedDay).Where(o => o.Customer == customer);
                            if (customerOrders.Any())
                            {
                                ordersByDay.AddRange(customerOrders);
                            }
                        }
                        ExcelWorksheet worksheet = package.Workbook.Worksheets["Customer Order Template"];


                        // Generate a list of all customers
                        List<Customer> allCustomers = Data.GetInstance().GetCustomers();




                        //Clear Column K after the first row
                        for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                        {
                            worksheet.Cells[row, 11].Clear();
                            worksheet.Cells[row, 17].Clear();
                        }

                        worksheet.Cells["K1"].Value = $"Output: {selectedDay}";

                        // Populate all customers in Column 17 ("reference
                        int startRow = 2; // Start from row 2

                        foreach (var customer in allCustomers)
                        {
                            if (customer.CustomerName != "***")
                            {
                                worksheet.Cells[startRow, 17].Value = customer.CustomerName;
                                startRow++;
                            }
                        }

                        if (ordersByDay.Count > 0)
                        {
                            foreach (var customer in customers)
                            {
                                var customerOrders = Data.GetInstance().GetOrders(selectedDay).Where(o => o.Customer == customer);
                                if (customerOrders.Any())
                                {
                                    ordersByDay.AddRange(customerOrders);
                                }
                            }

                            int srow = 2;
                            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                            {
                                var cellValue = worksheet.Cells[row, 1].Text;
                                bool customerNameAdded = false;

                                // For each cell, check if there's a matching customer in orders
                                foreach (var order in ordersByDay)
                                {
                                    foreach (var orderItem in order.OrderItems)
                                    {
                                        if (orderItem.Order.Customer.CustomerName == cellValue)
                                        {
                                            // Output the name to the desired column (Column K)
                                            worksheet.Cells[srow, 11].Value = orderItem.Order.Customer.CustomerName;
                                            customerNameAdded = true;

                                            break; // Exit the loop if a match is found
                                        }
                                    }

                                    if (customerNameAdded)
                                    {
                                        srow++;
                                        break; // Exit the outer loop if a match is found
                                    }
                                }
                            }
                        }
                        worksheet.Cells.AutoFitColumns();
                        package.Save();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // Handle or log the exception for debugging
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
        }
    }
}
