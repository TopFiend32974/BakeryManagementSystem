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
        public static void Delivery(string Local)
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
                        csv.WriteRecord(new { Run = "A", Input = item });
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

    }     
}
