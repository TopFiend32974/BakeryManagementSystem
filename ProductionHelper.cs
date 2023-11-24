using Delete_Push_Pull;
using Delete_Push_Pull.Properties;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Delete_Push_Pull.Product;

namespace Delete_Push_Pull
{
    internal class ProductionHelp
    {
        public static bool ProductionHelperMain(DayOfWeek selectedDay, string GenProd)
        {
            List<CombinedProducts> combinedProducts = GetCombinedProducts(selectedDay);
            if (!GenPastyHelper(selectedDay, GenProd))            
                return false;
            //if (!GenProductsTotal(selectedDay, GenProd))
            //    return false;
            if (!GenProductsTotalV2(combinedProducts))
                return false;
            return true;
        }
        public class ProductLists
        {
            public List<float> i24toTray { get; set; }
            public List<float> i15toTray { get; set; }
            public List<float> i30toTray { get; set; }
            public List<float> i5toTray { get; set; }
            public List<float> X4ToTray { get; set; }
            public List<float> X6ToTray { get; set; }
            public List<float> SausageToTray { get; set; }
            public List<float> XaintsToTray { get; set; }
            public List<float> Torpedo110GCuts { get; set; }
            public List<float> SconeCuts { get; set; }
            public List<float> X4StrapsSQ { get; set; }
            public List<float> X5StrapsSM { get; set; }
            public List<float> HighlightList { get; set; }
            public List<float> Stilton { get; set; }
            public List<float> chickenbacon { get; set; }
            public List<float> chickenCurry { get; set; }
            public List<float> CheeseBacon { get; set; }
            public List<float> medCheeseBacon { get; set; }
            public List<float> medVeg { get; set; }

            


        }
        public static bool GenProductsTotal(DayOfWeek selectedDay, string localDir)
        {
            try
            {
                string excelFilePath = Path.Combine(localDir, $"ProductionHelper_{selectedDay}.xlsx");
                List<Order> orders = Data.GetInstance().GetOrders(selectedDay);
               
                Dictionary<int, int> productTotals = new Dictionary<int, int>();
                //var ordersByDay = Data.GetInstance().GetOrders(selectedDay);

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
                //else if (orderItem.Product.ProductId == 244)
                //{
                //    sourceProductQuantities[230] += (orderItem.Quantity);
                //}//add 230 to 244 together on further column away, adjacvent to 230 

                try
                {
                    using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
                    {
                        string jsonFilePath = (string)Settings.Default["Local"] + @"\productTrayData.json";
                        string jsonContent = File.ReadAllText(jsonFilePath);
                        ProductLists productLists = JsonConvert.DeserializeObject<ProductLists>(jsonContent);


                        int halfDivider = 2;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ProductTotals");
                        worksheet.Cells[1, 1].Value = "ID";
                        worksheet.Cells[1, 2].Value = "Product Name";
                        worksheet.Cells[1, 3].Value = "Total";
                        worksheet.Cells[1, 4].Value = "Trays";
                        int row = 2;
                        List<float> i24toTray = productLists.i24toTray;
                        List<float> i15toTray = productLists.i15toTray;
                        List<float> i30toTray = productLists.i30toTray;
                        List<float> i5toTray = productLists.i5toTray;
                        List<float> X4ToTray = productLists.X4ToTray;
                        List<float> X6ToTray = productLists.X6ToTray;
                        List<float> SausageToTray = productLists.SausageToTray;
                        List<float> XaintsToTray = productLists.XaintsToTray;
                        List<float> Torpedo110GCuts = productLists.Torpedo110GCuts;
                        List<float> SconeCuts = productLists.SconeCuts;
                        List<float> X4StrapsSQ = productLists.X4StrapsSQ;
                        List<float> X5StrapsSM = productLists.X5StrapsSM;
                        List<float> HighlightList = productLists.HighlightList;
                        int x4toTrays = 0;
                        int bapsTotal = 0;
                        int FrisbeesTotal = 0;
                        int WhiteBapsTotal = 0;
                        //cocktail sausage rolls
                        //both flavours of bridge rolls


                        foreach (var productTotal in productTotals)
                        {
                            Product product = Data.GetInstance().GetProducts().FirstOrDefault(p => p.ProductId == productTotal.Key);
                                if (HighlightList.Contains(product.ProductId))
                                {
                                    int currentRow = row; // The row you want to highlight

                                    // Set background color for each cell in the row
                                    for (int col = 1; col <= 4; col++)
                                    {
                                        worksheet.Cells[currentRow, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        worksheet.Cells[currentRow, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow); // You can choose any color
                                    }
                                }
                                worksheet.Cells[row, 1].Value = product.ProductId;
                                worksheet.Cells[row, 2].Value = product.ProductName;
                                worksheet.Cells[row, 3].Value = productTotal.Value;

                                if (X4ToTray.Contains(product.ProductId))
                                {
                                    x4toTrays += productTotal.Value * 4;
                                    worksheet.Cells[row, 4].Value = AdjustEquation((productTotal.Value * 4), 24, halfDivider);
                                }
                                if (i24toTray.Contains(product.ProductId))
                                {
                                    if (product.ProductId == 185)
                                    {
                                        WhiteBapsTotal += productTotal.Value;
                                    }
                                    bapsTotal += productTotal.Value;
                                    worksheet.Cells[row, 4].Value = AdjustEquation(productTotal.Value, 24, halfDivider);
                                }
                                else if (i15toTray.Contains(product.ProductId))
                                {
                                    FrisbeesTotal += productTotal.Value;
                                    worksheet.Cells[row, 4].Value = AdjustEquation(productTotal.Value, 15, halfDivider);
                                }
                                else if (i30toTray.Contains(product.ProductId))
                                {
                                    worksheet.Cells[row, 4].Value = AdjustEquation(productTotal.Value, 30, halfDivider);
                                }
                                else if (X6ToTray.Contains(product.ProductId))
                                {
                                    worksheet.Cells[row, 4].Value = AdjustEquation((productTotal.Value * 6), 30, halfDivider);
                                }
                                else if (SausageToTray.Contains(product.ProductId))
                                {
                                    worksheet.Cells[row, 4].Value = AdjustEquation(productTotal.Value, 24, halfDivider);
                                }
                                else if (Torpedo110GCuts.Contains(product.ProductId))
                                {
                                    worksheet.Cells[row, 4].Value = AdjustEquationScones(productTotal.Value, 30, halfDivider);
                                }
                                else if (SconeCuts.Contains(product.ProductId))
                                {
                                    worksheet.Cells[row, 4].Value = AdjustEquationScones(productTotal.Value, 37, halfDivider);
                                }
                                else if (i5toTray.Contains(product.ProductId))
                                {
                                    worksheet.Cells[row, 4].Value = AdjustEquation(productTotal.Value, 5, halfDivider);
                                }
                                else if (XaintsToTray.Contains(product.ProductId))
                                {
                                    worksheet.Cells[row, 4].Value = AdjustEquation(productTotal.Value, 5, halfDivider);
                                }
                                else if (X4StrapsSQ.Contains(product.ProductId))
                                {
                                    worksheet.Cells[row, 4].Value = AdjustEquationBread(productTotal.Value, 4, halfDivider);
                                }
                                else if (X5StrapsSM.Contains(product.ProductId))
                                {
                                    worksheet.Cells[row, 4].Value = AdjustEquationBread(productTotal.Value, 5, halfDivider);
                                }
                                row++;
                            }
                        
                        row += 2;
                        worksheet.Cells[row, 1].Value = "Product Name";
                        worksheet.Cells[row, 2].Value = "Total Product";
                        worksheet.Cells[row, 3].Value = "Aprox Trays";
                        row++;
                        if (bapsTotal + x4toTrays > 0)
                        {
                            worksheet.Cells[row, 1].Value = "Total Baps";
                            worksheet.Cells[row, 2].Value = bapsTotal + x4toTrays;
                            worksheet.Cells[row, 3].Value = Math.Ceiling(((double)bapsTotal + x4toTrays) / 24);
                            row++;
                        }
                        if (FrisbeesTotal > 0)
                        {
                            worksheet.Cells[row, 1].Value = "Total Frisbees";
                            worksheet.Cells[row, 2].Value = FrisbeesTotal;
                            worksheet.Cells[row, 3].Value = Math.Ceiling((double)FrisbeesTotal / 15.0);
                            row++;
                        }
                        if (WhiteBapsTotal + x4toTrays > 0)
                        {
                            worksheet.Cells[row, 1].Value = "Total White Bap";
                            worksheet.Cells[row, 2].Value = WhiteBapsTotal + x4toTrays;
                            worksheet.Cells[row, 3].Value = Math.Ceiling((double)(WhiteBapsTotal + x4toTrays) / 24.0);

                        }
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


        private static string AdjustEquation(int total, int divider, int halfDivider)
        {
            int trays = total / divider;
            int remainder = total % divider;
            int decider = divider / halfDivider;

            // Check if remainder is greater than half of the divider
            if (remainder > decider)
            {
                trays += 1;
                remainder = divider - remainder;
                return $"{trays}T - {remainder}";
            }
            else if (trays == 0)
            {
                return $"{remainder} individuals";
            }
            else if (remainder == 0)
            {
                return $"{trays}T";
            }
            else
            {
                return $"{trays}T + {remainder} individuals";
            }
        }
        private static string AdjustEquationBread(int total, int divider, int halfDivider)
        {
            int trays = total / divider;
            int remainder = total % divider;
            int decider = divider / halfDivider;

            // Check if remainder is greater than half of the divider
            if (remainder > decider)
            {
                trays += 1;
                remainder = divider - remainder;
                return $"{trays} Straps - {remainder} individuals";
            }
            else if (trays == 0)
            {
                return $"{remainder} individuals";
            }
            else if (remainder == 0)
            {
                return $"{trays} Straps";
            }
            else
            {
                return $"{trays} Straps + {remainder} individuals";
            }
        }
        private static string AdjustEquationScones(int total, int divider, int halfDivider)
        {
            int trays = total / divider;
            int remainder = total % divider;
            int decider = divider / halfDivider;

            // Check if remainder is greater than half of the divider
            if (divider != 37)
            {
                if (remainder > decider)
                {
                    trays += 1;
                    remainder = divider - remainder;
                    return $"{trays} Cuts - {remainder} Torpedos (3.3KG)";
                }
                else if (trays == 0)
                {
                    return $"{remainder} individuals (110G)";
                }
                else if (remainder == 0)
                {
                    return $"{trays} Cuts (3.3KG)";
                }
                else
                {
                    return $"{trays} Cuts + {remainder} Torpedos (3.3KG)";
                }
            }
            else
            {
                if (remainder > decider)
                {
                    trays += 1;
                    remainder = divider - remainder;
                    return $"{trays} Cuts - {remainder} Scones P:(3.2KG) F:(3.7KG)";
                }
                else if (trays == 0)
                {
                    return $"{remainder} Scones";
                }
                else if (remainder == 0)
                {
                    return $"{trays} Cuts";
                }
                else
                {
                    return $"{trays} Cuts + {remainder} Scones P:(3.2KG) F:(3.7KG)";
                }
            }
        }



        public static bool GenPastyHelper(DayOfWeek selectedDay, string GenProd)
        {
            try
            {
                string outputFilePath = GenProd + $@"\ProductionHelper_{selectedDay}.xlsx";
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                string jsonFilePath = (string)Settings.Default["Local"] + @"\productTrayData.json";
                string jsonContent = File.ReadAllText(jsonFilePath);
                ProductLists productLists = JsonConvert.DeserializeObject<ProductLists>(jsonContent);

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
                        List<float> Stilton = productLists.Stilton;
                        List<float> chickenbacon = productLists.chickenbacon;
                        List<float> chickenCurry = productLists.chickenCurry;
                        List<float> CheeseBacon = productLists.CheeseBacon;
                        List<float> medCheeseBacon = productLists.medCheeseBacon;
                        List<float> medVeg = productLists.medVeg;
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
                        int totalQuicheCutQuantity = 0;   

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
                                    else if (product.ProductName.ToLower().Contains("quiche")){
                                        traysRequired = totalQuantity;
                                        totalQuicheCutQuantity += totalQuantity;  

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
                                            totalSteakQuantity += totalQuantity;
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
                                    
                                    if (product.ProductName.ToLower().Contains("quiche"))
                                    {
                                        int currentRow = row; // The row you want to highlight

                                        // Set background color for each cell in the row
                                        for (int col = 1; col <= 4; col++)
                                        {
                                            worksheet.Cells[currentRow, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            worksheet.Cells[currentRow, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow); // You can choose any color
                                        }
                                        worksheet.Cells[row, 1].Value = product.ProductId;
                                        worksheet.Cells[row, 2].Value = product.ProductName;
                                        worksheet.Cells[row, 3].Value = totalQuantity;
                                        worksheet.Cells[row, 4].Value = traysRequired;
                                        row++;

                                    }
                                    else
                                    {
                                        worksheet.Cells[row, 1].Value = product.ProductId;
                                        worksheet.Cells[row, 2].Value = product.ProductName;
                                        worksheet.Cells[row, 3].Value = totalQuantity;
                                        worksheet.Cells[row, 4].Value = traysRequired;
                                        row++;
                                    }
                                    
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
                        if(totalQuicheCutQuantity > 0)
                        {
                            worksheet.Cells[row, 2].Value = "Total Quiche:";
                            worksheet.Cells[row, 3].Value = totalQuicheCutQuantity;
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
            string[] keywords = { "pas", "part bake", "cocktail", "quiche" };
            return keywords.Any(keyword => productName.ToLower().Contains(keyword));
        }

        public static List<CombinedProducts> GetCombinedProducts(DayOfWeek selectedDay)
        {
            List<CombinedProducts> combinedProducts = new List<CombinedProducts>();
            List<Order> orders = Data.GetInstance().GetOrders(selectedDay);
            Dictionary<int, int> sourceProductQuantities = new Dictionary<int, int>();
            foreach (Product product in Data.GetInstance().GetProducts())
            {
                if (product.ProductType == ProductTypeEnum.S)
                {
                    sourceProductQuantities.Add(product.ProductId, 0);
                }
                else if (product.ProductType == ProductTypeEnum.BLANK)
                {
                    sourceProductQuantities.Add(product.ProductId, 0);
                }
            }
            int existingKey = 0;
            int existingValue = 0;
            int additionalValue = 0;
            foreach (Order order in orders)
            {
                foreach (OrderItem orderItem in order.OrderItems)
                {
                    try
                    {
                        if (orderItem.Product.ProductType == ProductTypeEnum.S)
                        {
                            sourceProductQuantities[orderItem.Product.ProductId] += orderItem.Quantity;
                        }
                        else if (orderItem.Product.ProductType == ProductTypeEnum.P)
                        {
                            existingKey = orderItem.Product.SourceProductId;
                            existingValue = sourceProductQuantities[existingKey];
                            sourceProductQuantities[existingKey] += (orderItem.Quantity * orderItem.Product.PackSize);
                        }
                        else if (orderItem.Product.ProductType == ProductTypeEnum.BLANK)//&& orderItem.Product.ProductId > 0
                        {

                            sourceProductQuantities[orderItem.Product.ProductId] += orderItem.Quantity;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error:: {ex.Message}");
                    }
                }
            }

            foreach (var kvp in sourceProductQuantities)
            {
                int productId = kvp.Key;
                int quantity = kvp.Value;

                // Fetch the product information from the Data.GetInstance().GetProducts() collection
                Product product = Data.GetInstance().GetProducts().FirstOrDefault(p => p.ProductId == productId);

                // Check if the product is found
                if (product.ProductName != "***")
                {
                    CombinedProducts combinedProduct = new CombinedProducts
                    {
                        ProductId = productId,
                        ProductName = product.ProductName, // Assuming there's a property called ProductName in your Product class
                        ProductQuantity = quantity
                    };

                    combinedProducts.Add(combinedProduct);
                }
            }
            //MessageBox.Show("Done it");
            return combinedProducts;
        }


        public static bool GenProductsTotalV2(List<CombinedProducts> combinedProducts)
        {
            //open new pop up form asking the user if they want to generate this method into file path one, or file path two.
            //if they select file path one, then it will generate the file into the file path one, if they select file path two, then it will generate the file into file path two.
            //if they select cancel, then it will not generate the file.

            string GenDir = (string)Settings.Default["ProductionHelpDir"];
            DayOfWeek selectedDay = MainClass.selectedDayInstance.SelectedDay;
            string excelFilePath = Path.Combine(GenDir, $"ProductionHelper_{selectedDay}.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
                {
                    string jsonFilePath = (string)Settings.Default["Local"] + @"\productTrayData.json";
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    ProductLists productLists = JsonConvert.DeserializeObject<ProductLists>(jsonContent);


                    int halfDivider = 2;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("I am lost");
                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "Product Name";
                    worksheet.Cells[1, 3].Value = "Total";
                    worksheet.Cells[1, 4].Value = "Trays";
                    int row = 2;
                    List<float> i24toTray = productLists.i24toTray;
                    List<float> i15toTray = productLists.i15toTray;
                    List<float> i30toTray = productLists.i30toTray;
                    List<float> i5toTray = productLists.i5toTray;
                    List<float> X4ToTray = productLists.X4ToTray;
                    List<float> X6ToTray = productLists.X6ToTray;
                    List<float> SausageToTray = productLists.SausageToTray;
                    List<float> XaintsToTray = productLists.XaintsToTray;
                    List<float> Torpedo110GCuts = productLists.Torpedo110GCuts;
                    List<float> SconeCuts = productLists.SconeCuts;
                    List<float> X4StrapsSQ = productLists.X4StrapsSQ;
                    List<float> X5StrapsSM = productLists.X5StrapsSM;
                    List<float> HighlightList = productLists.HighlightList;
                    int x4toTrays = 0;
                    int bapsTotal = 0;
                    int FrisbeesTotal = 0;
                    int WhiteBapsTotal = 0;
                    //cocktail sausage rolls
                    //both flavours of bridge rolls

                    Dictionary<int, int> quantitiesToAddTo230 = new Dictionary<int, int>();

                    foreach (var product in combinedProducts)
                    {

                        if (product.ProductQuantity == 0)
                        {
                            continue;
                        }
                        else if (product.ProductQuantity > 0)
                        {


                            //Product product = Data.GetInstance().GetProducts().FirstOrDefault(p => p.ProductId == productTotal.Key);
                            if (HighlightList.Contains(product.ProductId))
                            {
                                int currentRow = row; // The row you want to highlight

                                // Set background color for each cell in the row
                                for (int col = 1; col <= 4; col++)
                                {
                                    worksheet.Cells[currentRow, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[currentRow, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow); // You can choose any color
                                }
                            }
                            worksheet.Cells[row, 1].Value = product.ProductId;
                            worksheet.Cells[row, 2].Value = product.ProductName;
                            worksheet.Cells[row, 3].Value = product.ProductQuantity;

                            if (X4ToTray.Contains(product.ProductId))
                            {
                                x4toTrays += product.ProductQuantity;
                                worksheet.Cells[row, 4].Value = AdjustEquation((product.ProductQuantity * 4), 24, halfDivider);
                            }
                            if (i24toTray.Contains(product.ProductId))
                            {
                                if (product.ProductId == 185)
                                {
                                    WhiteBapsTotal += product.ProductQuantity;
                                }
                                bapsTotal += product.ProductQuantity;
                                worksheet.Cells[row, 4].Value = AdjustEquation(product.ProductQuantity, 24, halfDivider);
                            }
                            else if (i15toTray.Contains(product.ProductId))
                            {
                                FrisbeesTotal += product.ProductQuantity;
                                worksheet.Cells[row, 4].Value = AdjustEquation(product.ProductQuantity, 15, halfDivider);
                            }
                            else if (i30toTray.Contains(product.ProductId))
                            {
                                worksheet.Cells[row, 4].Value = AdjustEquation(product.ProductQuantity, 30, halfDivider);
                            }
                            else if (X6ToTray.Contains(product.ProductId))
                            {
                                worksheet.Cells[row, 4].Value = AdjustEquation((product.ProductQuantity * 6), 30, halfDivider);
                            }
                            else if (SausageToTray.Contains(product.ProductId))
                            {
                                worksheet.Cells[row, 4].Value = AdjustEquation(product.ProductQuantity, 24, halfDivider);
                            }
                            else if (Torpedo110GCuts.Contains(product.ProductId))
                            {
                                worksheet.Cells[row, 4].Value = AdjustEquationScones(product.ProductQuantity, 30, halfDivider);
                            }
                            else if (SconeCuts.Contains(product.ProductId))
                            {
                                worksheet.Cells[row, 4].Value = AdjustEquationScones(product.ProductQuantity, 37, halfDivider);
                            }
                            else if (i5toTray.Contains(product.ProductId))
                            {
                                worksheet.Cells[row, 4].Value = AdjustEquation(product.ProductQuantity, 5, halfDivider);
                            }
                            else if (XaintsToTray.Contains(product.ProductId))
                            {
                                worksheet.Cells[row, 4].Value = AdjustEquation(product.ProductQuantity, 5, halfDivider);
                            }
                            else if (X4StrapsSQ.Contains(product.ProductId))
                            {
                                worksheet.Cells[row, 4].Value = AdjustEquationBread(product.ProductQuantity, 4, halfDivider);
                            }
                            else if (X5StrapsSM.Contains(product.ProductId))
                            {
                                worksheet.Cells[row, 4].Value = AdjustEquationBread(product.ProductQuantity, 5, halfDivider);
                            }

                            if (product.ProductId == 230)
                            {
                                int index244 = combinedProducts.FindIndex(p => p.ProductId == 244);
                                if (index244 != -1 && combinedProducts[index244].ProductQuantity > 0)
                                {
                                    int totalQuantity = product.ProductQuantity + combinedProducts[index244].ProductQuantity;
                                    worksheet.Cells[row, 4].Value = totalQuantity + " Round Doughnuts";
                                }
                            }

                            row++;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    row += 2;
                    worksheet.Cells[row, 1].Value = "Product Name";
                    worksheet.Cells[row, 2].Value = "Total Product";
                    worksheet.Cells[row, 3].Value = "Aprox Trays";
                    row++;
                    if (bapsTotal + x4toTrays > 0)
                    {
                        worksheet.Cells[row, 1].Value = "Total Baps";
                        worksheet.Cells[row, 2].Value = bapsTotal + x4toTrays;
                        worksheet.Cells[row, 3].Value = Math.Ceiling(((double)bapsTotal + x4toTrays) / 24);
                        row++;
                    }
                    if (FrisbeesTotal > 0)
                    {
                        worksheet.Cells[row, 1].Value = "Total Frisbees";
                        worksheet.Cells[row, 2].Value = FrisbeesTotal;
                        worksheet.Cells[row, 3].Value = Math.Ceiling((double)FrisbeesTotal / 15.0);
                        row++;
                    }
                    if (WhiteBapsTotal + x4toTrays > 0)
                    {
                        worksheet.Cells[row, 1].Value = "Total White Bap";
                        worksheet.Cells[row, 2].Value = WhiteBapsTotal + x4toTrays;
                        worksheet.Cells[row, 3].Value = Math.Ceiling((double)(WhiteBapsTotal + x4toTrays) / 24.0);

                    }
                    ExcelConversions.AdjustExcelPrintPortrait(worksheet);
                    package.SaveAs(new FileInfo(excelFilePath));
                    //MessageBox.Show($"WEHRE IS IT? ");
                }
                return true;
            }
            catch (Exception ex)
            {
                
                
                MessageBox.Show($"Error processing data: {ex.Message}");
                return false;
            }
        }
    }

    public class CombinedProducts
    {
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductName { get; set; }
    }
   
}
    
    