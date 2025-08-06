using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace LogPriceChange0._1
{
    public partial class MainForm : Form
    {
        private string _username;
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\SharedDB\pricematrix.accdb;";

        public MainForm(string username)
        {
           
            InitializeComponent();
            _username = username;
            this.Text = "Employee Dashboard";
            UserSession.Username = _username;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (this.Controls.Find("dashb_lbl_userlogged", true).Length > 0)
            {
                Label userLabel = (Label)this.Controls.Find("dashb_lbl_userlogged", true)[0];
                userLabel.Text = GetFullName(_username);
            }

            // Load ctrDashboard by default when the form loads
            pnl_main.Controls.Clear();
                ctrDashboard ctrdash = new ctrDashboard();
                ctrdash.Dock = DockStyle.Fill;
                pnl_main.Controls.Add(ctrdash);    
        }
        private void btn_dashboard_Click(object sender, EventArgs e)
        {
            pnl_main.Controls.Clear();
            ctrDashboard ctrdash = new ctrDashboard();
            ctrdash.Dock = DockStyle.Fill;
            pnl_main.Controls.Add(ctrdash);
        }
        // Event handlers for button clicks to load different user controls
        private void btn_lpc_Click(object sender, EventArgs e)
        {
            pnl_main.Controls.Clear();
            ctrLogPriceChange ctrlpc = new ctrLogPriceChange();
            ctrlpc.Dock = DockStyle.Fill;
            pnl_main.Controls.Add(ctrlpc);

        }
        private void btn_claimbysellout_Click(object sender, EventArgs e)
        {
            pnl_main.Controls.Clear();
            ctrClaimBySellOut ctrcbso = new ctrClaimBySellOut();
            ctrcbso.Dock = DockStyle.Fill;
            pnl_main.Controls.Add(ctrcbso);
        }
        private void btn_claimbyinventory_Click(object sender, EventArgs e)
        {
            pnl_main.Controls.Clear();
            ctrClaimByInventory ctrcbi = new ctrClaimByInventory();
            ctrcbi.Dock = DockStyle.Fill;
            pnl_main.Controls.Add(ctrcbi);
        }
        private void btn_logout_Click(object sender, EventArgs e)
        {
            string updateQuery = "UPDATE tbl_employee SET IsLoggedIn = ? WHERE username = ?";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(updateQuery, connection))
                {
                    command.Parameters.Add("IsLoggedIn", OleDbType.Boolean).Value = false;
                    command.Parameters.Add("username", OleDbType.VarWChar, 255).Value = _username;

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        

                        if (rowsAffected > 0)
                        {
                            // Optional: Show a success message to confirm the update
                            this.Hide();
                            wfLoginform loginForm = new wfLoginform();  
                            loginForm.ShowDialog();


                        }
                        else
                        {
                            // No rows were updated. The username may not exist.
                            MessageBox.Show("No user found to update.");
                            MessageBox.Show("Username to update: '" + _username + "'");


                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating log status: " + ex.Message);
                     
                    }
                }
            }
        }
        public string GetFullName(string username)
        {
            string fullName = string.Empty;
            string query = "SELECT Lastname, Firstname FROM tbl_employee WHERE Username = ?";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    // The parameter is for the username
                    command.Parameters.Add("?", OleDbType.VarWChar, 255).Value = username;

                    try
                    {
                        connection.Open();
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Concatenate Lastname and Firstname from the database
                                string lastName = reader["Lastname"].ToString();
                                string firstName = reader["Firstname"].ToString();
                                fullName = $"{lastName} {firstName}";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error retrieving full name: " + ex.Message, "Database Error");
                    }
                }
            }
            return fullName;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Your code to update the IsLoggedIn status to false
            string username = UserSession.Username;
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\SharedDB\pricematrix.accdb;";
            string updateQuery = "UPDATE tbl_employee SET IsLoggedIn = ? WHERE Username = ?";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    using (OleDbCommand cmdUpdate = new OleDbCommand(updateQuery, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("?", false);
                        cmdUpdate.Parameters.AddWithValue("?", username);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on logout: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Now, let the application close gracefully.
            Application.Exit();
        }
    }
}
