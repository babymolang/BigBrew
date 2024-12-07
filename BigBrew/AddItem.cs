using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BigBrew
{
    public partial class AddItem : Form // This is really just the Registration Form lmao
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Inventory"].ConnectionString; //Currently there is only Logins and Inventory Databases.
        private const int CornerRadius = 20; //Higher number makes more rounded corners
        private string emailaddress;
        private string name;
        private string password;
        private string contact;
        public AddItem()
        {
            InitializeComponent();
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

        private void AddItem_Load(object sender, EventArgs e)
        {
            LoadCategories();
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
        }

        private void FormClose(object sender, FormClosedEventArgs e)
        {

        }
    }
}
