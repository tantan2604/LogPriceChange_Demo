using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
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
        private string _docIDValue;
        Label activeLabel = null;
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
                        WHERE NOT (DocStatus = 'Draft')
                        ORDER BY CreatedDate ASC";

                    using (var dataAdapter = new OleDbDataAdapter(dataQuery, connection))
                    {
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        bindingSource.DataSource = dataTable;
                        dgv_main.DataSource = bindingSource; // one grid only
                        dgv_main.ReadOnly = false;

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


        }
        private void UpdateDocumentStatus(string newDocStatus, string approvedBy = null, string approvedDate = null)
        {
            if (string.IsNullOrEmpty(_docIDValue))
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            string query = "";
            var parameters = new List<OleDbParameter>();

            if (newDocStatus == "Rejected")
            {
                query = "UPDATE tbl_logpricechange SET DocStatus = ? WHERE DocID = ?";
                parameters.Add(new OleDbParameter("DocStatus", "Rejected"));
                parameters.Add(new OleDbParameter("DocID", _docIDValue));
            }
            else if (newDocStatus == "Approved")
            {
                query = "UPDATE tbl_logpricechange SET DocStatus = ?, ApprovedBy = ?, ApprovedDate = ? WHERE DocID = ?";
                parameters.Add(new OleDbParameter("DocStatus", "Approved"));
                parameters.Add(new OleDbParameter("ApprovedBy", approvedBy ?? (object)DBNull.Value));
                parameters.Add(new OleDbParameter("ApprovedDate", approvedDate ?? (object)DBNull.Value));
                parameters.Add(new OleDbParameter("DocID", _docIDValue));
            }
            else
            {
                MessageBox.Show("Invalid status provided.");
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters.ToArray());

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Record has been {newDocStatus.ToLower()} successfully.");
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
            lbl_adminLog.Text = GetFullName(_username);
            LoadDataAndStatus();

        }
        private void AdminForm_Load(object sender, EventArgs e)
        {


            lbl_forapproval.Click += LabelFilter_Click;
            lbl_rejected.Click += LabelFilter_Click;
            lbl_approved.Click += LabelFilter_Click;
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
            if (string.IsNullOrEmpty(_docIDValue))
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
            UpdateChangedRows();
        }
        private void btn_reject_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_docIDValue))
            {
                MessageBox.Show("Please select a record to reject.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to reject this?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                UpdateDocumentStatus("Rejected");
            }
            LoadDataAndStatus();
        }
        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateIsLoginTofalse();

            // Now, let the application close gracefully.
            Application.Exit();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateChangedRows();
            LoadDataAndStatus();
        }

        #region ************************** DataGridView Cell Edit Logic **************************
        public class ColumnMapping
        {
            public int BaseIndex { get; set; }
            public int ValueIndex { get; set; }
            public int RateIndex { get; set; }
        }

        private readonly List<ColumnMapping> columnMappings = new List<ColumnMapping>
        {
            //supplier column mappings
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  43  ,  RateIndex =  40  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  49  ,  RateIndex =  46  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  55  ,  RateIndex =  52  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  61  ,  RateIndex =  58  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  67  ,  RateIndex =  64  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  73  ,  RateIndex =  70  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  79  ,  RateIndex =  76  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  85  ,  RateIndex =  82  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  91  ,  RateIndex =  88  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  97  ,  RateIndex =  94  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  109 ,  RateIndex =  106 },
            //promo column mappings
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  44  ,  RateIndex =  41  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  50  ,  RateIndex =  47  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  56  ,  RateIndex =  53  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  62  ,  RateIndex =  59  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  68  ,  RateIndex =  65  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  74  ,  RateIndex =  71  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  80  ,  RateIndex =  77  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  86  ,  RateIndex =  83  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  92  ,  RateIndex =  89  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  98  ,  RateIndex =  95  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  110 ,  RateIndex =  107 },




        };

        private void UpdateDependentValues(int rowIndex, int editedColumnIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgv_main.Rows.Count)
                return;

            var row = dgv_main.Rows[rowIndex];

            foreach (var mapping in columnMappings)
            {
                if (editedColumnIndex != mapping.ValueIndex && editedColumnIndex != mapping.RateIndex)
                    continue;

                // Get Base value
                if (!decimal.TryParse(row.Cells[mapping.BaseIndex]?.Value?.ToString(), out decimal baseValue) || baseValue == 0)
                    continue;

                if (editedColumnIndex == mapping.ValueIndex &&
                    decimal.TryParse(row.Cells[mapping.ValueIndex]?.Value?.ToString(), out decimal val))
                {
                    // Calculate rate
                    decimal newRate = Math.Round((val / baseValue) * 100, 2);

                    // If bound to a DataTable, update DataRow directly
                    if (row.DataBoundItem is DataRowView drv)
                        drv[mapping.RateIndex] = newRate;
                    else
                        row.Cells[mapping.RateIndex].Value = newRate;
                }
                else if (editedColumnIndex == mapping.RateIndex &&
                         decimal.TryParse(row.Cells[mapping.RateIndex]?.Value?.ToString(), out decimal rate))
                {
                    // Calculate value
                    decimal newValue = Math.Round((baseValue * rate) / 100, 2);

                    if (row.DataBoundItem is DataRowView drv)
                        drv[mapping.ValueIndex] = newValue;
                    else
                        row.Cells[mapping.ValueIndex].Value = newValue;
                }
            }
        }

        private void dgv_main_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Make sure the cell value is committed before reading it
                dgv_main.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgv_main.EndEdit();

                UpdateDependentValues(e.RowIndex, e.ColumnIndex);
            }
        }

        private void dgv_main_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgv_main.Rows[e.RowIndex].Cells["ID"].Value != null)
            {
                _idValue = dgv_main.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                _docIDValue = dgv_main.Rows[e.RowIndex].Cells["DocID"].Value.ToString();
                
            }
        }
        #endregion

        private void SetupLabel(Label lbl, string text, Color color)
        {
            lbl.Text = text;
            lbl.ForeColor = color;
            lbl.Cursor = Cursors.Hand;
            lbl.Font = new Font(lbl.Font.FontFamily, lbl.Font.Size, FontStyle.Underline);
        }

        private void LabelFilter_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            string status = clickedLabel.Text;

            // Filter data
            bindingSource.Filter = $"DocStatus = '{status}'";

            // Update label styles
            SetActiveLabel(clickedLabel);

            if (string.Equals(status, "ForApproval", StringComparison.OrdinalIgnoreCase))
            {
                btnUpdate.Visible = true;
                btn_approve.Visible = true;
                btn_reject.Visible = true;
            }
            else
            {
                btnUpdate.Visible = false;
                btn_approve.Visible = false;
                btn_reject.Visible = false;
            }
        }

        private void SetActiveLabel(Label selected)
        {
            // Reset previous label
            if (activeLabel != null)
            {
                activeLabel.Font = new Font(activeLabel.Font.FontFamily, activeLabel.Font.Size);
                activeLabel.ForeColor = GetDefaultColor(activeLabel.Text);
            }

            // Set new active label
            selected.Font = new Font(selected.Font.FontFamily, selected.Font.Size, FontStyle.Bold | FontStyle.Underline);
            selected.ForeColor = Color.Black;
            activeLabel = selected;
        }

        private Color GetDefaultColor(string status)
        {
            switch (status)
            {
                case "Rejected": return Color.Black;
                case "ForApproval": return Color.Black;
                case "Approved": return Color.Black;
                default: return Color.Black;
            }
        }


    }
}