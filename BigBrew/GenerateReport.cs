using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Windows.Forms;
using System.Configuration;

namespace BigBrew
{
    internal class GenerateReport
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Inventory"].ConnectionString; //Currently there is only Logins and Inventory Databases.
        public GenerateReport(string loginstatus) {
            string query = "SELECT Id, Name, Price, Quantity, MaxQuantity, Category FROM Inventory"; // Gets the Data
            DataTable inventoryTable = new DataTable(); // Puts Data in a Table

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(inventoryTable); // Fills the Table with the Data
                    }
                }
                string pdfPath = "InventoryReport.pdf"; // Changeable Path Directory. Example: "C:\Users\Admin\Desktop\InventoryReport.pdf"
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 20f); // Initializing the Document
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(pdfPath, FileMode.Create)); // Creates the Pdf

                pdfDoc.Open(); // Allows the pdf to be edited
                Font titleFont = FontFactory.GetFont("Arial", 18); // Title fonts
                pdfDoc.Add(new Paragraph("Inventory Report", titleFont)); // Structured like an html, sort of.
                pdfDoc.Add(new Paragraph("\n")); // Add spacing
                PdfPTable table = new PdfPTable(6); // Creates a Table in the PDF, specifically 5 columns
                table.WidthPercentage = 100;//Sets the width by Percentage
                Font headerFont = FontFactory.GetFont("Arial", 12);// Le Headers for the Column Names
                table.AddCell(new PdfPCell(new Phrase("Id", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Name", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Price", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Max Quantity", headerFont)));
                table.AddCell(new PdfPCell(new Phrase("Category", headerFont)));
                Font rowFont = FontFactory.GetFont("Arial", 10);
                foreach (DataRow row in inventoryTable.Rows) // grabs the data from inventoryTable aka. Data Table. Simplified. For each row in inventoryTable, loop.
                {
                    table.AddCell(new PdfPCell(new Phrase(row["Id"].ToString(), rowFont)));
                    table.AddCell(new PdfPCell(new Phrase(row["Name"].ToString(), rowFont)));
                    table.AddCell(new PdfPCell(new Phrase(row["Price"].ToString(), rowFont)));
                    table.AddCell(new PdfPCell(new Phrase(row["Quantity"].ToString(), rowFont)));
                    table.AddCell(new PdfPCell(new Phrase(row["MaxQuantity"].ToString(), rowFont)));
                    table.AddCell(new PdfPCell(new Phrase(row["Category"].ToString(), rowFont)));
                }
                pdfDoc.Add(table); // Adds the table to the document.
                pdfDoc.Close();// End of Editing

                // Le MessageBox for checking if its successful
                MessageBox.Show($"PDF file has been successfully created at: {Path.GetFullPath(pdfPath)}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Le Email Notification
                if (MessageBox.Show("Do you want it to be sent to your email as well?", "Send to Email", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        MailMessage mail = new MailMessage() // Mail Contents
                        {
                            From = new MailAddress("kylecalolot@gmail.com"), // Sender's email address
                            //Need to Rework this to send Emails directly to the current Logged In Admin's Email
                            Subject = "Inventory Report" + DateTime.Now.ToString("MM\\/dd\\/yyyy h\\:mm tt"), // Email subject
                            Body = Path.GetFullPath(pdfPath) // Email body
                        };
                        mail.To.Add(loginstatus); // Recipient's email address
                        Attachment attachment = new Attachment(pdfPath);
                        mail.Attachments.Add(attachment);
                        SmtpClient sendemail = new SmtpClient("smtp.gmail.com", 587); // Gmail's Smtp Server. Annoying to find.
                        sendemail.Credentials = new NetworkCredential("kylecalolot@gmail.com", "hyue owui qqus rpyf"); // Sender's credentials // App Password is the second one. Plz don't hack me
                        sendemail.EnableSsl = true; // Enable SSL
                        sendemail.Send(mail);//Send
                        MessageBox.Show("Report sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        attachment.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to send report. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
