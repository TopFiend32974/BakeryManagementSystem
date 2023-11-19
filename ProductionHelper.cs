using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Delete_Push_Pull
{
    internal class ProductionHelp
    {
        public static bool ProductionHelperMain(DayOfWeek selectedDay, string GenProd)
        {
            if (!GenPastyHelper(selectedDay, GenProd))            
                return false;
            //if (!GenTrayHelper(selectedDay, GenSheets))
            //    return false;
                        
             return true;
        }
       
        public static bool GenPastyHelper(DayOfWeek selectedDay, string GenProd)
        {
            try
            {
                // Specify the output Excel file path for the Pasty Helper report
                string outputFilePath = GenProd + $@"\ProductionHelper_{selectedDay}.xlsx";

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    
                    var worksheet = package.Workbook.Worksheets.Add("PastyHelper");

                    var ordersByDay = Data.GetInstance().GetOrders(selectedDay);

                    if (ordersByDay.Count > 0)
                    {
                        // Create a header row
                        worksheet.Cells["A1"].Value = "Product ID";
                        worksheet.Cells["B1"].Value = "Product Name";
                        worksheet.Cells["C1"].Value = "Quantity";
                        worksheet.Cells["D1"].Value = "Trays Required";

                        var uniqueProducts = ordersByDay.SelectMany(o => o.OrderItems.Select(oi => oi.Product))
                            .Distinct()
                            .OrderBy(p => p.ProductId)
                            .ToList();

                        int row = 2;

                        int totalMedPastyQuantity = 0;
                        int totalCocktailPastyQuantity = 0;
                        int totalFarmersQuantity = 0;
                        int totalOtherQuantity = 0;
                        int totalChickenQuantity = 0;  // New variable for chicken total
                        int totalCheeseQuantity = 0;   // New variable for cheese total
                        int totalVeganQuantity = 0;   // New variable for cheese total
                        int totalSteakQuantity = 0;   // New variable for cheese total
                        int totalMedCheeseQuantity = 0;   // New variable for cheese total
                        int totalMedSteakQuantity = 0;   // New variable for cheese total

                        int totalLargeCutQuantity = 0;   // New variable for cheese total
                        int totalMedCutQuantity = 0;   // New variable for cheese total
                        int totalCocktailCutQuantity = 0;   // New variable for cheese total

                        foreach (var product in uniqueProducts)
                        {
                            // Check if the product is part bake, pasty, or cocktail
                            if (PastyKeywords(product.ProductName))
                            {
                                int totalQuantity = ordersByDay.SelectMany(o => o.OrderItems)
                                    .Where(oi => oi.Product.ProductId == product.ProductId)
                                    .Sum(oi => oi.Quantity);
                                if (totalQuantity > 0)
                                {
                                    // Calculate trays required based on the rules with decimals
                                    double traysRequired = 0;
                                    if (product.ProductName.ToLower().Contains("cocktail"))
                                    {
                                        traysRequired = totalQuantity / 30.0;
                                        totalCocktailPastyQuantity += totalQuantity;
                                        totalCocktailCutQuantity += totalQuantity;
                                    }
                                    else if (product.ProductName.ToLower().Contains("med"))
                                    {
                                        if (product.ProductName.ToLower().Contains("cheese"))
                                        {
                                            totalMedCheeseQuantity += totalQuantity;
                                        }
                                        else if (product.ProductName.ToLower().Contains("steak"))
                                        {
                                            totalMedSteakQuantity += totalQuantity;
                                        }
                                        traysRequired = totalQuantity / 20.0;
                                        totalMedPastyQuantity += totalQuantity;
                                        totalMedCutQuantity += totalQuantity;
                                    }
                                    else
                                    {
                                        traysRequired = totalQuantity / 16.0;

                                        if (product.ProductName.ToLower().Contains("farmer"))
                                        {
                                            totalFarmersQuantity += totalQuantity;
                                        }
                                        else
                                        {
                                            // Check for "chick" or "chicken"
                                            if (product.ProductName.ToLower().Contains("chick") ||
                                                product.ProductName.ToLower().Contains("chicken"))
                                            {
                                                totalChickenQuantity += totalQuantity;
                                            }
                                            // Check for "cheese"
                                            else if (product.ProductName.ToLower().Contains("cheese"))
                                            {
                                                totalCheeseQuantity += totalQuantity;
                                            }
                                            else if (product.ProductName.ToLower().Contains("steak"))
                                            {
                                                totalSteakQuantity += totalQuantity;
                                            }
                                            else if (product.ProductName.ToLower().Contains("vegan") ||
                                                product.ProductName.ToLower().Contains("veg"))
                                            {
                                                totalVeganQuantity += totalQuantity;
                                            }

                                            totalOtherQuantity += totalQuantity;
                                            totalLargeCutQuantity += totalQuantity;
                                        }
                                    }



                                    worksheet.Cells[row, 1].Value = product.ProductId;
                                    worksheet.Cells[row, 2].Value = product.ProductName;
                                    worksheet.Cells[row, 3].Value = totalQuantity;
                                    worksheet.Cells[row, 4].Value = traysRequired;
                                    row++;
                                }
                            }
                        }


                        ProductionHelp productionHelper = new ProductionHelp();

                        // Your existing code to add data rows

                        int startDataRow = 2; // The first row where data starts
                        int endDataRow = row - 1; // The last row with data (excluding totals)
                        string totalFormula = $"SUM(D{startDataRow}:D{endDataRow})"; // Excel formula to sum the values

                        // Insert the total row
                        row++;
                        worksheet.Cells[row, 4].Value = "Trays Total:";
                        row++;
                        worksheet.Cells[row, 4].Formula = totalFormula;
                        worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00"; // Formatting if needed
                        row++;
                        worksheet.Cells[row, 2].Value = "Steaked Up Total";
                        worksheet.Cells[row, 3].Value = totalSteakQuantity;
                        row++;
                        worksheet.Cells[row, 2].Value = "Chickened Up Total";
                        worksheet.Cells[row, 3].Value = totalChickenQuantity;  // Display the total for "chick" and "chicken"
                        row++;
                        worksheet.Cells[row, 2].Value = "Cheesed Up Total";
                        worksheet.Cells[row, 3].Value = totalCheeseQuantity;
                        row++;
                        worksheet.Cells[row, 2].Value = "Veganed Up Total";
                        worksheet.Cells[row, 3].Value = totalVeganQuantity;
                        row++;
                        worksheet.Cells[row, 2].Value = "Med Cheesed Up Total";
                        worksheet.Cells[row, 3].Value = totalMedCheeseQuantity;
                        row++;
                        worksheet.Cells[row, 2].Value = "Med Steak Up Total";
                        worksheet.Cells[row, 3].Value = totalMedSteakQuantity;
                        row++;
                        row++;
                        worksheet.InsertRow(row, 6);
                        worksheet.Cells[row, 4].Value = "Cuts Needed";
                        row++;
                        worksheet.Cells[row, 2].Value = "Total Med Pasties";
                        worksheet.Cells[row, 3].Value = totalMedPastyQuantity;
                        string totalMedCutResult = productionHelper.CutRounder(totalMedCutQuantity);
                        worksheet.Cells[row, 4].Value = totalMedCutResult;
                        row++;
                        worksheet.Cells[row, 2].Value = "Total Cocktail Pasties";
                        worksheet.Cells[row, 3].Value = totalCocktailPastyQuantity;
                        string totalCocktailCutResult = productionHelper.CutRounder(totalCocktailCutQuantity);
                        worksheet.Cells[row, 4].Value = totalCocktailCutResult;
                        row++;
                        worksheet.Cells[row, 2].Value = "Total Farmers";
                        worksheet.Cells[row, 3].Value = totalFarmersQuantity;
                        row++;
                        worksheet.Cells[row, 2].Value = "Total Large";
                        worksheet.Cells[row, 3].Value = totalOtherQuantity;
                        string totalLargeCutResult = productionHelper.CutRounder(totalLargeCutQuantity);
                        worksheet.Cells[row, 4].Value = totalLargeCutResult;
                        row++;                       

                        //fontsize 22
                        //enable gridlines 
                        //enable header lines
                    }


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





                    package.SaveAs(new FileInfo(outputFilePath));
                }

                return true; // Method executed successfully
            }
            catch (Exception ex)
            {
                // Handle exceptions and return false on failure
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string CutRounder(int totalQuantity)
        {
            int BallsREMOVE = 0;
            int cutSize = 30; // 1 cut = 30 balls
            int numberOfCuts = totalQuantity / cutSize; // Calculate the number of cuts
            int remainingBalls = totalQuantity % cutSize; // Calculate the remaining balls
            if (numberOfCuts == 0)
            {
                return $"{remainingBalls} balls";
            }
            else if (numberOfCuts == 1)
            {
                if (Math.Abs(remainingBalls) == 0)
                {
                    return $"{numberOfCuts} cut";
                }
                else if (Math.Abs(remainingBalls) < 15 && Math.Abs(remainingBalls) > 0)
                {
                    if ((numberOfCuts * cutSize) + remainingBalls == totalQuantity)
                    {
                        return $"{numberOfCuts} cut + {remainingBalls} balls";
                    }
                    else
                    {
                        return "Line 990 went wrong sorry pal";
                    }
                }
                else
                {
                    BallsREMOVE = remainingBalls - cutSize;
                    numberOfCuts++;
                    if ((numberOfCuts * cutSize) + BallsREMOVE == totalQuantity)
                    {
                        return $"{numberOfCuts} cut {BallsREMOVE} balls";
                    }
                    else
                    {
                        return $"{numberOfCuts} cut - {BallsREMOVE} balls - but it all went wrong";
                    }
                }
            }
            else
            {
                if (Math.Abs(remainingBalls) == 0)
                {
                    return $"{numberOfCuts} cuts";
                }
                else if (Math.Abs(remainingBalls) < 15 && Math.Abs(remainingBalls) > 0)
                {
                    if ((numberOfCuts * cutSize) + remainingBalls == totalQuantity)
                    {
                        return $"{numberOfCuts} cuts + {remainingBalls} balls";
                    }
                    else
                    {
                        return "Line 990 went wrong sorry pal";
                    }
                }
                else
                {
                    BallsREMOVE = remainingBalls - cutSize;
                    numberOfCuts++;
                    if ((numberOfCuts * cutSize) + BallsREMOVE == totalQuantity)
                    {
                        return $"{numberOfCuts} cuts {BallsREMOVE} balls";
                    }
                    else
                    {
                        return $"{numberOfCuts} cuts - {BallsREMOVE} balls - but it all went wrong";
                    }
                }
            }
        }




        private static bool PastyKeywords(string productName)
        {
            string[] keywords = { "pas", "part bake", "cocktail" };
            return keywords.Any(keyword => productName.ToLower().Contains(keyword));
        }


    }
}
