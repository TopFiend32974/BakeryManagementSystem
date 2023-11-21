using Delete_Push_Pull.Properties;
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
            if (!GenProductsTotal(selectedDay, GenProd))
                return false;

            return true;
        }

        public static bool GenProductsTotal(DayOfWeek selectedDay, string localDir)
        {
            try
            {
                string excelFilePath = Path.Combine(localDir, $"ProductionHelper_{selectedDay}.xlsx");
                List<Order> orders = Data.GetInstance().GetOrders(selectedDay);
                Dictionary<int, int> productTotals = new Dictionary<int, int>();
                foreach (Product product in Data.GetInstance().GetProducts())
                {
                    productTotals.Add(product.ProductId, 0);
                }
                foreach (Order order in orders)
                {
                    foreach (OrderItem orderItem in order.OrderItems)
                    {
                        int productId = orderItem.Product.ProductId;
                        productTotals[productId] += orderItem.Quantity;
                    }
                }
                try
                {
                    using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ProductTotals");
                        worksheet.Cells[1, 1].Value = "ID";
                        worksheet.Cells[1, 2].Value = "Product Name";
                        worksheet.Cells[1, 3].Value = "Total";
                        worksheet.Cells[1, 4].Value = "Trays";
                        int row = 2;
                        List<float> i24toTray = new List<float> { 185, 172 };
                        List<float> i15toTray = new List<float> { 168, 179, 199 };
                        List<float> i30toTray = new List<float> { 187 };
                        List<float> i5toTray = new List<float> { 215 };
                        List<float> X4ToTray = new List<float> { 180, 183, 184 };
                        List<float> X6ToTray = new List<float> { 214 };
                        List<float> SausageToTray = new List<float> { 380 };
                        List<float> XaintsToTray = new List<float> { 311 };
                        List<float> Torpedo110GCuts = new List<float> { 142, 198 };
                        List<float> SconeCuts = new List<float> { 260, 265 };
                        List<float> X4StrapsSQ = new List<float> { 5, 73, 113, 134 };
                        List<float> X5StrapsSM = new List<float> { 12, 86, 118, 138 };
                        int x4toTrays = 0;
                        int bapsTotal = 0;
                        int FrisbeesTotal = 0;
                        foreach (var productTotal in productTotals)
                        {                           
                            Product product = Data.GetInstance().GetProducts().FirstOrDefault(p => p.ProductId == productTotal.Key);                            
                            if (productTotal.Value > 0)
                            {
                                worksheet.Cells[row, 1].Value = product.ProductId;
                                worksheet.Cells[row, 2].Value = product.ProductName;
                                worksheet.Cells[row, 3].Value = productTotal.Value;

                                if (X4ToTray.Contains(product.ProductId))
                                {
                                    x4toTrays += productTotal.Value * 4;
                                    int trays = (productTotal.Value * 4) / 24;
                                    int remainder = (productTotal.Value * 4) % 24;
                                    worksheet.Cells[row, 4].Value = @$"{trays}T + {remainder} Baps";
                                }
                                if (i24toTray.Contains(product.ProductId))
                                {                                   
                                    bapsTotal += productTotal.Value;
                                    int Total = productTotal.Value;
                                    int trays =  Total / 24;
                                    int remainder = Total % 24;
                                    worksheet.Cells[row, 4].Value = @$"{trays}T + {remainder} Baps";
                                }                                
                                else if(i15toTray.Contains(product.ProductId)){
                                    FrisbeesTotal += productTotal.Value;
                                    int trays = productTotal.Value / 15;
                                    int remainder = productTotal.Value % 15;
                                    worksheet.Cells[row, 4].Value = @$"{trays}T + {remainder} Frisbees";
                                }
                                else if (i30toTray.Contains(product.ProductId))
                                {
                                    int trays = productTotal.Value / 30;
                                    int remainder = productTotal.Value % 30;
                                    worksheet.Cells[row, 4].Value = @$"{trays}T + {remainder} Splits";
                                }
                                else if (X6ToTray.Contains(product.ProductId))
                                {
                                    int trays = (productTotal.Value * 6) / 30;
                                    int remainder = (productTotal.Value * 6) % 30;
                                    worksheet.Cells[row, 4].Value = @$"{trays}T + {remainder} Finger Rolls";
                                }
                                else if (SausageToTray.Contains(product.ProductId))
                                {
                                    int trays = productTotal.Value / 24;
                                    int remainder = productTotal.Value % 24;
                                    worksheet.Cells[row, 4].Value = @$"{trays}T + {remainder} Sauages";
                                }
                                else if (Torpedo110GCuts.Contains(product.ProductId))
                                {
                                    int Cuts = productTotal.Value / 30;
                                    int remainder = productTotal.Value % 30;
                                    worksheet.Cells[row, 4].Value = @$"{Cuts}Cuts + {remainder} Torpedos (3.3KG)";
                                }
                                else if (SconeCuts.Contains(product.ProductId))
                                {
                                    int Cuts = productTotal.Value / 37;
                                    int remainder = productTotal.Value % 37;
                                    worksheet.Cells[row, 4].Value = @$"{Cuts}Cuts + {remainder} Scones P:(3.2KG) F:(3.7KG)";
                                }
                                else if (i5toTray.Contains(product.ProductId))
                                {
                                    int trays = productTotal.Value / 5;
                                    int remainder = productTotal.Value % 5;
                                    worksheet.Cells[row, 4].Value = @$"{trays}T + {remainder} Sticks";
                                }
                                else if (XaintsToTray.Contains(product.ProductId))
                                {
                                    int trays = productTotal.Value / 5;
                                    int remainder = (productTotal.Value * 4) % 5;
                                    worksheet.Cells[row, 4].Value = @$"{trays}T + {remainder} Crossiants";
                                }else if (X4StrapsSQ.Contains(product.ProductId))
                                {
                                    int trays = productTotal.Value / 4;
                                    int remainder = productTotal.Value % 4;
                                    worksheet.Cells[row, 4].Value = @$"{trays}Straps + {remainder} SQ";
                                }else if (X5StrapsSM.Contains(product.ProductId))
                                {
                                    int trays = productTotal.Value / 5;
                                    int remainder = productTotal.Value % 5;
                                    worksheet.Cells[row, 4].Value = @$"{trays}Staps + {remainder} SM";
                                }
                                row++;
                            }
                        }
                        row += 2;
                        worksheet.Cells[row, 1].Value = "Product Name";
                        worksheet.Cells[row, 2].Value = "Total Product";
                        worksheet.Cells[row, 3].Value = "Total Trays";
                        row++;
                        worksheet.Cells[row, 1].Value = "Total Baps";
                        worksheet.Cells[row, 2].Value = bapsTotal + x4toTrays;
                        worksheet.Cells[row, 3].Value = (bapsTotal + x4toTrays) / 24;
                        row++;
                        worksheet.Cells[row, 1].Value = "Total Frisbees";
                        worksheet.Cells[row, 2].Value = FrisbeesTotal;
                        worksheet.Cells[row, 3].Value = FrisbeesTotal / 15;
                        ExcelConversions.AdjustExcelPrintPortrait(worksheet);
                        package.SaveAs(new FileInfo(excelFilePath));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing data: {ex.Message}");
                }

                return true;
            }
            catch
            {
                return false;
            }
           
        }


        public static bool GenPastyHelper(DayOfWeek selectedDay, string GenProd)
        {
            try
            {
                string outputFilePath = GenProd + $@"\ProductionHelper_{selectedDay}.xlsx";
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {                    
                    var worksheet = package.Workbook.Worksheets.Add("PastyHelper");
                    var ordersByDay = Data.GetInstance().GetOrders(selectedDay);
                    if (ordersByDay.Count > 0)
                    {
                        worksheet.Cells["A1"].Value = "ID";
                        worksheet.Cells["B1"].Value = "Product Name";
                        worksheet.Cells["C1"].Value = "Qty";
                        worksheet.Cells["D1"].Value = "Trays";
                        var uniqueProducts = ordersByDay.SelectMany(o => o.OrderItems.Select(oi => oi.Product))
                            .Distinct()
                            .OrderBy(p => p.ProductId)
                            .ToList();

                        int row = 2;

                        List<float> Stilton = new List<float> { 373, 338};
                        List<float> chickenbacon = new List<float> { 336, 369};
                        List<float> chickenCurry = new List<float> { 339, 366};
                        List<float> CheeseBacon = new List<float> { 335, 372};
                        List<float> medCheeseBacon = new List<float> { 388, 372};
                        List<float> medVeg = new List<float> { 400 };
                        int totalMedPastyQuantity = 0;
                        int totalCocktailPastyQuantity = 0;
                        int totalFarmersQuantity = 0;
                        int totalOtherQuantity = 0;
                        int totalChickenQuantity = 0;  
                        int totalCheeseQuantity = 0;   
                        int totalVeganQuantity = 0;   
                        int totalSteakQuantity = 0;   
                        int totalMedCheeseQuantity = 0;   
                        int totalMedSteakQuantity = 0;   
                        int totalStilton = 0;
                        int totalChickenBacon = 0;
                        int totalChickenCurry = 0;
                        int totalCheeseBacon = 0;
                        int totalMedCheeseBacon = 0;
                        int totalMedVeg = 0;
                        int totalLargeCutQuantity = 0;   
                        int totalMedCutQuantity = 0;   
                        int totalCocktailCutQuantity = 0;   

                        foreach (var product in uniqueProducts)
                        {                       
                            if (PastyKeywords(product.ProductName))
                            {
                                int totalQuantity = ordersByDay.SelectMany(o => o.OrderItems)
                                    .Where(oi => oi.Product.ProductId == product.ProductId)
                                    .Sum(oi => oi.Quantity);
                                if (totalQuantity > 0)
                                {
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
                                            if (medCheeseBacon.Contains(product.ProductId))
                                            {
                                                totalMedCheeseBacon += totalQuantity;
                                            }                                            
                                            totalMedCheeseQuantity += totalQuantity;
                                        }
                                        else if (product.ProductName.ToLower().Contains("steak"))
                                        {
                                            totalMedSteakQuantity += totalQuantity;
                                        }
                                        else if (medVeg.Contains(product.ProductId))
                                        {
                                            totalMedVeg += totalQuantity;
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
                                            if (product.ProductName.ToLower().Contains("chick"))
                                            {
                                                if (chickenbacon.Contains(product.ProductId))
                                                {
                                                    totalChickenBacon += totalQuantity;
                                                }
                                                else if (chickenCurry.Contains(product.ProductId))
                                                {
                                                    totalChickenCurry += totalQuantity;
                                                }
                                                totalChickenQuantity += totalQuantity;
                                            }                                            
                                            else if (product.ProductName.ToLower().Contains("steak"))
                                            {
                                                if (Stilton.Contains(product.ProductId))
                                                {
                                                    totalStilton += totalQuantity;
                                                }
                                                totalSteakQuantity += totalQuantity;
                                            }                                            
                                            else if (product.ProductName.ToLower().Contains("veg"))
                                            {
                                                totalVeganQuantity += totalQuantity;
                                            }
                                            else if (product.ProductName.ToLower().Contains("cheese"))
                                            {                                                
                                                if (CheeseBacon.Contains(product.ProductId))
                                                {
                                                    totalCheeseBacon += totalQuantity;
                                                }
                                                totalCheeseQuantity += totalQuantity;
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
                        int startDataRow = 2;
                        int endDataRow = row - 1;
                        string totalFormula = $"SUM(D{startDataRow}:D{endDataRow})";                        
                        worksheet.Cells[row, 3].Value = "Trays Total:";
                        worksheet.Cells[row, 4].Formula = totalFormula;
                        worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00"; 
                        row++;
                        row++;
                        worksheet.Cells[row, 2].Value = "Base Total";
                        worksheet.Cells[row, 3].Value = "QTY"; 
                        worksheet.Cells[row, 4].Value = "Subcat QTY";
                        row++;
                        if (totalSteakQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Steaked Up Total";
                            worksheet.Cells[row, 3].Value = totalSteakQuantity;
                            row++;
                            worksheet.Cells[row, 3].Value = "Stilton:";
                            worksheet.Cells[row, 4].Value = totalStilton;
                            row++;
                        }                         
                        if (totalChickenQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Chickened Up Total";
                            worksheet.Cells[row, 3].Value = totalChickenQuantity;
                            row++;
                            worksheet.Cells[row, 3].Value = "ChickBacon:";
                            worksheet.Cells[row, 4].Value = totalChickenBacon;
                            row++;
                            worksheet.Cells[row, 3].Value = "ChickCurry:";
                            worksheet.Cells[row, 4].Value = totalChickenCurry;
                            row++;

                        }
                        if (totalCheeseQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Cheesed Up Total";
                            worksheet.Cells[row, 3].Value = totalCheeseQuantity;
                            row++;
                            worksheet.Cells[row, 3].Value = "CheeseBacon:";
                            worksheet.Cells[row, 4].Value = totalCheeseBacon;
                            row++;
                        }                        
                        if (totalVeganQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Veganed Up Total";
                            worksheet.Cells[row, 3].Value = totalVeganQuantity;
                            row++;
                        }
                        if (totalMedCheeseQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Med Cheesed Up Total";
                            worksheet.Cells[row, 3].Value = totalMedCheeseQuantity;
                            row++;
                        }
                        if (totalMedCheeseBacon > 0)
                        {
                            worksheet.Cells[row, 3].Value = "MedCheeseBacon:";
                            worksheet.Cells[row, 4].Value = totalMedCheeseBacon;
                            row++;
                        }
                        if (totalMedVeg > 0)
                        {
                            worksheet.Cells[row, 2].Value = "MedVeg:";
                            worksheet.Cells[row, 3].Value = totalMedVeg;
                            row++;
                        }
                        if 
                        (totalMedSteakQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Med Steak Up Total";
                            worksheet.Cells[row, 3].Value = totalMedSteakQuantity;
                            row++;

                        }
                        worksheet.Cells[row, 2].Value = "Base Size";
                        worksheet.Cells[row, 3].Value = "Base Total";
                        worksheet.Cells[row, 4].Value = "Cuts Needed";
                        row++;
                        if (totalMedPastyQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Total Med Pasties";
                            worksheet.Cells[row, 3].Value = totalMedPastyQuantity;
                            string totalMedCutResult = productionHelper.CutRounder(totalMedCutQuantity);
                            worksheet.Cells[row, 4].Value = totalMedCutResult;
                            row++;
                        }
                        if(totalCocktailPastyQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Total Cocktail Pasties";
                            worksheet.Cells[row, 3].Value = totalCocktailPastyQuantity;
                            string totalCocktailCutResult = productionHelper.CutRounder(totalCocktailCutQuantity);
                            worksheet.Cells[row, 4].Value = totalCocktailCutResult;
                            row++;
                        }
                        if(totalFarmersQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Total Farmers";
                            worksheet.Cells[row, 3].Value = totalFarmersQuantity;
                            row++;
                        }
                        if(totalOtherQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Total Large";
                            worksheet.Cells[row, 3].Value = totalOtherQuantity;
                            string totalLargeCutResult = productionHelper.CutRounder(totalLargeCutQuantity);
                            worksheet.Cells[row, 4].Value = totalLargeCutResult;
                        }
                        
                    }

                    ExcelConversions.AdjustExcelPrintPortrait(worksheet);
                    package.SaveAs(new FileInfo(outputFilePath));
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string CutRounder(int totalQuantity)
        {
            int BallsREMOVE = 0;
            int cutSize = 30;
            int numberOfCuts = totalQuantity / cutSize; 
            int remainingBalls = totalQuantity % cutSize; 
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
