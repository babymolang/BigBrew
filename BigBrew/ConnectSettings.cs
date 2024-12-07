using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace BigBrew
{
    public partial class ConnectSettings : Form // This is really just the Registration Form lmao
    {
        private string loginstatus;
        private const int CornerRadius = 20; //Higher number makes more rounded corners
        public string UpdatedLoginStatus { get; private set; }

        private string currentLoginStatus;
        public ConnectSettings(string loginstatus)
        {
            InitializeComponent();
            currentLoginStatus = loginstatus;
            UpdatedLoginStatus = loginstatus;
            this.loginstatus = loginstatus;
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

        private void NoAccess_Load(object sender, EventArgs e)
        {
            textBox1.Text = loginstatus;
            textBox1.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(new EmailAddressAttribute().IsValid(textBox1.Text.Trim()))
            {
                UpdatedLoginStatus = textBox1.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid email address.", "Error");
            }
        }
        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.Enabled = true;
            }
            else
            {
                {
                    textBox1.Enabled = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
