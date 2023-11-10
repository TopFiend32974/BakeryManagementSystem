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

    public class jsonData
    {
        string CustomerName { get; set; }
    }

    class DeliveryRoutes
    {

        public static bool CheckJsonExists()
        {
            try
            {
                string filePath = (string)Settings.Default["Local"] + @"\Delivery.json";

                // Check if the file already exists
                if (!File.Exists(filePath))
                {
                    //create new "Delivery.json" file in local folder
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine("{\"RunA\":[],\"RunB\":[]}");
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
        public static void jsonRead(string Local)
        {
            try
            {
                //Get Local Dir
                string deliveryJSONFile = Local + @"\Delivery.json";

                // Read the JSON data
                string jsonContent = File.ReadAllText(deliveryJSONFile);
                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonContent);

                // Specify the path for the CSV file
                string csvFilePath = Path.Combine(Local, "testingDelivery.csv");

                using (var writer = new StreamWriter(csvFilePath))
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    // Loop through "RunA" data and write it to the CSV file
                    foreach (var item in jsonData["RunA"])
                    {
                        //write the record in one column of the csv file
                        csv.WriteRecord(new { Run = "A", Input = item});
                    }

                    // Loop through "RunB" data and write it to the CSV file
                    foreach (var item in jsonData["RunB"])
                    {
                        csv.WriteRecord(new { Run = "B", Input = item });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
            

        }


        public static bool GenerateDeliveryFromJson (DayOfWeek selectedDay, string Local)
        {
            try
            {
                if (!CheckJsonExists())
                {
                    MessageBox.Show("How Did that happen? - ERROR CREATING DELIVERY EXCEL SHEET");
                    return false;
                }
                else
                {
                    // Load the existing Excel file
                    string deliveryJSONFile = Local + @"\Delivery.json";
                    
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
                    // Generate a list of all customers
                    List<Customer> allCustomers = Data.GetInstance().GetCustomers();
                   
                    foreach (var customer in allCustomers)
                    {
                        if (customer.CustomerName != "***")
                        {
                                
                        }
                    }
                    if (ordersByDay.Count > 0)
                    {
                        foreach (var customerOrder in ordersByDay)
                        {
                            // Assuming you have extracted the relevant data from the JSON file and stored it in a variable named 'jsonData'
                            string cellValue = "";//jsonData.CustomerName; Replace 'CustomerName' with the actual field name from your JSON data

                            foreach (var orderItem in customerOrder.OrderItems)
                            {
                                if (orderItem.Order.Customer.CustomerName == cellValue)
                                {
                                    // Update the Excel file with the customer name
                                    // Here, you need to add code to update the Excel file based on the match
                                    // You haven't provided details about your Excel file structure, so you'll need to adapt this part to your specific needs.
                                }
                            }
                        }
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
