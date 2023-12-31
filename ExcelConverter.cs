﻿using Delete_Push_Pull.Properties;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Delete_Push_Pull
{
    internal class ExcelConversions
    {


        public static bool GenerateMatrixReport(DayOfWeek selectedDay, string GenSheets)
        {

            try
            {
                // Specify the output Excel file path
                string outputFilePath = GenSheets + $@"\MyMatrix_{selectedDay}.xlsx";
                //ExcelDeleteOriginalFile(outputFilePath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(selectedDay.ToString());

                    var ordersByDay = Data.GetInstance().GetOrders(selectedDay);

                    // Freeze the first row and column B
                    worksheet.View.FreezePanes(2, 3);

                    if (ordersByDay.Count > 0)
                    {
                        // Create a header row
                        worksheet.Cells["A1"].Value = "ID";
                        worksheet.Cells["B1"].Value = "Prod Name";

                        var uniqueCustomers = ordersByDay.SelectMany(o => o.OrderItems.Select(oi => oi.Order.Customer))
                            .Distinct()
                            .OrderBy(c => c.CustomerID)
                            .ToList();

                        int col = 3; // Start from column C
                        int lastCol = col + uniqueCustomers.Count - 1;

                        // Set the customer names in the header row at an angle
                        foreach (var customer in uniqueCustomers)
                        {
                            var cell = worksheet.Cells[1, col];
                            cell.Value = customer.CustomerName;
                            cell.Style.TextRotation = 90; // Rotate text clockwise by 90 degrees
                            col++;
                        }

                        worksheet.Cells[1, lastCol + 1].Value = "Quantity"; // Column for Quantity

                        var uniqueProducts = ordersByDay.SelectMany(o => o.OrderItems.Select(oi => oi.Product))
                            .Distinct()
                            .OrderBy(p => p.ProductId)
                            .ToList();

                        int row = 2;

                        foreach (var product in uniqueProducts)
                        {
                            worksheet.Cells[row, 1].Value = product.ProductId;
                            worksheet.Cells[row, 2].Value = product.ProductName;

                            col = 3;

                            foreach (var customer in uniqueCustomers)
                            {
                                var quantity = ordersByDay.SelectMany(o => o.OrderItems)
                                    .Where(oi => oi.Product.ProductId == product.ProductId && oi.Order.Customer == customer)
                                    .Sum(oi => oi.Quantity);

                                if (quantity != 0)
                                {
                                    worksheet.Cells[row, col].Value = quantity;
                                }
                                else
                                {
                                    worksheet.Cells[row, col].Value = null;
                                }

                                col++;
                            }


                            worksheet.Cells[row, lastCol + 1].Formula = $"SUM(C{row}:{ConvertToLetter(lastCol)}{row})"; // Dynamic SUM formula
                            row++;
                        }

                    }
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(0);
                    
                    //AlternateColumnColors(worksheet);
                    AdjustExcelPrint(worksheet);
                    package.SaveAs(new FileInfo(outputFilePath));
                    //MessageBox.Show($"MyMatrix for {selectedDay} exported to {outputFilePath}");
                   

                }

                return true; // Method executed successfully
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                // Handle exceptions and return false on failure
                return false;
            }

        }


        public static bool OutputCustomerOrdersToExcel(DayOfWeek selectedDay, string GenSheets)
        {

            try
            {
                // Specify the output Excel file path
                string outputFilePath = GenSheets + $@"\MyMatrix_{selectedDay}.xlsx";

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(outputFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Customer Orders");
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

                    if (ordersByDay.Count > 0)
                    {
                        // Create a new worksheet for the selected day
                        //var worksheet = package.Workbook.Worksheets.Add(selectedDay.ToString());

                        // Set the date for the specific order day
                        string date = DateTime.Now.AddDays((int)selectedDay - (int)DateTime.Now.DayOfWeek).ToShortDateString();

                        worksheet.Cells["A1"].Value = "Customer ID";
                        worksheet.Cells["B1"].Value = "Customer Name";
                        worksheet.Cells["C1"].Value = "Date";
                        worksheet.Cells["D1"].Value = "Day";
                        worksheet.Cells["E1"].Value = "Product ID";
                        worksheet.Cells["F1"].Value = "Product Name";
                        worksheet.Cells["G1"].Value = "Quantity";

                        int row = 2;

                        foreach (var order in ordersByDay)
                        {
                            foreach (var orderItem in order.OrderItems)
                            {
                                worksheet.Cells[row, 1].Value = orderItem.Order.Customer.CustomerID;
                                worksheet.Cells[row, 2].Value = orderItem.Order.Customer.CustomerName;
                                worksheet.Cells[row, 3].Value = date;
                                worksheet.Cells[row, 4].Value = selectedDay;
                                worksheet.Cells[row, 5].Value = orderItem.Product.ProductId;
                                worksheet.Cells[row, 6].Value = orderItem.Product.ProductName;
                                worksheet.Cells[row, 7].Value = orderItem.Quantity;

                                row++;
                            }
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    }

                    package.Save();
                    //MessageBox.Show($"Customer orders for {selectedDay} exported to {outputFilePath}");

                    return true; // Method executed successfully
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // Handle exceptions and return false on failure
                return false;
            }


        }

        // Helper function to convert a number to a column letter (e.g., 1 -> A, 27 -> AA)
        public static string ConvertToLetter(int colNumber)
        {
            int dividend = colNumber;
            string columnName = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        //----------------------//

        public static bool GenerateBreadSortedSheet(DayOfWeek selectedDay, string GenSheets)
        {
            try
            {
                // Specify the output Excel file path
                string outputFilePath = GenSheets + $@"\MyMatrix_{selectedDay}.xlsx";
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


                using (var package = new ExcelPackage(new FileInfo(outputFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Bread Sorted");

                    var ordersByDay = Data.GetInstance().GetOrders(selectedDay);
                    worksheet.View.FreezePanes(2, 3);

                    if (ordersByDay.Count > 0)
                    {
                        // Create a header row
                        worksheet.Cells["A1"].Value = "ID";
                        worksheet.Cells["B1"].Value = "Prod Name";
                        worksheet.Cells["C1"].Value = "Total";
                        worksheet.Cells["D1"].Value = "Bread Sorted";
                        worksheet.Cells["E1"].Value = "Remaining";
                        worksheet.Cells["F1"].Value = "Completed?";
                        worksheet.Cells["G1"].Value = "Hot Baskets";
                        worksheet.Cells["H1"].Value = "Hot Loaves";
                        worksheet.Cells["I1"].Value = "Cold Baskets";
                        worksheet.Cells["J1"].Value = "Cold Loaves";


                        var uniqueProducts = new List<(int ProductId, string ProductName)>();

                        foreach (var order in ordersByDay)
                        {
                            foreach (var orderItem in order.OrderItems)
                            {
                                var product = (orderItem.Product.ProductId, orderItem.Product.ProductName);

                                // Check if the product name contains "sliced" or "doorstep"
                                if (product.ProductName.Contains("SLICE", StringComparison.OrdinalIgnoreCase) ||
                                    product.ProductName.Contains("DOORSTEP", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Check if the product is already in the list; if not, add it
                                    if (!uniqueProducts.Contains(product))
                                    {
                                        uniqueProducts.Add(product);
                                    }
                                }
                            }
                        }

                        // Sort the unique products by ProductID in ascending order
                        uniqueProducts = uniqueProducts.OrderBy(p => p.ProductId).ToList();

                        int row = 2;

                        foreach (var (ProductId, ProductName) in uniqueProducts)
                        {
                            int quantity = ordersByDay.SelectMany(o => o.OrderItems)
                                .Where(oi => oi.Product.ProductId == ProductId)
                                .Sum(oi => oi.Quantity);

                            worksheet.Cells[row, 1].Value = ProductId;
                            worksheet.Cells[row, 2].Value = ProductName;
                            worksheet.Cells[row, 3].Value = quantity;
                            worksheet.Cells[row, 4].Value = 0;
                            worksheet.Cells[row, 5].Formula = $"C{row} - D{row}";
                            // I1 is hot sort (whole number of baskets)
                            worksheet.Cells[row, 7].Formula = $"INT(E{row} / 6)";

                            // I1 is the remaining loaves after hot sort
                            worksheet.Cells[row, 8].Formula = $"MOD(E{row}, 6)";

                            // J1 is cold sort (whole number of baskets)
                            worksheet.Cells[row, 9].Formula = $"INT(E{row} / 10)";

                            // K1 is the remaining loaves after cold sort
                            worksheet.Cells[row, 10].Formula = $"MOD(E{row}, 10)";
                            row++;
                        }                        
                        int lastRow = worksheet.Cells[worksheet.Dimension.Address].End.Row;                       
                        worksheet.Cells["F2"].Formula = "IF(C2-D2=0, \"True\", \"False\")";
                        for (int Trow = 2; Trow <= lastRow; Trow++)
                        {
                            worksheet.Cells[Trow, 6].Formula = $"IF(C{Trow}-D{Trow}=0, \"True\", \"False\")";
                        }
                    }

                    //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    AlternateRowColors(worksheet);
                    AdjustExcelPrint(worksheet);
                    package.Save();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        public static bool GeneratePartBakePastyCocktailReport(DayOfWeek selectedDay, string GenSheets)
        {

            try
            {
                // Specify the output Excel file path for the Part Bake Pasty Cocktail report
                string outputFilePath = GenSheets + $@"\MyMatrix_{selectedDay}.xlsx";

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(outputFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Pasty Matrix");

                    var ordersByDay = Data.GetInstance().GetOrders(selectedDay);

                    if (ordersByDay.Count > 0)
                    {
                        // Create a header row
                        worksheet.Cells["A1"].Value = "ID";
                        worksheet.Cells["B1"].Value = "Prod Name";

                        // Filter unique customers who have ordered pasties (part bake pasty cocktail products)
                        var uniqueCustomers = ordersByDay
                            .SelectMany(o => o.OrderItems
                                .Where(oi => IsPartBakePastyCocktailProduct(oi.Product.ProductName.ToLower()))
                                .Select(oi => oi.Order.Customer))
                            .Distinct()
                            .OrderBy(c => c.CustomerID)
                            .ToList();

                        int col = 3; // Start from column C
                        int lastCol = col + uniqueCustomers.Count - 1;

                        // Set the customer names in the header row at an angle
                        foreach (var customer in uniqueCustomers)
                        {
                            var cell = worksheet.Cells[1, col];
                            cell.Value = customer.CustomerName;
                            cell.Style.TextRotation = 90; // Rotate text clockwise by 90 degrees
                            col++;
                        }

                        worksheet.Cells[1, lastCol + 1].Value = "Quantity"; // Column for Quantity

                        var uniqueProducts = ordersByDay.SelectMany(o => o.OrderItems.Select(oi => oi.Product))
                            .Distinct()
                            .OrderBy(p => p.ProductId)
                            .ToList();

                        int row = 2;

                        foreach (var product in uniqueProducts)
                        {
                            // Check if the product is part bake, pasty, or cocktail
                            if (IsPartBakePastyCocktailProduct(product.ProductName.ToLower()))
                            {
                                worksheet.Cells[row, 1].Value = product.ProductId;
                                worksheet.Cells[row, 2].Value = product.ProductName;

                                col = 3;

                                foreach (var customer in uniqueCustomers)
                                {
                                    var quantity = ordersByDay.SelectMany(o => o.OrderItems)
                                        .Where(oi => oi.Product.ProductId == product.ProductId && oi.Order.Customer == customer)
                                        .Sum(oi => oi.Quantity);

                                    if (quantity != 0)
                                    {
                                        worksheet.Cells[row, col].Value = quantity;
                                    }
                                    else
                                    {
                                        worksheet.Cells[row, col].Value = null;
                                    }

                                    col++;
                                }

                                worksheet.Cells[row, lastCol + 1].Formula = $"SUM(C{row}:{ConvertToLetter(lastCol)}{row})"; // Dynamic SUM formula
                                row++;
                            }
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(0);
                    }
                    AdjustExcelPrint(worksheet);
                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.FitToWidth = 1;
                    //freeze row 1 and column A
                    worksheet.View.FreezePanes(2, 3);
                    package.Save();
                }


                return true; // Method executed successfully
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                // Handle exceptions and return false on failure
                return false;
            }



        }
        // Helper function to check if a product is "part bake," "pasty," or "cocktail"
        private static bool IsPartBakePastyCocktailProduct(string productName)
        {
            string[] keywords = { "part bake", "pas", "cocktail" };
            return keywords.Any(keyword => productName.ToLower().Contains(keyword));
        }



        public static bool GenerateBreadReport(DayOfWeek selectedDay, string GenSheets)
        {
            try
            {
                string outputFilePath = GenSheets + $@"\MyMatrix_{selectedDay}.xlsx";

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(outputFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Bread Matrix");

                    var ordersByDay = Data.GetInstance().GetOrders(selectedDay);

                    if (ordersByDay.Count > 0)
                    {
                        // Create a header row
                        worksheet.Cells["A1"].Value = "ID";
                        worksheet.Cells["B1"].Value = "Prod Name";

                        var uniqueCustomers = ordersByDay
                            .SelectMany(o => o.OrderItems
                                .Where(oi => ContainsBreadKeywords(oi.Product.ProductName.ToLower()))
                                .Select(oi => oi.Order.Customer))
                            .Distinct()
                            .OrderBy(c => c.CustomerID)
                            .ToList();

                        int col = 3; // Start from column C
                        int lastCol = col + uniqueCustomers.Count - 1;

                        // Set the customer names in the header row at an angle
                        foreach (var customer in uniqueCustomers)
                        {
                            var cell = worksheet.Cells[1, col];
                            cell.Value = customer.CustomerName;
                            cell.Style.TextRotation = 90; // Rotate text clockwise by 90 degrees
                            col++;
                        }

                        worksheet.Cells[1, lastCol + 1].Value = "Quantity"; // Column for Quantity

                        var uniqueProducts = ordersByDay.SelectMany(o => o.OrderItems.Select(oi => oi.Product))
                            .Distinct()
                            .OrderBy(p => p.ProductId)
                            .ToList();

                        int row = 2;

                        foreach (var product in uniqueProducts)
                        {
                            // Check if the product is part bake, pasty, or cocktail
                            if (ContainsBreadKeywords(product.ProductName))
                            {
                                worksheet.Cells[row, 1].Value = product.ProductId;
                                worksheet.Cells[row, 2].Value = product.ProductName;

                                col = 3;

                                foreach (var customer in uniqueCustomers)
                                {
                                    var quantity = ordersByDay.SelectMany(o => o.OrderItems)
                                        .Where(oi => oi.Product.ProductId == product.ProductId && oi.Order.Customer == customer)
                                        .Sum(oi => oi.Quantity);

                                    if (quantity != 0)
                                    {
                                        worksheet.Cells[row, col].Value = quantity;
                                    }
                                    else
                                    {
                                        worksheet.Cells[row, col].Value = null;
                                    }

                                    col++;
                                }

                                worksheet.Cells[row, lastCol + 1].Formula = $"SUM(C{row}:{ConvertToLetter(lastCol)}{row})"; // Dynamic SUM formula
                                row++;
                            }
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(0);
                    }
                    AdjustExcelPrint(worksheet);
                    package.Save();
                    
                    //MessageBox.Show($"Bread Report for {selectedDay} exported to {outputFilePath}");
                }

                return true; // Method executed successfully
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                // Handle exceptions and return false on failure
                return false;
            }
        }

        // Helper function to check if a product name contains bread-related keywords
        private static bool ContainsBreadKeywords(string productName)
        {
            string[] keywords = { "large", "slice", "sliced", "doorstep", "bloomer", "lge", "sq" };
            return keywords.Any(keyword => productName.ToLower().Contains(keyword));
        }



        public static bool GenerateFrozenReport(DayOfWeek selectedDay, string GenSheets)
        {
            try
            {
                string outputFilePath = GenSheets + $@"\MyMatrix_{selectedDay}.xlsx";

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(outputFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Frozen Matrix");

                    var ordersByDay = Data.GetInstance().GetOrders(selectedDay);

                    if (ordersByDay.Count > 0)
                    {
                        // Create a header row
                        worksheet.Cells["A1"].Value = "ID";
                        worksheet.Cells["B1"].Value = "Prod Name";

                        var uniqueCustomers = ordersByDay
                            .SelectMany(o => o.OrderItems
                                .Where(oi => ContainsFrozenKeywords(oi.Product.ProductName.ToLower()))
                                .Select(oi => oi.Order.Customer))
                            .Distinct()
                            .OrderBy(c => c.CustomerID)
                            .ToList();

                        int col = 3; // Start from column C
                        int lastCol = col + uniqueCustomers.Count - 1;

                        // Set the customer names in the header row at an angle
                        foreach (var customer in uniqueCustomers)
                        {
                            var cell = worksheet.Cells[1, col];
                            cell.Value = customer.CustomerName;
                            cell.Style.TextRotation = 90; // Rotate text clockwise by 90 degrees
                            col++;
                        }

                        worksheet.Cells[1, lastCol + 1].Value = "Quantity"; // Column for Quantity

                        var uniqueProducts = ordersByDay.SelectMany(o => o.OrderItems.Select(oi => oi.Product))
                            .Distinct()
                            .OrderBy(p => p.ProductId)
                            .ToList();

                        int row = 2;

                        foreach (var product in uniqueProducts)
                        {
                            // Check if the product is part bake, pasty, or cocktail
                            if (ContainsFrozenKeywords(product.ProductName))
                            {
                                worksheet.Cells[row, 1].Value = product.ProductId;
                                worksheet.Cells[row, 2].Value = product.ProductName;

                                col = 3;

                                foreach (var customer in uniqueCustomers)
                                {
                                    var quantity = ordersByDay.SelectMany(o => o.OrderItems)
                                        .Where(oi => oi.Product.ProductId == product.ProductId && oi.Order.Customer == customer)
                                        .Sum(oi => oi.Quantity);

                                    if (quantity != 0)
                                    {
                                        worksheet.Cells[row, col].Value = quantity;
                                    }
                                    else
                                    {
                                        worksheet.Cells[row, col].Value = null;
                                    }

                                    col++;
                                }

                                worksheet.Cells[row, lastCol + 1].Formula = $"SUM(C{row}:{ConvertToLetter(lastCol)}{row})"; // Dynamic SUM formula
                                row++;
                            }
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(0);
                    }
                    AdjustExcelPrint(worksheet);
                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.FitToWidth = 1;
                    //freeze row 1 and column A
                    worksheet.View.FreezePanes(2, 3);
                    package.Save();
                    //MessageBox.Show($"Bread Report for {selectedDay} exported to {outputFilePath}");
                }

                return true; // Method executed successfully
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                // Handle exceptions and return false on failure
                return false;
            }
        }

        // Helper function to check if a product name contains bread-related keywords
        private static bool ContainsFrozenKeywords(string productName)
        {
            string[] keywords = { "saffron", "meat", "pie", "puff", "croissant", "sausage", "part bake lamb & mint", "part bake pork & apple", "tearing" };
            return keywords.Any(keyword => productName.ToLower().Contains(keyword));
        }


        public static bool GenerateBapReport(DayOfWeek selectedDay, string GenSheets)
        {
            try
            {
                string outputFilePath = GenSheets + $@"\MyMatrix_{selectedDay}.xlsx";


                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(outputFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Bap Matrix");

                    var ordersByDay = Data.GetInstance().GetOrders(selectedDay);

                    if (ordersByDay.Count > 0)
                    {
                        // Create a header row
                        worksheet.Cells["A1"].Value = "ID";
                        worksheet.Cells["B1"].Value = "Prod Name";

                        var uniqueCustomers = ordersByDay
                            .SelectMany(o => o.OrderItems
                                .Where(oi => ContainsBapKeywords(oi.Product.ProductName.ToLower()))
                                .Select(oi => oi.Order.Customer))
                            .Distinct()
                            .OrderBy(c => c.CustomerID)
                            .ToList();

                        int col = 3; // Start from column C
                        int lastCol = col + uniqueCustomers.Count - 1;

                        // Set the customer names in the header row at an angle
                        foreach (var customer in uniqueCustomers)
                        {
                            var cell = worksheet.Cells[1, col];
                            cell.Value = customer.CustomerName;
                            cell.Style.TextRotation = 90; // Rotate text clockwise by 90 degrees
                            col++;
                        }

                        worksheet.Cells[1, lastCol + 1].Value = "Quantity"; // Column for Quantity

                        var uniqueProducts = ordersByDay.SelectMany(o => o.OrderItems.Select(oi => oi.Product))
                            .Distinct()
                            .OrderBy(p => p.ProductId)
                            .ToList();

                        int row = 2;

                        foreach (var product in uniqueProducts)
                        {
                            // Check if the product is part bake, pasty, or cocktail
                            if (ContainsBapKeywords(product.ProductName))
                            {
                                worksheet.Cells[row, 1].Value = product.ProductId;
                                worksheet.Cells[row, 2].Value = product.ProductName;

                                col = 3;

                                foreach (var customer in uniqueCustomers)
                                {
                                    var quantity = ordersByDay.SelectMany(o => o.OrderItems)
                                        .Where(oi => oi.Product.ProductId == product.ProductId && oi.Order.Customer == customer)
                                        .Sum(oi => oi.Quantity);

                                    if (quantity != 0)
                                    {
                                        worksheet.Cells[row, col].Value = quantity;
                                    }
                                    else
                                    {
                                        worksheet.Cells[row, col].Value = null;
                                    }

                                    col++;
                                }

                                worksheet.Cells[row, lastCol + 1].Formula = $"SUM(C{row}:{ConvertToLetter(lastCol)}{row})"; // Dynamic SUM formula
                                row++;
                            }
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(0);
                    }
                    AdjustExcelPrint(worksheet);
                    package.Save();
                    //MessageBox.Show($"Bread Report for {selectedDay} exported to {outputFilePath}");
                }

                return true; // Method executed successfully
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                // Handle exceptions and return false on failure
                return false;
            }
        }

        // Helper function to check if a product name contains bread-related keywords
        private static bool ContainsBapKeywords(string productName)
        {
            string[] keywords = { "x4", "bap", "baps", "roll", "rolls", "torpedo" };
            return keywords.Any(keyword => productName.ToLower().Contains(keyword));
        }



        public static bool GenerateCakeReport(DayOfWeek selectedDay, string GenSheets)
        {
            try
            {
                string outputFilePath = GenSheets + $@"\MyMatrix_{selectedDay}.xlsx";

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(outputFilePath)))
                {

                    // Create a new sheet for cakes
                    var worksheet = package.Workbook.Worksheets.Add("Cake Matrix");

                    var ordersByDay = Data.GetInstance().GetOrders(selectedDay);

                    if (ordersByDay.Count > 0)
                    {
                        // Create a header row
                        worksheet.Cells["A1"].Value = "ID";
                        worksheet.Cells["B1"].Value = "Prod Name";

                        var uniqueCustomers = ordersByDay
                            .SelectMany(o => o.OrderItems
                                .Where(oi => oi.Product.ProductId >= 216 && oi.Product.ProductId <= 253)
                                .Select(oi => oi.Order.Customer))
                            .Distinct()
                            .OrderBy(c => c.CustomerID)
                            .ToList();

                        int col = 3; // Start from column C
                        int lastCol = col + uniqueCustomers.Count - 1;

                        // Set the customer names in the header row at an angle
                        foreach (var customer in uniqueCustomers)
                        {
                            var cell = worksheet.Cells[1, col];
                            cell.Value = customer.CustomerName;
                            cell.Style.TextRotation = 90; // Rotate text clockwise by 90 degrees
                            col++;
                        }

                        worksheet.Cells[1, lastCol + 1].Value = "Quantity"; // Column for Quantity

                        var uniqueProducts = ordersByDay.SelectMany(o => o.OrderItems.Select(oi => oi.Product))
                            .Distinct()
                            .Where(p => p.ProductId >= 216 && p.ProductId <= 253)
                            .OrderBy(p => p.ProductId)
                            .ToList();

                        int row = 2;

                        foreach (var product in uniqueProducts)
                        {
                            worksheet.Cells[row, 1].Value = product.ProductId;
                            worksheet.Cells[row, 2].Value = product.ProductName;

                            col = 3;

                            foreach (var customer in uniqueCustomers)
                            {
                                var quantity = ordersByDay.SelectMany(o => o.OrderItems)
                                    .Where(oi => oi.Product.ProductId == product.ProductId && oi.Order.Customer == customer)
                                    .Sum(oi => oi.Quantity);

                                if (quantity != 0)
                                {
                                    worksheet.Cells[row, col].Value = quantity;
                                }
                                else
                                {
                                    worksheet.Cells[row, col].Value = null;
                                }

                                col++;
                            }

                            worksheet.Cells[row, lastCol + 1].Formula = $"SUM(C{row}:{ConvertToLetter(lastCol)}{row})"; // Dynamic SUM formula
                            row++;
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(0);
                    }

                    AdjustExcelPrint(worksheet);
                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.FitToWidth = 1;
                    //freeze row 1 and column A
                    worksheet.View.FreezePanes(2, 3);
                    package.Save();
                    //MessageBox.Show($"Cake Report for {selectedDay} exported to {outputFilePath}");
                }

                return true; // Method executed successfully
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                // Handle exceptions and return false on failure
                return false;
            }
        }

        public static void AdjustExcelPrint(ExcelWorksheet worksheet)
        {
            string cellRange = "A1:AQ200"; // Change this to the range you need

            // Set the font size for the specified cell range
            using (var cells = worksheet.Cells[cellRange])
            {
                cells.Style.Font.Size = (float)(decimal)Settings.Default["ExcelFontSize"];
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(0);

            }

            // Set narrower margins
            worksheet.PrinterSettings.LeftMargin = 0.25m;  
            worksheet.PrinterSettings.RightMargin = 0.25m;
            worksheet.PrinterSettings.TopMargin = 0.25m;
            worksheet.PrinterSettings.BottomMargin = 0.25m;

            // Show gridlines for printing
            worksheet.PrinterSettings.ShowGridLines = true;

            // Change to landscape
            worksheet.PrinterSettings.Orientation = eOrientation.Landscape;


            // Rows to repeat at the top
            worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:1"];

            // Columns to repeat at the left
            worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:B"];


        }
        public static void AdjustExcelPrintPortrait(ExcelWorksheet worksheet)
        {
            string cellRange = "A1:G90"; // Change this to the range you need

            // Set the font size for the specified cell range
            using (var cells = worksheet.Cells[cellRange])
            {
                cells.Style.Font.Size = 22; // Change the font size as needed (in points)
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(0);

            }

            worksheet.PrinterSettings.LeftMargin = 0.25m;
            worksheet.PrinterSettings.RightMargin = 0.25m;
            worksheet.PrinterSettings.TopMargin = 0.25m;
            worksheet.PrinterSettings.BottomMargin = 0.25m;

            worksheet.PrinterSettings.ShowGridLines = true;

            worksheet.PrinterSettings.FitToPage = true;
            worksheet.PrinterSettings.FitToWidth = 1;


        }
        private static void AlternateRowColors(ExcelWorksheet worksheet)
        {
            // Define the two colors you want to alternate between
            var color1 = System.Drawing.Color.LightGray; // Change to your desired color
            var color2 = System.Drawing.Color.White;     // Change to your desired color

            int rowIndex = 2; // Starting row index (adjust as needed)

            for (; rowIndex <= worksheet.Dimension.End.Row; rowIndex++)
            {
                if (rowIndex % 2 == 0)
                {
                    // Set the background color for even rows
                    worksheet.Cells[rowIndex, 2, rowIndex, worksheet.Dimension.End.Column].Style.Fill.PatternType =
                        OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rowIndex, 2, rowIndex, worksheet.Dimension.End.Column].Style.Fill.BackgroundColor.SetColor(color1);
                }
                else
                {
                    // Set the background color for odd rows
                    worksheet.Cells[rowIndex, 2, rowIndex, worksheet.Dimension.End.Column].Style.Fill.PatternType =
                        OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rowIndex, 2, rowIndex, worksheet.Dimension.End.Column].Style.Fill.BackgroundColor.SetColor(color2);
                }

                // Add bottom border
                for (int colIndex = 2; colIndex <= worksheet.Dimension.End.Column; colIndex++)
                {
                    worksheet.Cells[rowIndex, colIndex].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }
            }
        }






    }
}
