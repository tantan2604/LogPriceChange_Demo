using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace LogPriceChange0._1
{
    public partial class MainForm : Form
    {
  
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\SharedDB\pricematrix.accdb;";
        private Button activeButton = null;
        wfLoginform loginForm = new wfLoginform();
        private string _username;
        // Define fields to hold control instances
        private ctrDashboard _ctrDashboard;
        private ctrLogPriceChange _ctrLogPriceChange;
        private ctrClaimBySellOut _ctrClaimBySellOut;
        private ctrClaimByInventory _ctrClaimByInventory;


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

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn != activeButton)
            {
                btn.ForeColor = Color.White;  // Text color on hover
                btn.BackColor = ColorTranslator.FromHtml("#d11018");  // Background color on hover
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn != activeButton)
            {
                btn.ForeColor = Color.Silver;  // Default text color
                btn.BackColor = Color.Transparent;  // Default background color
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedBtn = sender as Button;
            if (clickedBtn == null) return;

            // Reset all buttons
            foreach (Control ctrl in pnl_navbar.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.ForeColor = Color.Silver;
                    btn.BackColor = Color.Transparent;  // Reset background
                }
            }

            // Set new active button
            activeButton = clickedBtn;

            // Highlight clicked button
            clickedBtn.ForeColor = Color.White;
            clickedBtn.BackColor = ColorTranslator.FromHtml("#d11018");
        }

        #endregion
        public ctrLogPriceChange logPriceChangeControl;
        public MainForm(string username)
        {

            InitializeComponent();
            _username = username;
            this.Text = "Employee Dashboard";
            UserSession.Username = _username;
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            logPriceChangeControl = new ctrLogPriceChange();
            logPriceChangeControl.Dock = DockStyle.Fill;
            pnl_main.Controls.Add(logPriceChangeControl);
            logPriceChangeControl.Visible = false;
        }

        public void ShowLogPriceChange(string docId)
        {
            logPriceChangeControl.LoadLogPriceChange(docId, connectionString);
            logPriceChangeControl.BringToFront();
            logPriceChangeControl.Visible = true;
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

            foreach (Control ctrl in pnl_navbar.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.MouseEnter += Button_MouseEnter;
                    btn.MouseLeave += Button_MouseLeave;
                    btn.Click += Button_Click;
                    
                }
            }
            // Initialize controls once
            _ctrDashboard = new ctrDashboard() { Dock = DockStyle.Fill };
            _ctrLogPriceChange = new ctrLogPriceChange() { Dock = DockStyle.Fill };
            _ctrClaimBySellOut = new ctrClaimBySellOut() { Dock = DockStyle.Fill };
            _ctrClaimByInventory = new ctrClaimByInventory() { Dock = DockStyle.Fill };

            // Load the default
            LoadControl(_ctrDashboard);
        }

        private void LoadControl(UserControl ctrl)
        {
            pnl_main.SuspendLayout();
            pnl_main.Controls.Clear();
            pnl_main.Controls.Add(ctrl);
            pnl_main.ResumeLayout();
        }

        private void btn_dashboard_Click(object sender, EventArgs e)
        {
            LoadControl(_ctrDashboard);
        }

        private void btn_lpc_Click(object sender, EventArgs e)
        {
            LoadControl(_ctrLogPriceChange);
        }

        private void btn_claimbysellout_Click(object sender, EventArgs e)
        {
            LoadControl(_ctrClaimBySellOut);
        }

        private void btn_claimbyinventory_Click(object sender, EventArgs e)
        {
            LoadControl(_ctrClaimByInventory);
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
