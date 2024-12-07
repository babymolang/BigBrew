using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing.Drawing2D;

namespace BigBrew
{
    public partial class LoginPage : Form
    {
        string loginstatus, loginname;
        private string connectionString = ConfigurationManager.ConnectionStrings["Logins"].ConnectionString; //Currently there is only Logins and Inventory Databases.
        private const int CornerRadius = 20; //Higher number makes more rounded corners
        public LoginPage()
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
            closeButton.Click += (s, e) => Application.Exit();
            this.Controls.Add(closeButton);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            panel2.BackgroundImage = Image.Images[0];
            textBox1.KeyDown += TextBox1_KeyDown;
            textBox2.KeyDown += TextBox2_KeyDown;
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

        private bool AdminChecking(String username, String password)
        {
            try
            {
                string query = "SELECT * FROM Admins";
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();
                    using (SqlCommand request = new SqlCommand(query, connect))
                    using (SqlDataReader reader = request.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            loginname = reader["Name"].ToString();
                            string passwordDB = reader["Password"].ToString();
                            loginstatus = reader["EmailAddress"].ToString();
                            if (name == username && passwordDB == password)
                            {
                                
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return false;
        }
        private bool EmployeeChecking(String username, String password)
        {
            try
            {
                string query = "SELECT * FROM Employees";
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();
                    using (SqlCommand request = new SqlCommand(query, connect))
                    using (SqlDataReader reader = request.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            loginname = reader["Name"].ToString();
                            string passwordDB = reader["Password"].ToString();
                            loginstatus = reader["EmailAddress"].ToString();
                            if (name == username && passwordDB == password)
                            {

                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return false;
        }
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoginBtn.PerformClick();
            }
        }
        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoginBtn.PerformClick();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void LoginBtn_Click_1(object sender, EventArgs e)
        {
            String username = textBox1.Text;
            String password = textBox2.Text;
            try
            {
                if (AdminCheck.Checked)
                {
                    if (AdminChecking(username, password))
                    {
                        this.Visible = false;
                        AdminPage adminpage = new AdminPage(loginstatus, loginname);
                        adminpage.Show();

                    }
                    else
                    {
                        MessageBox.Show("Check your credentials and try again.");
                    }

                }
                else
                {
                    if (EmployeeChecking(username, password))
                    {
                        this.Visible = false;
                        EmployeeInventory employee = new EmployeeInventory(loginstatus, loginname);
                        employee.Show();
                    }
                    else
                    {

                        MessageBox.Show("Check your credentials and try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error. " + ex.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm forgot = new RegisterForm();
            forgot.Show();
        }

        //The Code to Send to Emails
        /*private void SendBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                
                // Create a new MailMessage
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(From.Text); // Sender's email address
                mail.To.Add(To.Text); // Recipient's email address
                mail.Subject = Subject.Text; // Email subject
                mail.Body = Body.Text; // Email body

                // Configure SMTP client
                SmtpClient sendemail = new SmtpClient("smtp.gmail.com", 587); // Use Gmail's SMTP server
                sendemail.Credentials = new NetworkCredential("kylecalolot@gmail.com", "hyue owui qqus rpyf"); // Sender's credentials
                sendemail.EnableSsl = true; // Enable SSL

                // Send the email
                sendemail.Send(mail);
                MessageBox.Show("Email sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send email. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
    }
}
