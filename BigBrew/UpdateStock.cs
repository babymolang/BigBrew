using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BigBrew
{
    public partial class UpdateStock : Form // This is really just the Registration Form lmao
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Inventory"].ConnectionString; //Currently there is only Logins and Inventory Databases.
        private const int CornerRadius = 20; //Higher number makes more rounded corners
        private string emailaddress;
        private string name;
        private string password;
        private string contact;
        public UpdateStock()
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
        private void LoadInventory()
        {
            string query = "SELECT Id, Name, Quantity FROM Inventory";
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(table);
            }
            DataGrid.DataSource = table;
        }
        private void UpdateStockLevel()
        {
            int quantity = 0;
            try
            {
                if (int.TryParse(QuantityText.Text, out quantity))
                {
                    quantity = int.Parse(QuantityText.Text);
                }

                if (DataGrid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row to Update.");
                    return;
                }
                int id = Convert.ToInt32(DataGrid.SelectedRows[0].Cells[0].Value);
                string query = "UPDATE Inventory SET Quantity = @Quantity WHERE Id = @Id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    LoadInventory();
                    MessageBox.Show("Stock Level Updated Successfully");
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Something went wrong. " + ex.Message);
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

        private void NoAccess_Load(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int quant = 0;
            if (int.TryParse(QuantityText.Text, out quant))
            {
                UpdateStockLevel();
                LoadInventory();
            }
            else
            {
                MessageBox.Show("Please input a valid Numerical Number.", "Error");
            }
        }

        private void NameText_TextChanged(object sender, EventArgs e)
        {
        }

        private void DataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
