using Delete_Push_Pull.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Delete_Push_Pull
{
    public partial class Config : Form
    {
        private string DataFilePath = (string)Settings.Default["Local"] + @"\productTrayData.json"; // Adjust the file path as needed

        private Dictionary<string, List<float>> productTrayMapping;
        public Config()
        {
            InitializeComponent();
            InitializeData();
            LoadProductIDs();
            listBoxProductIDs.SelectedIndexChanged += listBoxProductIDs_SelectedIndexChanged;
            buttonAddProductID.Click += buttonAddProductID_Click;
            buttonDeleteTray.Click += buttonDeleteTray_Click;
            FormClosing += MainForm_FormClosing;

            buttonDeleteTray.Enabled = false;

        }
        private void InitializeData()
        {
            // Load data from file or create a new dictionary if the file doesn't exist
            if (File.Exists(DataFilePath))
            {
                string json = File.ReadAllText(DataFilePath);
                productTrayMapping = JsonConvert.DeserializeObject<Dictionary<string, List<float>>>(json);
            }
            else
            {
                productTrayMapping = new Dictionary<string, List<float>>();
            }
        }
        private void LoadProductIDs()
        {
            // Load all product IDs into the ListBox
            foreach (string productId in productTrayMapping.Keys)
            {
                listBoxProductIDs.Items.Add(productId);
            }
        }

        private void listBoxProductIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle the selected index change event
            if (listBoxProductIDs.SelectedIndex != -1)
            {
                string selectedProductId = listBoxProductIDs.SelectedItem.ToString();

                // Check if the product ID is in the mapping
                if (productTrayMapping.ContainsKey(selectedProductId))
                {
                    List<float> trayList = productTrayMapping[selectedProductId];

                    // Display the tray list in the ListBox
                    listBoxTrayList.Items.Clear();
                    foreach (float trayId in trayList)
                    {
                        listBoxTrayList.Items.Add(trayId);
                    }

                    // Enable the delete button when a product ID is selected
                    buttonDeleteTray.Enabled = true;
                }
                else
                {
                    listBoxTrayList.Items.Clear();
                    listBoxTrayList.Items.Add($"Tray list not found for product ID {selectedProductId}");

                    // Disable the delete button when the product ID is not found
                    buttonDeleteTray.Enabled = false;
                }
            }
            else
            {
                // Clear the tray list and disable the delete button when no product ID is selected
                listBoxTrayList.Items.Clear();
                buttonDeleteTray.Enabled = false;
            }
        }

        private void buttonAddProductID_Click(object sender, EventArgs e)
        {
            // Add a new float product ID to the selected product's tray list
            if (listBoxProductIDs.SelectedIndex != -1 && float.TryParse(textBoxAddProductID.Text, out float newProductId))
            {
                string selectedProductId = listBoxProductIDs.SelectedItem.ToString();

                // Check if the product ID is in the mapping
                if (productTrayMapping.ContainsKey(selectedProductId))
                {
                    // Add the new product ID to the tray list
                    productTrayMapping[selectedProductId].Add(newProductId);

                    // Refresh the tray list display
                    listBoxTrayList.Items.Clear();
                    foreach (float trayId in productTrayMapping[selectedProductId])
                    {
                        listBoxTrayList.Items.Add(trayId);
                    }
                }
                else
                {
                    MessageBox.Show($"Tray list not found for product ID {selectedProductId}");
                }
            }
            else
            {
                MessageBox.Show("Invalid new product ID. Please enter a valid float number.");
            }
        }




        private void buttonDeleteTray_Click(object sender, EventArgs e)
        {
            // Delete the selected tray from the list
            if (listBoxTrayList.SelectedIndex != -1 && listBoxProductIDs.SelectedIndex != -1)
            {
                string selectedProductId = listBoxProductIDs.SelectedItem.ToString();

                if (productTrayMapping.ContainsKey(selectedProductId))
                {
                    float selectedTray = (float)listBoxTrayList.SelectedItem;

                    // Remove the selected tray from the tray list
                    productTrayMapping[selectedProductId].Remove(selectedTray);

                    // Refresh the tray list display
                    listBoxTrayList.Items.Clear();
                    foreach (float trayId in productTrayMapping[selectedProductId])
                    {
                        listBoxTrayList.Items.Add(trayId);
                    }

                    // Save the updated data
                    SaveData();
                }
            }
            else
            {
                MessageBox.Show("Please select a tray to delete.");
            }
        }




        private void SaveData()
        {
            // Save data to file
            string json = JsonConvert.SerializeObject(productTrayMapping, Formatting.Indented);
            File.WriteAllText(DataFilePath, json);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save data before closing the application
            SaveData();
        }
    }
}
