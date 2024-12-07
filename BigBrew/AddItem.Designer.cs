namespace BigBrew
{
    partial class AddItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.CategoryBox = new System.Windows.Forms.ComboBox();
            this.MaxQuant = new System.Windows.Forms.TextBox();
            this.ItemName = new System.Windows.Forms.TextBox();
            this.Adding = new System.Windows.Forms.Button();
            this.ItemQuantity = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ItemPrice = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add Item";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 141);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 13);
            this.label13.TabIndex = 25;
            this.label13.Text = "Max Quantity";
            // 
            // CategoryBox
            // 
            this.CategoryBox.FormattingEnabled = true;
            this.CategoryBox.Location = new System.Drawing.Point(230, 55);
            this.CategoryBox.Name = "CategoryBox";
            this.CategoryBox.Size = new System.Drawing.Size(121, 21);
            this.CategoryBox.TabIndex = 14;
            // 
            // MaxQuant
            // 
            this.MaxQuant.Location = new System.Drawing.Point(112, 138);
            this.MaxQuant.Name = "MaxQuant";
            this.MaxQuant.Size = new System.Drawing.Size(100, 20);
            this.MaxQuant.TabIndex = 24;
            // 
            // ItemName
            // 
            this.ItemName.Location = new System.Drawing.Point(112, 56);
            this.ItemName.Name = "ItemName";
            this.ItemName.Size = new System.Drawing.Size(100, 20);
            this.ItemName.TabIndex = 15;
            // 
            // Adding
            // 
            this.Adding.Location = new System.Drawing.Point(276, 235);
            this.Adding.Name = "Adding";
            this.Adding.Size = new System.Drawing.Size(75, 23);
            this.Adding.TabIndex = 16;
            this.Adding.Text = "Add Item";
            this.Adding.UseVisualStyleBackColor = true;
            this.Adding.Click += new System.EventHandler(this.Adding_Click);
            // 
            // ItemQuantity
            // 
            this.ItemQuantity.Location = new System.Drawing.Point(112, 111);
            this.ItemQuantity.Name = "ItemQuantity";
            this.ItemQuantity.Size = new System.Drawing.Size(100, 20);
            this.ItemQuantity.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "Name";
            // 
            // ItemPrice
            // 
            this.ItemPrice.Location = new System.Drawing.Point(112, 82);
            this.ItemPrice.Name = "ItemPrice";
            this.ItemPrice.Size = new System.Drawing.Size(100, 20);
            this.ItemPrice.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 88);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "Price";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Quantity/Stock";
            // 
            // AddItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Peru;
            this.ClientSize = new System.Drawing.Size(379, 270);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.CategoryBox);
            this.Controls.Add(this.MaxQuant);
            this.Controls.Add(this.ItemName);
            this.Controls.Add(this.Adding);
            this.Controls.Add(this.ItemQuantity);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ItemPrice);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddItem";
            this.Text = "RegisterForm1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormClose);
            this.Load += new System.EventHandler(this.AddItem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox CategoryBox;
        private System.Windows.Forms.TextBox MaxQuant;
        private System.Windows.Forms.TextBox ItemName;
        private System.Windows.Forms.Button Adding;
        private System.Windows.Forms.TextBox ItemQuantity;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox ItemPrice;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
    }
}