using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Windows.Forms;
using System.IO.Packaging;
using Delete_Push_Pull.Properties;
using System.Drawing.Drawing2D;

namespace Delete_Push_Pull
{
    public class DataLoader
    {
        public DataLoader(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }

        public bool LoadAllData()
        {
            string LocalDir = (string)Settings.Default["Local"];
            if (!LoadProducts(LocalDir) || !LoadCustomers(LocalDir) || !LoadOrders(LocalDir))
                return false;

            return true;
        }


        public bool LoadProducts(string directory)
        {
            
            var filePath = directory + @"\PRODUCTS.DAT";

            if (File.Exists(filePath))
            {
                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (var reader = new BinaryReader(fs))
                    {
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            byte[] b = reader.ReadBytes(255);
                            string productRawString = Encoding.GetEncoding("iso-8859-1").GetString(b);
                            Product p = Product.ParseProduct(productRawString);
                            if (p != null)
                            {
                                Data.GetInstance().AddProduct(p);
                            }
                            else
                            {
                                MessageBox.Show("Unable to parse product");
                            }
                        }
                    }

                    //MessageBox.Show(Data.GetInstance().GetProducts().Count + " products loaded");
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    return false;
                }
            }
            else
            {
                ErrorMessage = "PRODUCTS.DAT file not found";
                return false;
            }
        }


        public bool LoadCustomers(string directory)
        {
            string filePath = directory + @"\CUSTOMER.DAT";

            if (File.Exists(filePath))
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        byte[] b = new byte[255];

                        while (reader.Read(b, 0, b.Length) != 0)
                        {
                            string customerRawString = Encoding.GetEncoding("iso-8859-1").GetString(b);
                            Customer c = Customer.ParseCustomer(customerRawString);
                            if (c != null)
                            {
                                Data.GetInstance().AddCustomer(c);
                            }
                            else
                            {
                                MessageBox.Show("Unable to parse customer");
                            }
                        }
                    }

                    //MessageBox.Show(Data.GetInstance().GetCustomers().Count + " customers loaded");
                    

                    return true;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    return false;
                }
            }
            else
            {
                ErrorMessage = "CUSTOMER.DAT file not found";
                return false;
            }
        }


        //-------------------------------------------------//

        public bool LoadOrders(string directory)
        {
            foreach (Customer c in Data.GetInstance().GetCustomers())
            {
                if (!c.CustomerName.Replace(" ", "").Equals("***"))
                {
                    string orderFilePath = Path.Combine(directory, "ODR" + c.CustomerID + ".ODR");

                    if (File.Exists(orderFilePath))
                    {
                        try
                        {
                            using (StreamReader reader = new StreamReader(orderFilePath))
                            {
                                for (DayOfWeek day = DayOfWeek.Sunday; day <= DayOfWeek.Saturday; day++)
                                {
                                    Order o = new Order(c, day);

                                    string numItemsString = reader.ReadLine();

                                    if (!string.IsNullOrWhiteSpace(numItemsString))
                                    {
                                        int numItems = int.Parse(numItemsString.Trim());

                                        while (numItems > 0)
                                        {
                                            string productIdString = reader.ReadLine().Trim();
                                            string quantityString = reader.ReadLine().Trim();

                                            int productId = int.Parse(productIdString);
                                            int quantity = int.Parse(quantityString);

                                            Product product = Data.GetInstance().GetProducts().FirstOrDefault(p => p.ProductId == productId);

                                            if (product != null)
                                            {
                                                OrderItem oi = new OrderItem(product, quantity);
                                                o.AddOrderItem(oi);
                                            }

                                            numItems--;
                                        }

                                        Data.GetInstance().AddOrder(o);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = ex.Message;
                            Console.Error.WriteLine(ex);
                            return false;
                        }
                    }
                }
            }

            //MessageBox.Show("Orders happened");
            return true;
        }


      
    }


    //------------------------------//
    

    class Product
    {
        public enum ProductClassEnum
        {
            B, C, R, O, I, M, BLANK
        }

        public enum MarkupClassEnum
        {
            A, BLANK
        }

        public enum ProductTypeEnum
        {
            S, P, BLANK
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public ProductClassEnum ProductClass { get; set; }
        public MarkupClassEnum MarkupClass { get; set; }
        public int BatchSize { get; set; }

        public ProductTypeEnum ProductType { get; set; }
        public int PackSize { get; set; }
        public int SourceProductId { get; set; }

        private Product()
        {

        }

        public static Product ParseProduct(string input)
        {
            if (input.Length != 255)
            {
                MessageBox.Show("Product string not the correct size. Expected 255, got: " + input.Length);
                return null;
            }

            var p = new Product();

            try
            {
                p.ProductId = int.Parse(input.Substring(0, 4).Replace(" ", ""));
                p.ProductName = input.Substring(4, 25).Trim();

                if (p.ProductName.Replace(" ", "") == "***")
                {
                    p.BatchSize = 0;
                    p.MarkupClass = MarkupClassEnum.BLANK;
                    p.ProductClass = ProductClassEnum.BLANK;
                    p.ProductType = ProductTypeEnum.BLANK;
                    return p;
                }

                var productClassString = input[29].ToString();
                p.ProductClass = string.IsNullOrWhiteSpace(productClassString)
                    ? ProductClassEnum.BLANK
                    : (ProductClassEnum)Enum.Parse(typeof(ProductClassEnum), productClassString);

                var markupClassString = input[31].ToString();
                p.MarkupClass = string.IsNullOrWhiteSpace(markupClassString)
                    ? MarkupClassEnum.BLANK
                    : (MarkupClassEnum)Enum.Parse(typeof(MarkupClassEnum), markupClassString);

                p.BatchSize = int.Parse(input.Substring(33, 6).Replace(" ", ""));

                var productTypeString = input[243].ToString();
                p.ProductType = string.IsNullOrWhiteSpace(productTypeString)
                    ? ProductTypeEnum.BLANK
                    : (ProductTypeEnum)Enum.Parse(typeof(ProductTypeEnum), productTypeString);

                if (p.ProductType == ProductTypeEnum.P)
                {
                    p.PackSize = (int)input[246];
                    p.SourceProductId = BitConverter.ToInt16(new byte[] { (byte)input[244], (byte)input[245] }, 0);
                }
                else
                {
                    p.PackSize = 0;
                    p.SourceProductId = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong when parsing the product: " + ex.Message);
                return null;
            }

            return p;
        }

        public override bool Equals(object product)
        {
            if (product is Product p)
            {
                return p.ProductId == this.ProductId;
            }
            return false;
        }

        public override string ToString()
        {
            return this.ProductName;
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }
    }

    class Data
    {
        private static Data instance;

        private List<Product> products = new List<Product>();
        private List<Customer> customers = new List<Customer>();
        private List<Order> orders = new List<Order>();
        private List<ProductTotal> productTotals = new List<ProductTotal>();

        private string myDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string bakeryDirectory;
        private Font printFont;

        private Data()
        {

        }

        public static Data GetInstance()
        {
            if (instance == null)
            {
                instance = new Data();
                return instance;
            }
            else
            {
                return instance;
            }
        }

        public List<Product> GetProducts()
        {
            return products;
        }

        public List<Customer> GetCustomers()
        {
            return customers;
        }

        public List<Order> GetOrders()
        {
            return orders;
        }             

        public List<Order> GetOrders(DayOfWeek day)
        {
            return orders.Where(o => o.OrderDay == day).ToList();
        }

        public List<ProductTotal> GetProductTotals()
        {
            return productTotals;
        }

        public void ClearAllData()
        {
            ClearAllDataExceptProductGroups();
        }

        public void ClearAllDataExceptProductGroups()
        {
            products.Clear();
            customers.Clear();
            orders.Clear();
            productTotals.Clear();
        }

        public void ClearProducts()
        {
            products.Clear();
        }

        public void ClearCustomers()
        {
            customers.Clear();
        }

        public void ClearOrders()
        {
            orders.Clear();
        }

        public void ClearProductTotals()
        {
            productTotals.Clear();
        }

        public void AddProduct(Product p)
        {
            if (!products.Contains(p))
                products.Add(p);
        }

        public void RemoveProduct(Product p)
        {
            products.Remove(p);
        }

        public void AddCustomer(Customer c)
        {
            if (!customers.Contains(c))
                customers.Add(c);
        }

        public void RemoveCustomer(Customer c)
        {
            customers.Remove(c);
        }

        public void AddOrder(Order o)
        {
            if (!orders.Contains(o))
                orders.Add(o);
        }

        public void RemoveOrder(Order o)
        {
            orders.Remove(o);
        }

        public void AddProductTotal(ProductTotal pt)
        {
            if (!productTotals.Contains(pt))
                productTotals.Add(pt);
        }

        public void RemoveProductTotal(ProductTotal pt)
        {
            productTotals.Remove(pt);
        }

        public string GetBakeryDirectory()
        {
            return bakeryDirectory;
        }

        public void SetBakeryDirectory(string directory)
        {
            bakeryDirectory = directory;
        }

        public Font GetPrintFont()
        {
            return printFont;
        }

        public void SetPrintFont(Font font)
        {
            printFont = font;
        }

        public string GetMyDocumentsFolder()
        {
            return myDocumentsFolder;
        }

        public static string GetSoftwareVersion()
        {
            return "1.2.1";
        }
    }

    class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }

        private Customer()
        {

        }

        public static Customer ParseCustomer(string input)
        {
            if (input.Length != 255)
                return null;

            var c = new Customer();

            try
            {
                c.CustomerID = int.Parse(input.Substring(0, 4).Replace(" ", ""));
                c.CustomerName = input.Substring(4, 25).Trim();

                return c;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong when parsing the customer: " + ex.Message);
                return null;
            }
        }
    }

    class Order
    {
        public Customer Customer { get; set; }
        public DayOfWeek OrderDay { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public Order(Customer customer, DayOfWeek orderDay)
        {
            Customer = customer;
            OrderDay = orderDay;
            OrderItems = new List<OrderItem>();
        }

        public void AddOrderItem(OrderItem oi)
        {
            OrderItems.Add(oi);
            oi.Order = this;
        }
    }

    class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }

        public OrderItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public override string ToString()
        {
            string productName = Product != null ? Product.ProductName : "Unknown product";
            return productName + ": " + Quantity;
        }
    }

    class ProductTotal : OrderItem
    {
        public ProductTotal(Product p, int q) : base(p, q)
        {

        }

        public void IncreaseQuantity(int value)
        {
            Quantity += value;
        }
    }

    class loadAllDAta
    {
        public static bool ResetData()
        {

            try
            {
                Data dataInstance = Data.GetInstance();
                dataInstance.ClearAllData();
                var dataLoader = new DataLoader("");

                if (!dataLoader.LoadAllData())
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    class testingGrounds
    {
        public static void GenProductsTotal(DayOfWeek selectedDay)
        {
            string localDir = (string)Settings.Default["Local"];
            string GenSheets = (string)Settings.Default["GenSheets"];
            string filePath = Path.Combine(localDir, "PRODUCTS_TOTAL.txt");
            string excelFilePath = Path.Combine(GenSheets, $"ProductionHelper_{selectedDay}.xlsx");


            // Clear the text file
            File.WriteAllText(filePath, string.Empty);

            // Get orders from customers for the selected day
            List<Order> orders = Data.GetInstance().GetOrders(selectedDay);

            // Dictionary to store the total quantity for each product
            Dictionary<int, int> productTotals = new Dictionary<int, int>();

            // Iterate through all products and initialize total quantity to 0
            foreach (Product product in Data.GetInstance().GetProducts())
            {
                productTotals.Add(product.ProductId, 0);
            }

            // Iterate through orders and update product totals for the selected day
            foreach (Order order in orders)
            {
                foreach (OrderItem orderItem in order.OrderItems)
                {
                    int productId = orderItem.Product.ProductId;

                    // Update existing total only for the selected day
                    productTotals[productId] += orderItem.Quantity;
                }
            }

            // Write product totals to the text file for products ordered on the selected day
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var productTotal in productTotals)
                    {
                        // Get product details using FirstOrDefault
                        Product product = Data.GetInstance().GetProducts().FirstOrDefault(p => p.ProductId == productTotal.Key);

                        // Write product details and total quantity to the file for products ordered on the selected day
                        if (productTotal.Value > 0)
                        {
                            writer.WriteLine($"{product.ProductId} {product.ProductName} {productTotal.Value}");
                        }
                    }
                }

                //ProductionHelp.ConvertTextToExcel(filePath, excelFilePath);
                // Open file in notepad
                System.Diagnostics.Process.Start("notepad.exe", filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to PRODUCTS_TOTAL.txt: {ex.Message}");
                // or log the error to a log file
            }
        }



    }

}

