using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
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

        public void LoadDataAndStatus()
        {
            string createdByFullName = GetFullName(loggedUsername);

            using (var connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string dataQuery = @"
                SELECT *
                FROM tbl_logpricechange
                WHERE CreatedBy = ?
                ORDER BY DocID, CreatedDate ASC";

                    using (var dataAdapter = new OleDbDataAdapter(dataQuery, connection))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("?", createdByFullName);
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Add temporary column for sorting
                        if (!dataTable.Columns.Contains("SortOrder"))
                            dataTable.Columns.Add("SortOrder", typeof(int));

                        var grouped = dataTable.AsEnumerable()
                            .GroupBy(row => row.Field<string>("DocID"))
                            .SelectMany(group =>
                            {
                                // Assign sort order based on row type (normal, _sup, _promo)
                                foreach (var row in group)
                                {
                                    int sortOrder = GetRowType(row); // 0 = normal, 1 = sup, 2 = promo
                                    row["SortOrder"] = sortOrder;
                                }

                                var sortedGroup = group.OrderBy(r => Convert.ToInt32(r["SortOrder"])).ToList();

                                bool first = true;
                                return sortedGroup.Select(row =>
                                {
                                    var newRow = dataTable.NewRow();

                                    foreach (DataColumn column in dataTable.Columns)
                                    {
                                        if (column.ColumnName == "DocID")
                                        {
                                            newRow[column] = first ? row[column] : DBNull.Value;
                                        }
                                        else
                                        {
                                            newRow[column] = row[column];
                                        }
                                    }

                                    first = false;
                                    return newRow;
                                });
                            }).CopyToDataTable();

                        // Remove SortOrder column if present
                        if (grouped.Columns.Contains("SortOrder"))
                            grouped.Columns.Remove("SortOrder");

                        bindingSource.DataSource = grouped;
                        dgvDocStat.DataSource = bindingSource;
                        dgvDocStat.ReadOnly = true;

                        dgvDocStat.RowPrePaint -= dgvDocStat_RowPrePaint;
                        dgvDocStat.RowPrePaint += dgvDocStat_RowPrePaint;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading data: " + ex.Message,
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Function to determine the type of row based on column suffixes
        private int GetRowType(DataRow row)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                var colName = column.ColumnName.ToLower();
                if (colName.EndsWith("_sup") && !string.IsNullOrEmpty(row[column]?.ToString()))
                    return 1; // sup row
                if (colName.EndsWith("_promo") && !string.IsNullOrEmpty(row[column]?.ToString()))
                    return 2; // promo row
            }
            return 0; // default: normal
        }

        // Color only first row (with DocID) green
        private void dgvDocStat_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var dgv = sender as DataGridView;
            var row = dgv.Rows[e.RowIndex];

            var docIdValue = row.Cells["DocID"].Value;
            if (docIdValue != DBNull.Value && docIdValue != null && !string.IsNullOrEmpty(docIdValue.ToString()))
            {
                row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFECA1"); 
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
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
        private DataTable TransformToVerticalView(DataRow sourceRow)
        {
            // 🧱 List of base field names (no _SUP or _USRINT suffixes)
            var baseFields = new List<string>
    {
        "FREE", "PLFOB", "NWF", "NWFR",
        "PC_PF", "PC_PFL", "PC_RP", "PC_PA", "PC_PLSRP", "PC_LSRP",
        "PC_PPA2LP", "PC_LP", "PC_PPA2WA", "PC_WA", "PC_PPA2WB", "PC_WB",
        "PC_PPA2WC", "PC_WC", "PC_PPA2LC", "PC_LC", "PC_PPA2PG", "PC_PG",
        "PC_PPA2PH", "PC_PH", "PC_PPA2PB", "PC_PB", "PC_PPA2PD", "PC_PD",
        "LPP_AMT", "LPP_REF", "PC_PPA2PC", "PC_PC"
    };

            // 🧱 Setup vertical table
            DataTable verticalTable = new DataTable();
            verticalTable.Columns.Add("Version"); // Normal, Supplier, Promo

            foreach (var field in baseFields)
            {
                verticalTable.Columns.Add(field);
            }

            // 🧱 Build rows: Normal, Supplier, Promo
            string[] suffixes = { "", "_SUP", "_USRINT" };
            string[] labels = { "Normal", "Supplier", "Promo" };

            for (int i = 0; i < suffixes.Length; i++)
            {
                var row = verticalTable.NewRow();
                row["Version"] = labels[i];

                foreach (var field in baseFields)
                {
                    string fullFieldName = field + suffixes[i];

                    if (sourceRow.Table.Columns.Contains(fullFieldName))
                        row[field] = sourceRow[fullFieldName];
                    else
                        row[field] = DBNull.Value;
                }

                verticalTable.Rows.Add(row);
            }

            return verticalTable;
        }

        #endregion

        public ctrDashboard()
        {
            InitializeComponent();
            connection = new OleDbConnection(connectionString);
        }
        private void ctrDashboard_Load_1(object sender, EventArgs e)
        {
            LoadDataAndStatus();
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

        private void dgvDocStat_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0)
            //{
            //    var rowView = dgvDocStat.Rows[e.RowIndex].DataBoundItem as DataRowView;
            //    if (rowView != null)
            //    {
            //        var verticalTable = TransformToVerticalView(rowView.Row);
            //        .DataSource = verticalTable;
            //    }
            //}
        }
    }
}
