using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace LogPriceChange0._1
{
    public partial class MainForm : Form
    {
        wfLoginform loginForm = new wfLoginform();
        private string _username;
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Desktop\CameraHaus\LogPriceChange_Demo\pricematrix.accdb;";

        #region Methods
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
        private void UpdateIsLoginTofalse()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE tbl_employee SET IsLoggedIn = false WHERE Username = ?";
                using (OleDbCommand cmdUpdate = new OleDbCommand(updateQuery, conn))
                {
                    // Parameters are added positionally for the UPDATE query
                    cmdUpdate.Parameters.AddWithValue("?", _username); // Only one parameter needed
                    cmdUpdate.ExecuteNonQuery();

                    conn.Close();
                }
            }
        }
        #endregion

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
            UpdateIsLoginTofalse();
            this.Hide();
            loginForm.Show();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateIsLoginTofalse();

            Application.Exit();
        }
    }
}
