using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delete_Push_Pull
{
    internal class mdbToCSV
    {
        public mdbToCSV() {
            // Specify file paths for ODR, BreadSorted, and the output
            string odrFilePath = @"C:\Users\jamie\OneDrive\Bakery Master\Bakery-Applications\23-3-1 - Copy (2)\Local\ODR171.ODR";
            string breadSortedFilePath = @"C:\Users\jamie\OneDrive\Desktop\example\output.csv";
            string outputFilePathe = @"C:\Users\jamie\OneDrive\Desktop\example";

            string csvCondensed = @"C:\Users\jamie\OneDrive\Desktop\example\output.csv";
            string outputDirectory = @"C:\Users\jamie\OneDrive\Desktop\example\";
            string BreadinputFilePath = csvCondensed;
            string BreadoutputFilePat = Path.Combine(outputDirectory, "BreadSorted.csv");


            CSV.CSVMethod();
            CSV.BreadSlicedSorted(BreadinputFilePath, BreadoutputFilePat);

            // Call the BreadCSVPostSAMPLES method to combine data
            CSV.BreadCSVPostSAMPLES(odrFilePath, breadSortedFilePath, outputFilePathe);

        }
                  
        
    }

    class CSV
    {

        public static void CSVMethod()
        {
            string directoryPath = "C:\\Users\\jamie\\OneDrive\\Bakery Master\\Bakery-Applications\\23-3-1 - Copy (2)\\Local\\labels"; // Change this to the directory where your .mdb files are located
            string outputPath = "C:\\Users\\jamie\\OneDrive\\Bakery Master\\Bakery-Applications\\23-3-1 - Copy (2)\\Local\\labels\\Example.csv"; // Change this to the desired output CSV file path

            // Create a StringBuilder to store the CSV data
            StringBuilder csvData = new StringBuilder();

            // Define the CSV header
            csvData.AppendLine("Product ID,Name,Items to print");

            // Get a list of .mdb files in the directory
            string[] mdbFiles = Directory.GetFiles(directoryPath, "*.mdb");

            foreach (string mdbFile in mdbFiles)
            {
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={mdbFile};";

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();


                    string sqlQuery = @"
                        SELECT 
                            P.[Product ID],
                            P.[Name],
                            ITP.[Labels to print]
                        FROM 
                            [LABELS] AS P
                        LEFT JOIN 
                            [Items to print] AS ITP
                        ON P.[Product ID] = ITP.[item]
                        WHERE ITP.[Labels to print] IS NOT NULL";

                    using (OleDbCommand command = new OleDbCommand(sqlQuery, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int productID = reader.GetInt32(0);

                                object nameObject = reader.GetValue(1);
                                string name = nameObject.ToString();

                                object itemstoPrintObject = reader.GetValue(2);
                                string itemsToPrint = itemstoPrintObject.ToString();

                                // Format the row with the desired delimiter (pipe symbol "|")
                                string csvRow = $"{productID},{name},{itemsToPrint}";

                                // Append the row to the CSV data
                                csvData.AppendLine(csvRow);
                            }
                        }
                    }

                    connection.Close();
                }
            }

            // Write the CSV data to the output file
            File.WriteAllText(outputPath, csvData.ToString());

            Console.WriteLine($"CSV data has been written to {outputPath}");


            string help = "C:\\Users\\jamie\\OneDrive\\Desktop\\example";
            CondenseCSV(outputPath, help);

            // Display a message indicating the operation is complete
            MessageBox.Show("CSV data has been condensed and saved to output.csv");
        }



        //----------------------------------------------------//






        public static void CondenseCSV(string inputFilePath, string outputFilePath)
        {
            // Read the input CSV file into a list of strings
            List<string> lines = File.ReadAllLines(inputFilePath).ToList();

            // Create a dictionary to store the product names (Product ID -> Name)
            Dictionary<string, string> productNames = new Dictionary<string, string>();

            // Create a dictionary to store the condensed data (Product ID -> Total Quantity)
            Dictionary<string, decimal> condensedData = new Dictionary<string, decimal>();

            // Loop through each line in the CSV file
            foreach (string line in lines)
            {
                // Split the line by the delimiter (e.g., comma) to access columns
                string[] columns = line.Split(',');

                // Ensure there are at least three columns (Product ID, Name, and Quantity)
                if (columns.Length >= 3)
                {
                    string productID = columns[0].Trim();
                    string productName = columns[1].Trim();

                    // Attempt to parse the string into a decimal; handle parsing errors gracefully
                    if (decimal.TryParse(columns[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal quantity))
                    {
                        // If product ID is not in the dictionary, add it with the value of quantity
                        if (!condensedData.ContainsKey(productID))
                        {
                            condensedData[productID] = quantity;
                            // Store the product name in the dictionary
                            productNames[productID] = productName;
                        }
                        else
                        {
                            // If product ID is already in the dictionary, add the value of quantity to the existing total
                            condensedData[productID] += quantity;
                        }
                    }
                    else
                    {
                        // Handle parsing error: You can log an error message or take appropriate action
                        Console.WriteLine($"Error parsing quantity in column 3: {columns[2].Trim()}");
                    }
                }
            }

            // Create a new list to store the condensed CSV data
            List<string> condensedLines = new List<string>();

            // Convert the condensed data back to CSV format, including the product name
            foreach (var kvp in condensedData)
            {
                string productID = kvp.Key;
                string productName = productNames[productID];
                decimal quantity = kvp.Value;

                // Create the condensed line with the desired format
                string condensedLine = $"{productID},{productName},{quantity}";
                condensedLines.Add(condensedLine);
            }


            string outputDirectory = @"C:\Users\jamie\OneDrive\Desktop\example";

            string outputFilePat = Path.Combine(outputDirectory, "output.csv");

            try
            {
                File.WriteAllLines(outputFilePat, condensedLines, Encoding.UTF8);
                MessageBox.Show("File Created Successfully");
                //Console.WriteLine("File created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file: {ex.Message}");
            }


            

            



        }



        //------------------------------//




        public static void BreadSlicedSorted(string inputFilePath, string outputFilePath)
        {
            // Read the input CSV file into a list of strings
            List<string> lines = File.ReadAllLines(inputFilePath).ToList();

            // Create a list to store the filtered products
            List<string> filteredLines = new List<string>();

            // Loop through each line in the CSV file
            foreach (string line in lines)
            {
                // Split the line by the delimiter (e.g., comma) to access columns
                string[] columns = line.Split(',');

                // Ensure there are at least two columns (Product ID and Name)
                if (columns.Length >= 2)
                {
                    string productName = columns[1].Trim();

                    // Check if the product name contains "Sliced" or "slice" (case-insensitive)
                    if (productName.IndexOf("Sliced", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        productName.IndexOf("slice", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        productName.IndexOf("doorstep", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // If the product name contains the keyword, add it to the filtered list
                        filteredLines.Add(line);
                    }
                }
            }

            // Sort the filtered products by ProductID in ascending order
            filteredLines.Sort((a, b) =>
            {
                // Extract Product IDs for comparison
                int productIDA = int.Parse(a.Split(',')[0]);
                int productIDB = int.Parse(b.Split(',')[0]);

                // Compare Product IDs
                return productIDA.CompareTo(productIDB);
            });

            // Write the filtered and sorted data to the output CSV file
            File.WriteAllLines(outputFilePath, filteredLines, Encoding.UTF8);

            
        }





        //------------------------//





        public static void BreadCSVPostSAMPLES(string odrFilePath, string breadSortedFilePath, string outputFilePath)
        {
            // Read the ODR file to extract product IDs and quantities
            List<string> odrLines = File.ReadAllLines(odrFilePath).ToList();

            // Create a dictionary to store ODR data (ProductID -> Quantity)
            Dictionary<int, int> odrData = new Dictionary<int, int>();

            // Initialize variables to keep track of Product ID and Quantity
            int currentProductID = 0;
            int currentQuantity = 0;

            // Initialize a flag to track whether the line is a ProductID or a Quantity
            bool isProductIDLine = true;

            foreach (string line in odrLines.Skip(1)) // Skip the first line
            {
                if (isProductIDLine)
                {
                    // Read ProductID (first line in pair)
                    if (int.TryParse(line, out currentProductID))
                    {
                        isProductIDLine = false; // Switch to Quantity line
                    }
                }
                else
                {
                    // Read Quantity (second line in pair)
                    if (int.TryParse(line, out currentQuantity))
                    {
                        // Check if the quantity is greater than -1
                        if (currentQuantity > -1)
                        {
                            break; // Stop reading pairs if the condition is met
                        }

                        // Store the Quantity in the dictionary
                        odrData[currentProductID] = currentQuantity;

                        // Reset the variables for the next ProductID-Quantity pair
                        currentProductID = 0;
                        currentQuantity = 0;
                        isProductIDLine = true;
                    }
                }
            }

            // Read the existing BreadSorted CSV file
            List<string> breadSortedLines = File.ReadAllLines(breadSortedFilePath).ToList();

            // Create a list to store the combined data
            List<string> combinedLines = new List<string>();

            foreach (string line in breadSortedLines)
            {
                // Split the line by the delimiter (e.g., comma) to access columns
                string[] columns = line.Split(',');

                // Ensure there are at least two columns (Product ID and Name)
                if (columns.Length >= 2)
                {
                    int productID;

                    if (int.TryParse(columns[0].Trim(), out productID))
                    {
                        // Check if the product ID exists in the ODR data
                        if (odrData.ContainsKey(productID))
                        {
                            // Append the quantity from the ODR data
                            int quantity = odrData[productID];
                            string combinedLine = $"{productID},{columns[1].Trim()},{quantity}";
                            combinedLines.Add(combinedLine);
                        }
                        else
                        {
                            // If not found in ODR, keep the line as is
                            combinedLines.Add(line);
                        }
                    }
                }
            }

            string outputDirectory = @"C:\Users\jamie\OneDrive\Desktop\example";
            outputFilePath = Path.Combine(outputDirectory, "PostSAMPLESbread.csv");

            if (File.Exists(outputFilePath))
            {
                // The file already exists, you can choose to overwrite it or handle it as needed.
                File.Delete(outputFilePath); // This line will delete the existing file.
            }

            // Now create the directory and write the file
            Directory.CreateDirectory(outputDirectory);
            File.WriteAllLines(outputFilePath, combinedLines, Encoding.UTF8);


        }
    }
}
