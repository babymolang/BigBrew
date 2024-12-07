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
    public partial class RegisterForm : Form // This is really just the Registration Form lmao
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Logins"].ConnectionString; //Currently there is only Logins and Inventory Databases.
        private const int CornerRadius = 20; //Higher number makes more rounded corners
        private string emailaddress;
        private string name;
        private string password;
        private string contact;
        public RegisterForm()
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

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //Start of Email Check
                if (new EmailAddressAttribute().IsValid(Email.Text.Trim()))
                {
                    emailaddress = Email.Text;
                }
                //End of Email Check
                //Start of Name Check
                if (NameText.Text.Trim().Length < 3)
                {
                    MessageBox.Show("Name must be at least 3 letters long.");
                }
                else
                {
                    name = NameText.Text;
                }
                //End of Name Check
                //Start of Password Check
                if (Password.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter a password.", "Error");
                }
                else if (Password.Text.Trim().Length < 8)
                {
                    MessageBox.Show("Please enter at least 8 characters in your password.");
                }
                else if (Password.Text.Trim() != ConfirmPassword.Text.Trim())
                {
                    MessageBox.Show("Confirm Password does not match with Password.", "Error");
                }
                else
                {
                    password = Password.Text;
                }
                //End of Password Check
                //Start of ContactNo Check
                if (ContactNo.Text.Trim() == "" && !checkBox1.Checked)
                {
                    MessageBox.Show("No Input in Contact No. Please Check the Box if you do not have a Contact Number.", "Error");
                }
                else if (!Regex.IsMatch(ContactNo.Text.Trim(), @"^[\p{L}]+$"))//If ContactNo has letters then it rejects it, otherwise it proceeds.
                {
                    if (ContactNo.Text.Length < 11 && ContactNo.Text.Length != 0)
                    {
                        MessageBox.Show("Enter A Valid Phone Number, 11 Numbers.", "Error");
                    }
                    else
                    {
                        contact = ContactNo.Text.Trim();
                    }
                }

                else if (checkBox1.Checked)
                {
                    contact = "No Contact";
                }
                else//If not, then error.
                {
                    MessageBox.Show("Enter A Valid Phone Number, 11 Numbers.", "Error");
                }
                //End of ContactNo Check

                if (
                    !string.IsNullOrWhiteSpace(emailaddress) &&
                    !string.IsNullOrWhiteSpace(name) &&
                    !string.IsNullOrWhiteSpace(password) &&
                    ((checkBox1.Checked && string.IsNullOrWhiteSpace(contact)) ||
                    (!string.IsNullOrWhiteSpace(contact) && !checkBox1.Checked))
                    )
                {
                    using (SqlConnection connect = new SqlConnection(connectionString))
                    {
                        connect.Open();
                        string query = "INSERT INTO Employees(EmailAddress, Name, Password, ContactNo) VALUES (@EmailAddress, @Name, @Password, @ContactNo)";
                        using (SqlCommand insert = new SqlCommand(query, connect))
                        {
                            insert.Parameters.AddWithValue("@EmailAddress", emailaddress);
                            insert.Parameters.AddWithValue("@Name", name);
                            insert.Parameters.AddWithValue("@Password", password);
                            if (!checkBox1.Checked)
                            {
                                insert.Parameters.AddWithValue("@ContactNo", contact);
                            }
                            else
                            {
                                insert.Parameters.AddWithValue("@ContactNo", "No Contact");
                            }
                            insert.ExecuteNonQuery();
                            MessageBox.Show("Successfully Registered.", "Success");
                            this.Close();
                            connect.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Something went wrong. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                ContactNo.Enabled = false;
            }
            else
            {
                ContactNo.Enabled = true;
            }
        }

        private void NoAccess_Load(object sender, EventArgs e)
        {

        }
    }
}
