using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text; // Added for StringBuilder
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LogPriceChange0._1
{
    public partial class ctrDashboard : UserControl
    {
        
        private const string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\SharedDB\pricematrix.accdb;";
        private string loggedUsername = UserSession.Username;
        private string GetFullName(string username)
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

        public ctrDashboard()
        {
            InitializeComponent();
            UpdateStatusCounts();
        }
        private void ctrDashboard_Load_1(object sender, EventArgs e)
        {
            // Populate the combobox with our filtering options
            // Add a blank option for no filter.
            statusComboBox.Items.Add("");
            statusComboBox.Items.Add("Rejected");
            statusComboBox.Items.Add("Draft");
            statusComboBox.Items.Add("ForApproval");
            statusComboBox.Items.Add("Approved");

            statusComboBox.SelectedIndex = 0;

            // Initially hide the DataGridView until a filter is selected
            dgvDocStat.Visible = false;
        }
        private void UpdateStatusCounts()
        {
            // This query is now correct. It counts the documents for each status,
            // filters by the username, and groups the results.
            string query = "SELECT DocStatus, COUNT(DocStatus) AS StatusCount FROM tbl_logpricechange WHERE CreatedBy = ? GROUP BY DocStatus;";

            using (var connection = new OleDbConnection(connectionString))
            {
                using (var command = new OleDbCommand(query, connection))
                {
                    // Use a parameter for the username to prevent SQL injection.
                    command.Parameters.Add("?", GetFullName(loggedUsername));

                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();

                        // Reset all labels to '0' before updating
                        lblApprovedCount.Text = "0";
                        lblForApproval.Text = "0";
                        lblRejectedCount.Text = "0";
                        lblDraftCount.Text = "0";

                        while (reader.Read())
                        {
                            string status = reader["DocStatus"].ToString();
                            // Safely convert the count to an integer.
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
                                case "Draft":
                                    lblDraftCount.Text = count.ToString();
                                    break;
                            }
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while updating status counts: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void LoadDataGrid(string docStatus)
        {
            // The query has been updated to use a parameter for the username and the docStatus.
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM tbl_logpricechange WHERE CreatedBy = ?");

            // Add the DocStatus filter only if it's not a blank string.
            if (!string.IsNullOrEmpty(docStatus))
            {
                queryBuilder.Append(" AND DocStatus = ?");
            }

            string query = queryBuilder.ToString();

            using (var connection = new OleDbConnection(connectionString))
            {
                using (var command = new OleDbCommand(query, connection))
                {
                    // Add the loggedUser parameter.
                    command.Parameters.Add("?", GetFullName(loggedUsername));

                    // Add the docStatus parameter only if needed.
                    if (!string.IsNullOrEmpty(docStatus))
                    {
                        command.Parameters.Add("?", OleDbType.VarWChar, 255).Value = docStatus;
                    }

                    try
                    {
                        connection.Open();
                        var adapter = new OleDbDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvDocStat.DataSource = dataTable;
                        dgvDocStat.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while loading data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgvDocStat.Visible = false;
                    }
                }
            }
        }
        private void statusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Pass the selected status to the LoadDataGrid method
            string selectedStatus = statusComboBox.SelectedItem.ToString();
            if (string.IsNullOrEmpty(selectedStatus))
            {
                // If no status is selected, clear the DataGridView
                dgvDocStat.DataSource = null;
                dgvDocStat.Visible = false;
                lblDocStat.Text = "";
                return;
            }
            LoadDataGrid(selectedStatus);
            lblDocStat.Text = selectedStatus;
        }
    }
}
