using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LogPriceChange0._1
{
    public partial class wfLoginform : Form
    {
        public string LoggedInUsername { get; private set; }
        public string LoggedInUserRole { get; private set; }
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\VisualStudio\LogPriceChange0.1\LogPriceChange0.1\pricematrix.accdb;");
        ctrLogPriceChange ctrLogPriceChange = new ctrLogPriceChange();
        

        public wfLoginform()
        {
            InitializeComponent();


            logf_tb_password.UseSystemPasswordChar = true;
            logf_pb_eyeopen.Click += TogglePasswordVisibility;
            logf_pb_eyeclose.Click += TogglePasswordVisibility;
        }
        private void wfLoginform_Load(object sender, EventArgs e)
        {
            
        }
        private void logf_btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "SELECT Lastname, Firstname, EmployeeRole FROM tbl_employee WHERE Username = ? AND EmployeePassword = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", logf_tb_username.Text.Trim());
                    cmd.Parameters.AddWithValue("?", logf_tb_password.Text);

                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            LoggedInUsername = dr["Lastname"]?.ToString() + " " + dr["Firstname"]?.ToString();
                            LoggedInUserRole = dr["EmployeeRole"]?.ToString() ?? string.Empty;
                            UserSession.Username = LoggedInUsername;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Login failed. Please check your username and password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private void NavigateToFormByRole(string username, string role)
        {
            this.Hide();
            Form nextForm = null;

            switch (role.ToLower()) // Compare the role in lowercase for robustness.
            {
                case "admin":
                    nextForm = new AdminForm(username); // Pass the username to the Admin form.
                    break;
                case "user":
                    nextForm = new MainForm(username); // Pass the username to the Employee form.
                    break;
                default:
                    MessageBox.Show("Unknown user role. Access denied.");
                    this.Show(); // Show the login form again for another attempt.
                    return;
            }

            if (nextForm != null)
            {
                nextForm.Show();
            }
        }
        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            
            // Check if the password is currently hidden
            if (logf_tb_password.UseSystemPasswordChar)
            {
                // If hidden, show the password by disabling the PasswordChar
                logf_tb_password.UseSystemPasswordChar = false;

                // Hide the open eye icon and show the closed eye icon
                logf_pb_eyeopen.Visible = false;
                logf_pb_eyeclose.Visible = true;
            }
            else
            {
                // If shown, hide the password by enabling the PasswordChar
                logf_tb_password.UseSystemPasswordChar = true;

                // Show the open eye icon and hide the closed eye icon
                logf_pb_eyeopen.Visible = true;
                logf_pb_eyeclose.Visible = false;
            }
        }
    }
}
