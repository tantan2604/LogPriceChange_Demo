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
            try {
                string username = logf_tb_username.Text.Trim();
                string password = logf_tb_password.Text;
                using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\SharedDB\pricematrix.accdb;"))
                {
                    try
                    {
                        conn.Open();

                        string checkLoginQuery = "SELECT Lastname, Firstname, EmployeeRole, IsLoggedIn FROM tbl_employee WHERE Username = ? AND EmployeePassword = ?";

                        using (OleDbCommand cmdCheck = new OleDbCommand(checkLoginQuery, conn))
                        {
                            cmdCheck.Parameters.AddWithValue("?", username);
                            cmdCheck.Parameters.AddWithValue("?", password);

                            using (OleDbDataReader dr = cmdCheck.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    bool isLoggedIn = dr.GetBoolean(dr.GetOrdinal("IsLoggedIn"));

                                    if (isLoggedIn)
                                    {
                                        MessageBox.Show("This account is already logged in.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                    string updateQuery = "UPDATE tbl_employee SET IsLoggedIn = true WHERE Username = ?";
                                    using (OleDbCommand cmdUpdate = new OleDbCommand(updateQuery, conn))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("?", username);
                                        cmdUpdate.ExecuteNonQuery();
                                    }

                                    // Store the full name for display, but keep the original username.
                                    string loggedInFullName = dr["Lastname"]?.ToString() + " " + dr["Firstname"]?.ToString();
                                    string loggedInUserRole = dr["EmployeeRole"]?.ToString() ?? string.Empty;

                                    this.DialogResult = DialogResult.OK;
                                    this.Hide();
                                    Form nextForm = null;

                                    switch (loggedInUserRole.ToLower())
                                    {
                                        case "admin":
                                            nextForm = new AdminForm(username); // Pass the correct username
                                            break;
                                        case "user":
                                            nextForm = new MainForm(username); // Pass the correct username
                                            break;
                                        default:
                                            MessageBox.Show("Unknown user role. Access denied.");
                                            this.Show();
                                            nextForm = null;
                                            return;
                                    }

                                    if (nextForm != null)
                                    {
                                        nextForm.Show();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Login failed. Please check your username and password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during login: " + ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
