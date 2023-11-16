using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Delete_Push_Pull.Properties;
using OfficeOpenXml;

namespace Delete_Push_Pull
{
    internal class CSVFiles
    {
        public static bool OutputProductsToCSV(string GenSheets)
        {
            try
            {

                List<Product> products = Data.GetInstance().GetProducts();

                if (products.Count == 0)
                {
                    MessageBox.Show("No products to export.");
                    return false;
                }

                string outputFilePath = GenSheets + @"\Products.csv";

                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    writer.WriteLine("ProductId,ProductName,ProductClass,MarkupClass,BatchSize,ProductType,PackSize,SourceProductId");

                    foreach (var product in products)
                    {
                        string productLine = $"{product.ProductId},{product.ProductName},{product.ProductClass},{product.MarkupClass},{product.BatchSize},{product.ProductType},{product.PackSize},{product.SourceProductId}";
                        writer.WriteLine(productLine);
                    }

                    //MessageBox.Show($"{products.Count} Products exported to .CSV");
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }
    }  

    class DeliveryRoutes
    {
        public static bool FilterAndOutputPriorityList(DayOfWeek selectedDay)
        {
            try
            {
                var jsonFilePath = (string)Settings.Default["Local"] + @"\CustomerDeliveryRuns.json"; // Change to your actual JSON file path
                var outputFilePath = (string)Settings.Default["GenSheets"] + @"\delivery.txt";

                ExcelConversions.ExcelDeleteOriginalFile(outputFilePath);

                // Read customer delivery runs from JSON
                Dictionary<string, List<string>> customerRuns = ReadJsonFile(jsonFilePath);

                // Check if the "RunA" key exists
                if (customerRuns.ContainsKey("RunA"))
                {
                    List<string> runACustomers = customerRuns["RunA"];

                    // Filter orders for the selected day
                    var ordersByDay = Data.GetInstance().GetOrders(selectedDay);

                    // Create a list to store customer names in the order they appear in the JSON file
                    List<string> customerNamesInOrder = new List<string>();

                    // Iterate through customer names in the JSON order
                    foreach (var customerName in runACustomers)
                    {
                        // Check if the customer has orders for the selected day
                        if (ordersByDay.Any(o => o.OrderItems.Any(oi => oi.Order.Customer.CustomerName == customerName)))
                        {
                            customerNamesInOrder.Add(customerName);
                        }
                    }

                    // Write the customer names to the "delivery.txt" file
                    File.WriteAllLines(outputFilePath, customerNamesInOrder);

                   // MessageBox.Show($"Delivery list for {selectedDay} exported to {outputFilePath}");
                }
                else
                {
                    MessageBox.Show("The specified delivery run key does not exist in the JSON file.");
                    return false;
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

        private static Dictionary<string, List<string>> ReadJsonFile(string filePath)
        {
            try
            {
                // Read JSON content from file
                string jsonContent = File.ReadAllText(filePath);

                // Deserialize JSON into a dictionary
                var customerRuns = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonContent);

                return customerRuns;
            }
            catch (Exception ex)
            {
                // Handle or log the exception for debugging
                MessageBox.Show("An error occurred while reading the JSON file: " + ex.Message);
                return null;
            }
        }


    }
}
