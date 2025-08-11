using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LogPriceChange0._1
{
    public partial class ctrDashboard : UserControl
    {
       
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\SharedDB\pricematrix.accdb;";

     
        private OleDbConnection connection;

 
        private BindingSource bindingSource = new BindingSource();
       
        
        private string loggedUsername = UserSession.Username;
        

        #region ************************** METHODS **************************

        private string GetFullName(string username)
        {
            string fullName = string.Empty;
            string query = "SELECT Lastname, Firstname FROM tbl_employee WHERE Username = ?";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
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
            }
            return fullName;
        }

        private void LoadDataAndStatus()
        {
            string createdByFullName = GetFullName(loggedUsername);

            using (var connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // 1️⃣ Get status counts
                    string statusQuery = @"
                SELECT DocStatus, COUNT(DocStatus) AS StatusCount
                FROM tbl_logpricechange
                WHERE CreatedBy = ?
                GROUP BY DocStatus";

                    

                    using (var statusCmd = new OleDbCommand(statusQuery, connection))
                    {
                        statusCmd.Parameters.Add("?", OleDbType.VarWChar, 255).Value = createdByFullName;

                        using (var reader = statusCmd.ExecuteReader())
                        {
                            // Reset labels
                            lblApprovedCount.Text = "0";
                            lblForApproval.Text = "0";
                            lblRejectedCount.Text = "0";
                            lblDraftCount.Text = "0";

                            while (reader.Read())
                            {
                                string status = reader["DocStatus"].ToString();
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
                        }
                    }

                    // 2️⃣ Load DataGridView data
                  
                    string dataQuery = @"
                SELECT *
                FROM tbl_logpricechange
                WHERE CreatedBy = ?
                ORDER BY CreatedDate ASC";
               

                    using (var dataAdapter = new OleDbDataAdapter(dataQuery, connection))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("?", createdByFullName);
                        var commandBuilder = new OleDbCommandBuilder(dataAdapter);

                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        bindingSource.DataSource = dataTable;
                        dgvDocStat.DataSource = bindingSource;
                        dgvDocStat.ReadOnly = true;
                        //if (statusComboBox.SelectedItem == "Rejected" || statusComboBox.SelectedItem == "Draft")
                        //{
                        //    dgvDocStat.ReadOnly = true;
                        //}
                        //else
                        //{
                        //    dgvDocStat.ReadOnly = false;
                        //}
                        

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
                            string pkColumn = dgvDocStat.Columns[0].Name;

                            // Build SET clause dynamically, skipping DocStatus
                            List<string> setClauses = new List<string>();
                            for (int i = 1; i < dgvDocStat.Columns.Count; i++)
                            {
                                string colName = dgvDocStat.Columns[i].Name;
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
                                for (int i = 1; i < dgvDocStat.Columns.Count; i++)
                                {
                                    string colName = dgvDocStat.Columns[i].Name;
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
       

        #endregion

        public ctrDashboard()
        {
            InitializeComponent();
            connection = new OleDbConnection(connectionString);
            LoadDataAndStatus();
        }

        private void ctrDashboard_Load_1(object sender, EventArgs e)
        {

        }

        private void statusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDocStatus = statusComboBox.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(statusComboBox.Text))
            {
                
                bindingSource.RemoveFilter();
                return;
            }

            lblDocStat.Visible = true;
            dgvDocStat.Visible = true;

            if (selectedDocStatus =="Rejected" || selectedDocStatus == "Draft")
            {
                btnUpdate.Visible = true;
            }
            else
            {
                btnUpdate.Visible = false;
            }


            bindingSource.Filter = $"DocStatus = '{selectedDocStatus}'";

            switch (selectedDocStatus)
            {
                case "ForApproval":
                    lblDocStat.Text = "For Approval";
                    
                    break;
                case "Approved":
                    lblDocStat.Text = "Approved";
                    dgvDocStat.ReadOnly = true;
                    break;
                case "Rejected":
                    lblDocStat.Text = "Rejected";
                   dgvDocStat.ReadOnly = false;
                    break;
                case "Draft":
                    lblDocStat.Text = "Draft";
                   dgvDocStat.ReadOnly = false;
                    break;
                default:
                    lblDocStat.Text = "";
                    dgvDocStat.ReadOnly = true;
                    break;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)

        {
            try
            {
                UpdateChangedRows();
                LoadDataAndStatus();



                MessageBox.Show("Data updated successfully.", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
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
            if (rowIndex < 0 || rowIndex >= dgvDocStat.Rows.Count)
                return;

            var row = dgvDocStat.Rows[rowIndex];

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

        private void dgvDocStat_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Make sure the cell value is committed before reading it
                dgvDocStat.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgvDocStat.EndEdit();

                UpdateDependentValues(e.RowIndex, e.ColumnIndex);
            }
        }

       
        #endregion
    }
}
