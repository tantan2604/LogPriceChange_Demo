using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace LogPriceChange0._1
{
    public partial class ctrLogPriceChange : UserControl
    {
       
         string tableName = "MyTable";
         string primaryKeyColumn = "ID"; // Assumes an AutoNumber primary key
         //OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\VisualStudio\LogPriceChange0.1\LogPriceChange0.1\pricematrix.accdb;");
        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TAN\Source\Repos\LogPriceChange_Demo\pricematrix.accdb;");
        DataTable dataTable; // Stores the data displayed in the DataGridView
         OleDbDataAdapter dataAdapter; // Manages data transfer between DB and DataTable

        public ctrLogPriceChange()
        {
            
            InitializeComponent();
            lpc_dtp_enddate.CustomFormat = " ";
        }
        public void LoadData()
        {
            try
            {
                
                connection.Open(); // Open the database connection

                string query = @"SELECT ID, PROD_C, PROD_N, FREE, PLFOB, NWF, NWFR, PC_PF, PC_PFL, PC_RP, PC_PA, PC_PLSRP, PC_LSRP, PC_PPA2LP, PC_LP, PC_PPA2WA, PC_WA, PC_PPA2WB, PC_WB, PC_PPA2WC, PC_WC, PC_PPA2LC, PC_LC, PC_PPA2PG, PC_PG, PC_PPA2PH, PC_PH, PC_PPA2PB, PC_PB, PC_PPA2PD, PC_PD, LPP_AMT, LPP_REF, PC_PPA2PC, PC_PC FROM tbl_billPTMP";
                dataAdapter = new OleDbDataAdapter(query, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable); // Fill the DataTable with data from the database

                lpc_dgv_searchbycode.DataSource = dataTable; // Bind the DataTable to the DataGridView

                // Make the primary key column read-only if it's auto-incrementing in Access.
                // This prevents users from manually changing the ID.
                if (lpc_dgv_searchbycode.Columns.Contains(primaryKeyColumn))
                {
                    lpc_dgv_searchbycode.Columns[primaryKeyColumn].ReadOnly = true;
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                // Display an error message if data loading fails
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the connection is closed even if an error occurs
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void ctrLogPriceChange_Load(object sender, EventArgs e)
        {

        }

        private void lpc_tb_searchbycode_TextChanged(object sender, EventArgs e)
        {
            // Check if the textbox is empty
            if (string.IsNullOrEmpty(lpc_tb_searchbycode.Text))
            {
                lpc_dgv_searchbycode.Visible = false;
                return;
            }

            lpc_dgv_searchbycode.Visible = true;



            try
            {
                connection.Open();
                string dbsearch = @"SELECT PROD_C, PROD_N, FREE, PLFOB, NWF, NWFR, PC_PF, PC_PFL, PC_RP, PC_PA, PC_PLSRP, PC_LSRP, PC_PPA2LP, PC_LP, PC_PPA2WA, PC_WA, PC_PPA2WB, PC_WB, PC_PPA2WC, PC_WC, PC_PPA2LC, PC_LC, PC_PPA2PG, PC_PG, PC_PPA2PH, PC_PH, PC_PPA2PB, PC_PB, PC_PPA2PD, PC_PD, LPP_AMT, LPP_REF, PC_PPA2PC, PC_PC FROM tbl_billPTMP WHERE PROD_C LIKE @searchText OR PROD_CN LIKE @searchText";
                using (OleDbCommand cmd = new OleDbCommand(dbsearch, connection))
                {
                    cmd.Parameters.AddWithValue("@searchText", "%" + this.lpc_tb_searchbycode.Text + "%");
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    lpc_dgv_searchbycode.DataSource = dt;
                    //foreach (DataGridViewColumn column in lpc_dgv_searchbycode.Columns)
                    //{
                    //    column.Visible = (column.HeaderText == "PROD_C");
                    //}

                    int lastColumnIndex = lpc_dgv_searchbycode.Columns.Count - 1;
                    foreach (DataGridViewColumn column in lpc_dgv_searchbycode.Columns)
                    {
                        column.AutoSizeMode = (column.Index == lastColumnIndex) ?
                            DataGridViewAutoSizeColumnMode.Fill :
                            DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            connection.Close();
        }

        private void lpc_rbtn_permanent_CheckedChanged(object sender, EventArgs e)
        {

            if (lpc_rbtn_permanent.Checked)
            {
                lpc_dtp_enddate.Enabled = false;
                lpc_dtp_enddate.CustomFormat = "";

            }
            else if (lpc_rbtn_temporary.Checked)
            {
                lpc_dtp_enddate.Enabled = true;
                lpc_dtp_enddate.CustomFormat = "MM / dd / yyyy hh: mm: ss tt";
                lpc_dtp_enddate.Format = DateTimePickerFormat.Custom;
            }

        }

        private void btnlpcsubmit_Click(object sender, EventArgs e)

        {
            InsertOrUpdateToSupp();
        }

        private void lpc_dgv_searchbycode_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header click
            if (e.RowIndex < 0) return;

            DataGridViewRow selectedRow = lpc_dgv_searchbycode.Rows[e.RowIndex];

            var prodCode = selectedRow.Cells["PROD_C"].Value;
            var productName = selectedRow.Cells["PROD_N"].Value;

            // 🧹 Clear all rows first
            lpc_dgv_dbvalue.Rows.Clear();

            // 📝 Ensure exactly 3 rows
            for (int i = 0; i < 3; i++)
            {
                lpc_dgv_dbvalue.Rows.Add();
            }

            // 🚀 Row 0 → full data
            for (int col = 0; col < selectedRow.Cells.Count; col++)
            {
                if (col >= lpc_dgv_dbvalue.Columns.Count) break; // safety
                lpc_dgv_dbvalue.Rows[0].Cells[col].Value = selectedRow.Cells[col].Value;
            }

            // 🚀 Row 1 & 2 → ProductName in column 1
            lpc_dgv_dbvalue.Rows[1].Cells[1].Value = productName;
            lpc_dgv_dbvalue.Rows[2].Cells[1].Value = productName;
            lpc_dgv_dbvalue.Rows[1].Cells[0].Value = prodCode;
            lpc_dgv_dbvalue.Rows[2].Cells[0].Value = prodCode;

            // 🔒 Make column 1 readonly in all rows
            foreach (DataGridViewRow row in lpc_dgv_dbvalue.Rows)
            {
                if (!row.IsNewRow)
                {
                    //row.Cells[1].ReadOnly = true;
                    //row.Cells[0].ReadOnly = true;
                }
            }
           
        }

        private void lpc_btnsup_Click(object sender, DataGridViewCellEventArgs e)
        {
            
            // check connection state
            try
            {
                connection.Open();
                MessageBox.Show("Connection to database established successfully.");


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                
                
                    connection.Close();
                
            }
        }

        // main entry point
        void InsertOrUpdateToSupp()
        {
            try
            {
                connection.Open();

                // Define which columns are numeric by index
                HashSet<int> numericColumns = new HashSet<int>
        {
            3,4,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,32,33,34,35
        };

                // Define your database column names in the correct order (match DB table)
                string[] columnNames =
                {
            "PROD_C", "PROD_N", "FREE_SUP", "PLFOB_SUP", "NWF_SUP", "NWFR_SUP",
            "PC_PF_SUP", "PC_PFL_SUP", "PC_RP_SUP", "PC_PA_SUP", "PC_PLSRP_SUP", "PC_LSRP_SUP",
            "PC_PPA2LP_SUP", "PC_LP_SUP", "PC_PPA2WA_SUP", "PC_WA_SUP", "PC_PPA2WB_SUP", "PC_WB_SUP",
            "PC_PPA2WC_SUP", "PC_WC_SUP", "PC_PPA2LC_SUP", "PC_LC_SUP", "PC_PPA2PG_SUP", "PC_PG_SUP",
            "PC_PPA2PH_SUP", "PC_PH_SUP", "PC_PPA2PB_SUP", "PC_PB_SUP", "PC_PPA2PD_SUP", "PC_PD_SUP",
            "LPP_AMT_SUP", "LPP_REF_SUP", "PC_PPA2PC_SUP", "PC_PC_SUP", "Claim1", "Claim2",
            "ClaimK1", "ClaimK2", "Remarks1", "Remarks2"
        };

                foreach (DataGridViewRow row in lpc_dgv_dbvalue.Rows)
                {
                    if (row.IsNewRow) continue;

                    List<object> values = new List<object>();

                    for (int i = 0; i < columnNames.Length; i++)
                    {
                        var cellValue = row.Cells[i].Value;

                        if (numericColumns.Contains(i))
                            values.Add(GetDoubleOrDefault(cellValue));
                        else
                            values.Add(GetStringOrDefault(cellValue));
                    }

                    string PROD_C = values[0].ToString();
                    string FREE_SUP = values[2].ToString();

                    if (string.IsNullOrWhiteSpace(FREE_SUP))
                    {
                        MessageBox.Show("Product Name cannot be empty for a row.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    string checkQuery = "SELECT COUNT(*) FROM tbl_logpricechange WHERE PROD_C = ?";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("?", PROD_C);

                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (exists > 0)
                            UpdateRow(PROD_C, columnNames, values);
                        else
                            InsertRow(columnNames, values);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }


        void UpdateRow(string prodC, string[] cols, List<object> vals)
        {
            string setClause = string.Join(", ", cols.Skip(1).Select(c => $"{c}=?")); // skip PROD_C for SET
            string updateQuery = $"UPDATE tbl_logpricechange SET {setClause} WHERE PROD_C = ?";
            using (OleDbCommand cmd = new OleDbCommand(updateQuery, connection))
            {
                // Add all except PROD_C for SET
                for (int i = 1; i < vals.Count; i++)
                    cmd.Parameters.AddWithValue("?", vals[i]);

                cmd.Parameters.AddWithValue("?", prodC); // WHERE

                cmd.ExecuteNonQuery();
                MessageBox.Show($"Updated: {prodC}");
            }
        }

        void InsertRow(string[] cols, List<object> vals)
        {
            string colList = string.Join(", ", cols);
            string placeholders = string.Join(", ", cols.Select(c => "?"));
            string insertQuery = $"INSERT INTO tbl_logpricechange ({colList}) VALUES ({placeholders})";

            using (OleDbCommand cmd = new OleDbCommand(insertQuery, connection))
            {
                foreach (var val in vals)
                    cmd.Parameters.AddWithValue("?", val);

                cmd.ExecuteNonQuery();
                MessageBox.Show($"Inserted: {vals[0]}");
            }
        }

        double GetDoubleOrDefault(object value, double defaultValue = 0.00)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return defaultValue;

            if (double.TryParse(value.ToString(), out double result))
                return result;

            return defaultValue;
        }

        string GetStringOrDefault(object value, string defaultValue = "") =>
            value?.ToString()?.Trim() ?? defaultValue;

    }
}
