using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LogPriceChange0._1
{
    public partial class AdminForm : Form
    {
        // Store the connection string in a private field
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Desktop\CameraHaus\LogPriceChange_Demo\pricematrix.accdb;";
        private string _username;
        private string _idValue;
        private BindingSource bindingSource = new BindingSource();

        #region **************************************************** Methods **********************************************************************************
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
        private string GetFullName(string username)
        {
            string fullName = string.Empty;
            string query = "SELECT Lastname, Firstname FROM tbl_employee WHERE Username = ?";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                command.Parameters.Add("?", OleDbType.VarWChar, 255).Value = username;

                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
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
            return fullName;
        }
        private void LoadDataAndStatus()
        {
            using (var connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Load counts for labels
                    string statusQuery = @"
                        SELECT DocStatus, COUNT(*) AS StatusCount
                        FROM tbl_logpricechange
                        GROUP BY DocStatus";

                    using (var statusCmd = new OleDbCommand(statusQuery, connection))
                    using (var reader = statusCmd.ExecuteReader())
                    {
                        lblApprovedCount.Text = "0";
                        lblForApproval.Text = "0";
                        lblRejectedCount.Text = "0";

                        while (reader.Read())
                        {
                            string status = reader["DocStatus"]?.ToString() ?? "";
                            int count = Convert.ToInt32(reader["StatusCount"]);

                            switch (status)
                            {
                                case "ForApproval":
                                    lblForApproval.Text = count.ToString();
                                    break;
                                case "Approved":
                                    lblApprovedCount.Text = count.ToString();
                                    break;
                                case "Rejected":
                                    lblRejectedCount.Text = count.ToString();
                                    break;
                            }
                        }
                    }

                    // Load all data into one DataGridView
                    string dataQuery = @"
                        SELECT *
                        FROM tbl_logpricechange
                        ORDER BY CreatedDate ASC";

                    using (var dataAdapter = new OleDbDataAdapter(dataQuery, connection))
                    {
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        bindingSource.DataSource = dataTable;
                        dgv_main.DataSource = bindingSource; // one grid only
                        dgv_main.ReadOnly = false;
                        tabCtr_forApproval.SelectedIndexChanged += tabCtr_forApproval_SelectedIndexChanged;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading data: " + ex.Message,
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void UpdateChangedRows()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                if (bindingSource.DataSource is DataTable table)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        if (dr.RowState == DataRowState.Modified)
                        {
                            string pkColumn = dgv_main.Columns[0].Name;

                            // Build SET clause dynamically, skipping DocStatus
                            List<string> setClauses = new List<string>();
                            for (int i = 1; i < dgv_main.Columns.Count; i++)
                            {
                                string colName = dgv_main.Columns[i].Name;
                                if (!colName.Equals("DocStatus", StringComparison.OrdinalIgnoreCase))
                                {
                                    setClauses.Add($"{colName} = ?");
                                }
                            }

                            // Append DocStatus manually at the end
                            setClauses.Add("DocStatus = ?");

                            string query = $"UPDATE tbl_logpricechange SET {string.Join(", ", setClauses)} WHERE {pkColumn} = ?";

                            using (OleDbCommand cmd = new OleDbCommand(query, conn))
                            {
                                // Add parameters for all columns except PK and DocStatus
                                for (int i = 1; i < dgv_main.Columns.Count; i++)
                                {
                                    string colName = dgv_main.Columns[i].Name;
                                    if (!colName.Equals("DocStatus", StringComparison.OrdinalIgnoreCase))
                                    {
                                        cmd.Parameters.AddWithValue("?", dr[colName] ?? DBNull.Value);
                                    }
                                }

                                // Add DocStatus param (fixed)
                                cmd.Parameters.AddWithValue("?", "ForApproval");

                                // Add PK param
                                cmd.Parameters.AddWithValue("?", dr[pkColumn] ?? DBNull.Value);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            MessageBox.Show("Only changed rows updated, DocStatus set to 'ForApproval'.");
        }
        private void ApplyTabFilter()
        {
            if (tabCtr_forApproval.SelectedTab == null) return;

            string filter = "";

            switch (tabCtr_forApproval.SelectedTab.Name)
            {
                case "tabApproved":
                    filter = "[DocStatus] = 'Approved'";
                    break;
               
                case "tabForApproval":
                default:
                    filter = "[DocStatus] = 'ForApproval'";
                    break;
            }

            bindingSource.Filter = filter;

            // Show/hide approve/reject buttons only for ForApproval tab
            bool isApprovalTab = tabCtr_forApproval.SelectedTab.Name == "tabForApproval";
            btn_approve.Visible = isApprovalTab;
            btn_reject.Visible = isApprovalTab;
            btnUpdate.Visible = isApprovalTab;
        }
        private void UpdateDocumentStatus(string newDocStatus, string approvedBy, string approvedDate = null)
        {
            if (string.IsNullOrEmpty(_idValue))
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            string query = "";
            if (newDocStatus == "Rejected")
            {
                query = $"UPDATE tbl_logpricechange SET DocStatus = 'Rejected' WHERE ID = {_idValue};";
            }
            else if (newDocStatus == "Approved")
            {
                query = $"UPDATE tbl_logpricechange SET DocStatus = 'Approved', ApprovedBy = '{approvedBy}', ApprovedDate = '{approvedDate}' WHERE ID = {_idValue};";
            }

            try
            {
                // Use a 'using' statement for the connection and command.
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Record has been {newDocStatus.ToLower()} successfully.");
                            //LoadDataByStatus("ForApproval", dgv_forApproval);
                        }
                        else
                        {
                            MessageBox.Show("No record was updated. Please check the ID.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {newDocStatus.ToLower()} record: " + ex.Message);
            }
        }

        #endregion ****************************************************End of Methods **********************************************************************************
        public AdminForm(string username)
        {
            InitializeComponent();
            _username = username;
            this.Text = "Admin Dashboard";
            LoadDataAndStatus();
           
        }
        private void AdminForm_Load(object sender, EventArgs e)
        {
            ApplyTabFilter();
        }
        private void btn_logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            wfLoginform loginForm = new wfLoginform();
            loginForm.Show();
            UpdateIsLoginTofalse();
        }
        private void btn_approve_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_idValue))
            {
                MessageBox.Show("Please select a record to approve.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to Approve this request?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string approvedDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                UpdateDocumentStatus("Approved", GetFullName(_username), approvedDate);
            }
            LoadDataAndStatus();
        }
        private void btn_reject_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_idValue))
            {
                MessageBox.Show("Please select a record to reject.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to reject this?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                UpdateDocumentStatus("Rejected", GetFullName(_username));
            }
            LoadDataAndStatus();
        }
        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateIsLoginTofalse();

            // Now, let the application close gracefully.
            Application.Exit();
        }
        private void dgv_main_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgv_main.Rows[e.RowIndex].Cells["ID"].Value != null)
            {
                _idValue = dgv_main.Rows[e.RowIndex].Cells["ID"].Value.ToString();
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateChangedRows();
            LoadDataAndStatus();
        }
        private void tabCtr_forApproval_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_main.Parent = tabCtr_forApproval.SelectedTab;
            dgv_main.Dock = DockStyle.Fill;
            ApplyTabFilter();
        }
        private void dgv_main_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgv_main.Rows[e.RowIndex].Cells["ID"].Value != null)
            {
                _idValue = dgv_main.Rows[e.RowIndex].Cells["ID"].Value.ToString();
            }
        }
    }
}