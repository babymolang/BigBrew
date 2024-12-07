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
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;
using Paragraph = iTextSharp.text.Paragraph;

namespace BigBrew
{
    public partial class AdminPage : Form
    {
        private string loginstatus, loginname;
        string connectionString = ConfigurationManager.ConnectionStrings["Inventory"].ConnectionString; //Currently there is only Logins and Inventory Databases.
        private const int CornerRadius = 20; //Higher number makes more rounded corners
        public AdminPage(string loginstatus, string loginname)
        {
            InitializeComponent();
            this.loginstatus = loginstatus;
            AdminName.Text = "Admin: " + loginname;
        }
        protected override void OnPaint(PaintEventArgs e) // Just For the Rounded Form
        {
            base.OnPaint(e);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90);
            path.AddArc(this.Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90);
            path.AddArc(this.Width - CornerRadius, this.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
            path.AddArc(0, this.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
            path.CloseAllFigures();
            this.Region = new Region(path);
        }

        protected override void OnMouseDown(MouseEventArgs e) // Just For the Rounded Form
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
                this.Tag = e.Location;
        }

        protected override void OnMouseMove(MouseEventArgs e) // Just For the Rounded Form
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left && this.Tag is Point mouseDownLocation)
            {
                this.Location = new Point(
                    this.Location.X + e.X - mouseDownLocation.X,
                    this.Location.Y + e.Y - mouseDownLocation.Y);
            }
        }

        protected override void OnResize(EventArgs e) // Just For the Rounded Form
        {
            base.OnResize(e);
            this.Invalidate();
        }


        private void EmployeePage_Load(object sender, EventArgs e)
        {
            LoadInventory();
            CheckStockLevels();
            LoadCategories();
            
                
        }
        private void LoadCategories()
        {
            CategoryBox.Items.Clear();
            string query = "SELECT Name FROM Categories";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CategoryBox.Items.Add(reader["Name"].ToString());
                    }
                }
            }
            if (CategoryBox.Items.Count > 0) // Default Selectiion
            {
                CategoryBox.SelectedIndex = 0;
            }

            else
            {
                MessageBox.Show("No categories found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadInventory()
        {
            string query = "SELECT Id, Name, Price, Quantity, MaxQuantity, Category FROM Inventory";
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(table);
            }
            DataGridView.DataSource = table;
            HighlightLowStockItems();
        }
        /*private void panelLoad() // Method to trigger PanelLoad which allows for refreshes if called again.
        {
            string query = "SELECT * FROM Inventory"; // Makes sure that the PROGRAM looks ONLY INTO THE INVENTORY TABLE.
            //Just a bunch of Restrictions because for some reason anyone can resize the Data Grid lmao
            DataGridView.AllowUserToResizeColumns = false; // No Column Resizing
            DataGridView.AllowUserToResizeRows = false;    // No Row Resizing
            DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells; // Fits Columns to Displayed Data
            DataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;       // No Auto Sizing Rows
            DataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing; // No Row Header Resizing for Width
            DataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; // No Row Header Resizing for Height
            DataGridView.ScrollBars = ScrollBars.Both; // Ensure scrolling works if the data overflows
            DataGridView.AllowUserToAddRows = false; // Removes the Extra White Space that allows user to add more for some reason
            //Manual Data Connection

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DataGridView.Rows.Clear();//Clearing the Data Grid. Required because if not cleared, the Data Grid will make duplicate Entries
                    DataGridView.Columns.Clear();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++) //Creates the Columns ID, Name, Price, Quantity
                        {
                            string columnName = reader.GetName(i);
                            DataGridView.Columns.Add(columnName, columnName.ToUpper());
                        }


                        while (reader.Read())//Writes the Data and adds it to the Data Grid
                        {
                            object[] rowData = new object[reader.FieldCount];
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                rowData[i] = reader[i].ToString();
                            }
                            DataGridView.Rows.Add(rowData);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"An error occurred: {error.Message}");
            }*/


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginPage login = new LoginPage();
            login.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void RefreshFormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is ConnectSettings settingsForm)
            {
                if (this.loginstatus != settingsForm.UpdatedLoginStatus)
                {
                    this.loginstatus = settingsForm.UpdatedLoginStatus;
                    MessageBox.Show($"Login status updated: {this.loginstatus}");
                }
            }
            if (sender is AddItem add)
            {
                LoadInventory();
            }
                
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConnectSettings setting = new ConnectSettings(loginstatus);
            setting.FormClosed += RefreshFormClosed;
            setting.Show();

        }

        private void Report(object sender, EventArgs e)
        {
            new GenerateReport(loginstatus);
        }

        private void Adding_Click(object sender, EventArgs e)
        {
            string name = ItemName.Text;
            double price = 0;
            int quantity = 0;
            int maxquant = 0;
            string selectedCategory = CategoryBox.SelectedItem?.ToString();

            // Validation
            if (string.IsNullOrEmpty(name) || !double.TryParse(ItemPrice.Text, out price) || !int.TryParse(ItemQuantity.Text, out quantity) || !int.TryParse(MaxQuant.Text, out maxquant))
            {
                MessageBox.Show("Please enter valid item details.");
                return;
            }

            string query = "INSERT INTO Inventory (Name, Price, Quantity, MaxQuantity, Category) VALUES (@Name, @Price, @Quantity, @MaxQuant, @Category)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@MaxQuant", maxquant);
                cmd.Parameters.AddWithValue("@Category", selectedCategory);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }

            LoadInventory();
        }
        private void Edit_Click(object sender, EventArgs e)
        {
            if (DataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to edit.");
                return;
            }

            int id = Convert.ToInt32(DataGridView.SelectedRows[0].Cells[0].Value);
            string name = ItemName.Text;
            double price = 0;
            int quantity = 0;
            int maxquant = 0;

            if (!double.TryParse(ItemPrice.Text, out price) || !int.TryParse(ItemQuantity.Text, out quantity) || !int.TryParse(MaxQuant.Text, out maxquant))
            {
                MessageBox.Show("Please enter valid item details.");
                return;
            }

            string query = "UPDATE Inventory SET Name = @Name, Price = @Price, Quantity = @Quantity, MaxQuantity = @MaxQuantity WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@MaxQuant", maxquant);
                cmd.Parameters.AddWithValue("@Id", id);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }

            LoadInventory();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (DataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            int id = Convert.ToInt32(DataGridView.SelectedRows[0].Cells[0].Value);
            string query = "DELETE FROM Inventory WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }

            LoadInventory();
        }
        private void CheckStockLevels()
        {
            string query = "SELECT Id, Name, Price, Quantity, MaxQuantity FROM Inventory WHERE Quantity <= MaxQuantity / 2";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable lowStockItems = new DataTable();
                adapter.Fill(lowStockItems);

                if (lowStockItems.Rows.Count > 0)
                {
                    string alertMessage = "Low stock items:\n";

                    foreach (DataRow row in lowStockItems.Rows)
                    {
                        string itemName = row["Name"].ToString ();
                        int quantity = Convert.ToInt32(row["Quantity"]);
                        int threshold = Convert.ToInt32(row["MaxQuantity"]);
                        alertMessage += $"{itemName} - Quantity: {quantity} (MaxQuantity: {threshold})\n";
                    }
                    MessageBox.Show(alertMessage, "Stock Alerts", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("All items have sufficient stock.", "Inventory Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckStockLevels();
            HighlightLowStockItems();
        }
        private void HighlightLowStockItems()
        {
            string query = "SELECT Id, Name, Price, Quantity, MaxQuantity, Category FROM Inventory";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable inventoryTable = new DataTable();
                adapter.Fill(inventoryTable);

                DataGridView.DataSource = inventoryTable;

                foreach (DataGridViewRow row in DataGridView.Rows)
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormClose(object sender, FormClosedEventArgs e)
        {
            this.Close();
            LoginPage login = new LoginPage();
            login.Show();
        }

        private void AddItem_Click(object sender, EventArgs e)
        {
            AddItem add = new AddItem();
            add.FormClosed += RefreshFormClosed;
            add.Show();
        }
    }




}
