using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Documents;
using System.Windows.Forms;

namespace BigBrew
{
    public partial class EmployeeInventory : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Inventory"].ConnectionString; //Currently there is only Logins and Inventory Databases.

        public EmployeeInventory(string loginstatus, string loginname)
        {
            InitializeComponent();
            label1.Text = "Employee: " + loginname;
            this.Font = new Font("Times New Roman", this.Font.Size);
            Button closeButton = new Button// Custom Close Button cause wynaut
            {
                Text = "X",
                Size = new Size(30, 30),
                Location = new Point(this.Width - 40, 10),
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);
        }

        private void LoadInventory()
        {
            string query = "SELECT Id, Name, Price, Quantity FROM Inventory";
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(table);
            }
            DataGrid.DataSource = table;
            HighlightLowStockItems();
        }
        

        private void EmployeeInventory_Load(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void LogOutBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginPage login = new LoginPage();
            login.Show();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Inventory (Name, Price, Quantity) VALUES (@Name, @Price, @Quantity)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", NameText.Text);
                command.Parameters.AddWithValue("@Price", PriceText.Text);
                command.Parameters.AddWithValue("@Quantity", QuantText.Text);
                command.Parameters.AddWithValue("@MaxQuantity", 20);
                //command.Parameters.AddWithValue("@Category", )

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                LoadInventory();
                MessageBox.Show("Item Added Successfully");
            }
        }
        private void HighlightLowStockItems()
        {
            string query = "SELECT Id, Name, Price, Quantity, MaxQuantity, Category FROM Inventory";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable inventoryTable = new DataTable();
                adapter.Fill(inventoryTable);

                DataGrid.DataSource = inventoryTable;

                foreach (DataGridViewRow row in DataGrid.Rows)
                {
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    int threshold = Convert.ToInt32(row.Cells["MaxQuantity"].Value);

                    if (quantity <= threshold / 2)
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;  // Change color to red to highlight low stock
                        row.DefaultCellStyle.ForeColor = Color.White;  // Change text color to white for visibility
                    }
                }
            }
        }
        private void RefreshFormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is AddItem add)
            {
                LoadInventory();
            }
            if (sender is UpdateStock update)
            {
                LoadInventory();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateStock update = new UpdateStock();
            update.FormClosed += RefreshFormClosed;
            update.Show();
        }
    }
}
