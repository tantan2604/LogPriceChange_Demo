using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace LogPriceChange0._1
{
    public partial class AdminForm : Form
    {
        // Store the connection string in a private field
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\VisualStudio\LogPriceChange0.1\LogPriceChange0.1\pricematrix.accdb;";
        private string _username;
        private string _idValue;

        public AdminForm(string username)
        {
            InitializeComponent();
            _username = username;
            this.Text = "Admin Dashboard";
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            LoadDataByStatus("ForApproval", dgv_forApproval);
            LoadDataByStatus("Approved", dgv_Approved);
            UpdateStatusCounts();
            if (this.Controls.Find("lbl_adminLog", true).Length > 0)
            {
                Label userLabel = (Label)this.Controls.Find("lbl_adminLog", true)[0];
                userLabel.Text = _username;
            }
        }
        // This method is now safe and properly manages its own connection.
        private void LoadDataByStatus(string docStatus, DataGridView dgv)
        {
            string query = $"SELECT * FROM tbl_logpricechange WHERE DocStatus = '{docStatus}';";

            try
            {
                // Use a 'using' statement to ensure the connection is always closed.
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgv.DataSource = dataTable;
                }

                if (dgv.Columns.Contains("ID"))
                {
                    dgv.Columns["ID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading {docStatus} data: " + ex.Message);
            }
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            wfLoginform loginForm = new wfLoginform();
            loginForm.ShowDialog();
        }

        private void dgv_forApproval_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgv_forApproval.Rows[e.RowIndex].Cells["ID"].Value != null)
            {
                _idValue = dgv_forApproval.Rows[e.RowIndex].Cells["ID"].Value.ToString();
            }
        }

        private void tabCtr_forApproval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtr_forApproval.SelectedIndex == 1)
            {
                btn_approve.Visible = false;
                btn_reject.Visible = false;
                LoadDataByStatus("Approved", dgv_Approved);
            }
            else
            {
                btn_approve.Visible = true;
                btn_reject.Visible = true;
            }
        }

        // This method now uses the class-level connectionString and handles its own resources.
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
                query = $"UPDATE tbl_logpricechange SET DocStatus = 'Rejected', ApprovedBy = '{approvedBy}' WHERE ID = {_idValue};";
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
                            LoadDataByStatus("ForApproval", dgv_forApproval);
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
                UpdateDocumentStatus("Approved", _username, approvedDate);
            }
            UpdateStatusCounts();
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
                UpdateDocumentStatus("Rejected", _username);
            }
            UpdateStatusCounts();
        }

        private void UpdateStatusCounts()
        {
            // Define the query to count all statuses
            string query = "SELECT DocStatus, COUNT(DocStatus) AS StatusCount FROM tbl_logpricechange GROUP BY DocStatus;";

            // Use a 'using' statement for the connection for proper resource management
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand(query, connection);
                    OleDbDataReader reader = command.ExecuteReader();

                    // Reset all labels to zero before updating
                    lblApprovedCount.Text = "0";
                    lblForApproval.Text = "0";
                    lblRejectedCount.Text = "0";
                   

                    // Loop through the query results
                    while (reader.Read())
                    {
                        string status = reader["DocStatus"].ToString();
                        int count = Convert.ToInt32(reader["StatusCount"]);

                        // Update the appropriate label based on the status
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

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating status counts: " + ex.Message);
                }
            }
        }
    }
}