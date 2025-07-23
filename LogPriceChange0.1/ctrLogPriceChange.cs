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
        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\VisualStudio\LogPriceChange0.1\LogPriceChange0.1\pricematrix.accdb;");
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
            //InsertPromo();
            InsertOrUpdateRow(0);// For the first row
            InsertOrUpdateRow(1); // For the second row
            InsertOrUpdateRow(2); // For the third row


            MessageBox.Show("Saved");
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

        private void lpc_dgv_searchbycode_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header click
            if (e.RowIndex < 0) return;

            DataGridViewRow selectedRow = lpc_dgv_searchbycode.Rows[e.RowIndex];

            var prodCode = selectedRow.Cells["PROD_C"].Value;
            var productName = selectedRow.Cells["PROD_N"].Value;

            //// 🧹 Clear all rows first
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
                    row.Cells[1].ReadOnly = true;
                    row.Cells[0].ReadOnly = true;
                }
            }

        }

        void InsertPromo()
        {
            //Insert data to database
            try
            {
                connection.Open();
                string dbinsert = "INSERT INTO tbl_logPriceChange (TDate,Supplier,PromoTitle,StartDate,EndDate) VALUES (?,?,?,?,?)";
                OleDbCommand cmd = new OleDbCommand(dbinsert, connection);
                cmd.Parameters.AddWithValue("?", lpc_dtp_memodate.Value.Date);
                cmd.Parameters.AddWithValue("?", lpc_tb_supplier.Text);
                cmd.Parameters.AddWithValue("?", lpc_tb_promotitle.Text);
                cmd.Parameters.AddWithValue("?", lpc_dtp_startdate.Value.ToString());
                cmd.Parameters.AddWithValue("?", lpc_dtp_enddate.Value.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Inserted Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally

            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }

        /*****************************************************************************Star For DataGridview Insert Update *******************************************************************************************************************************************************************************************************************************************************************************************************************************/
       
        
                
        
        string[] GetColumnNames(int rowIndex)
        {
            if (rowIndex == 1) // Second row (for 'SUP' columns)
            {
                return new string[]
                {
                "PROD_C", "PROD_N", "FREE_SUP", "PLFOB_SUP", "NWF_SUP", "NWFR_SUP",
                "PC_PF_SUP", "PC_PFL_SUP", "PC_RP_SUP", "PC_PA_SUP", "PC_PLSRP_SUP", "PC_LSRP_SUP",
                "PC_PPA2LP_SUP", "PC_LP_SUP", "PC_PPA2WA_SUP", "PC_WA_SUP", "PC_PPA2WB_SUP", "PC_WB_SUP",
                "PC_PPA2WC_SUP", "PC_WC_SUP", "PC_PPA2LC_SUP", "PC_LC_SUP", "PC_PPA2PG_SUP", "PC_PG_SUP",
                "PC_PPA2PH_SUP", "PC_PH_SUP", "PC_PPA2PB_SUP", "PC_PB_SUP", "PC_PPA2PD_SUP", "PC_PD_SUP",
                "LPP_AMT_SUP", "LPP_REF_SUP", "PC_PPA2PC_SUP", "PC_PC_SUP", "Claim1", "Claim2",
                "ClaimK1", "ClaimK2", "Remarks1", "Remarks2"
                };
            }
            if (rowIndex == 4) // Second row (for 'SUP' columns)
            {
                return new string[]
                {
                "PROD_C", "PROD_N", "FREE_SUP", "PLFOB_SUP", "NWF_SUP", "NWFR_SUP",
                "PC_PF_SUP", "PC_PFL_SUP", "PC_RP_SUP", "PC_PA_SUP", "PC_PLSRP_SUP", "PC_LSRP_SUP",
                "PC_PPA2LP_SUP", "PC_LP_SUP", "PC_PPA2WA_SUP", "PC_WA_SUP", "PC_PPA2WB_SUP", "PC_WB_SUP",
                "PC_PPA2WC_SUP", "PC_WC_SUP", "PC_PPA2LC_SUP", "PC_LC_SUP", "PC_PPA2PG_SUP", "PC_PG_SUP",
                "PC_PPA2PH_SUP", "PC_PH_SUP", "PC_PPA2PB_SUP", "PC_PB_SUP", "PC_PPA2PD_SUP", "PC_PD_SUP",
                "LPP_AMT_SUP", "LPP_REF_SUP", "PC_PPA2PC_SUP", "PC_PC_SUP", "FREE_USRINT",
                "PLFOB_USRINT",
                "NWF_USRINT",
                "NWFR_USRINT",
                "PC_PF_USRINT",
                "PC_PFL_USRINT",
                "PC_RP_USRINT",
                "PC_PA_USRINT",
                "PC_PLSRP_USRINT",
                "PC_LSRP_USRINT",
                "PC_PPA2LP_USRINT",
                "PC_LP_USRINT",
                "PC_PPA2WA_USRINT",
                "PC_WA_USRINT",
                "PC_PPA2WB_USRINT",
                "PC_WB_USRINT",
                "PC_PPA2WC_USRINT",
                "PC_WC_USRINT",
                "PC_PPA2LC_USRINT",
                "PC_LC_USRINT",
                "PC_PPA2PG_USRINT",
                "PC_PG_USRINT",
                "PC_PPA2PH_USRINT",
                "PC_PH_USRINT",
                "PC_PPA2PB_USRINT",
                "PC_PB_USRINT",
                "PC_PPA2P_USRINT",
                "PC_PD_USRINT",
                "LPP_AMT_USRINT",
                "LPP_REF_USRINT",
                "PC_PPA2PC_USRINT",
                "PC_PC_USRINT","FREE","PLFOB","NWF","NWFR","PC_PF","PC_PFL","PC_RP","PC_PA","PC_PLSRP","PC_LSRP","PC_PPA2LP","PC_LP","PC_PPA2WA","PC_WA","PC_PPA2WB","PC_WB","PC_PPA2WC","PC_WC","PC_PPA2LC","PC_LC","PC_PPA2PG","PC_PG","PC_PPA2PH","PC_PH","PC_PPA2PB","PC_PB","PC_PPA2PD","PC_PD","LPP_AMT","LPP_REF","PC_PPA2PC","PC_PC",
                    "Claim1", "Claim2",
                "ClaimK1", "ClaimK2", "Remarks1", "Remarks2"
                };
            }
            else if (rowIndex == 2) // Third row (for 'USRINT' columns)
            {
                return new string[]
                {
                "PROD_C",
                "PROD_N",
                "FREE_USRINT",
                "PLFOB_USRINT",
                "NWF_USRINT",
                "NWFR_USRINT",
                "PC_PF_USRINT",
                "PC_PFL_USRINT",
                "PC_RP_USRINT",
                "PC_PA_USRINT",
                "PC_PLSRP_USRINT",
                "PC_LSRP_USRINT",
                "PC_PPA2LP_USRINT",
                "PC_LP_USRINT",
                "PC_PPA2WA_USRINT",
                "PC_WA_USRINT",
                "PC_PPA2WB_USRINT",
                "PC_WB_USRINT",
                "PC_PPA2WC_USRINT",
                "PC_WC_USRINT",
                "PC_PPA2LC_USRINT",
                "PC_LC_USRINT",
                "PC_PPA2PG_USRINT",
                "PC_PG_USRINT",
                "PC_PPA2PH_USRINT",
                "PC_PH_USRINT",
                "PC_PPA2PB_USRINT",
                "PC_PB_USRINT",
                "PC_PPA2P_USRINT",
                "PC_PD_USRINT",
                "LPP_AMT_USRINT",
                "LPP_REF_USRINT",
                "PC_PPA2PC_USRINT",
                "PC_PC_USRINT",
                "Claim1",
                "Claim2",
                "ClaimK1",
                "ClaimK2",
                "Remarks1",
                "Remarks2"

                };
            }else if (rowIndex == 0)
            {
                return new string[]
                {
                    "PROD_C","PROD_N","FREE","PLFOB","NWF","NWFR","PC_PF","PC_PFL","PC_RP","PC_PA","PC_PLSRP","PC_LSRP","PC_PPA2LP","PC_LP","PC_PPA2WA","PC_WA","PC_PPA2WB","PC_WB","PC_PPA2WC","PC_WC","PC_PPA2LC","PC_LC","PC_PPA2PG","PC_PG","PC_PPA2PH","PC_PH","PC_PPA2PB","PC_PB","PC_PPA2PD","PC_PD","LPP_AMT","LPP_REF","PC_PPA2PC","PC_PC","Claim1","Claim2","ClaimK1","ClaimK2","Remarks1","Remarks2",

                };
            }
            else
            {
                // Handle additional rows here if needed
                throw new ArgumentException("Invalid row index.");
            }
        }

        void InsertOrUpdateRow(int rowIndex)
        {
            try
            {
                connection.Open();

                // Ensure the row exists in the DataGridView
                if (lpc_dgv_dbvalue.Rows.Count > rowIndex)
                {
                    DataGridViewRow row = lpc_dgv_dbvalue.Rows[rowIndex];

                    // Get column names dynamically based on the row index
                    string[] columnNames = GetColumnNames(rowIndex);

                    // Check the number of columns in the DataGridView row
                    if (row.Cells.Count != columnNames.Length)
                    {
                        MessageBox.Show($"Mismatch in column count: Expected {columnNames.Length}, but found {row.Cells.Count}.");
                        return;
                    }

                    // Collect values from the row's cells
                    List<object> values = new List<object>();
                    for (int i = 0; i < columnNames.Length; i++)
                    {
                        var cellValue = row.Cells[i].Value;
                        values.Add(cellValue != null ? cellValue.ToString().Trim() : string.Empty);
                    }

                    // Debugging: Log the values being passed
                    Console.WriteLine($"Inserting or Updating Row at Index {rowIndex}");
                    Console.WriteLine($"Columns: {string.Join(", ", columnNames)}");
                    Console.WriteLine($"Values: {string.Join(", ", values.Select(v => v?.ToString() ?? "NULL"))}");

                    string PROD_C = values[0].ToString(); // Unique identifier

                    // Check if the record with PROD_C already exists
                    string checkQuery = "SELECT COUNT(*) FROM tbl_logpricechange WHERE PROD_C = ?";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("?", PROD_C);

                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (exists > 0)
                        {
                            // If exists, update the record
                            UpdateRow(PROD_C, columnNames, values);
                        }
                        else
                        {
                            // If doesn't exist, insert the record
                            InsertRow(columnNames, values);
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Row at index {rowIndex} does not exist.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        void InsertRow(string[] cols, List<object> vals)
        {
            
            // Build column list and placeholders
            string colList = string.Join(", ", cols);  // Column names
            string placeholders = string.Join(", ", cols.Select(c => "?"));  // Placeholders for parameters

            // Construct the SQL insert query
            string insertQuery = $"INSERT INTO tbl_logpricechange (TDate, Supplier, PromoTitle, StartDate, EndDate,{colList}) VALUES (@TDate, @Supplier, @PromoTitle, @StartDate, @EndDate,{placeholders})";

            // Debugging: Log the query being constructed
            Console.WriteLine("Insert Query: " + insertQuery);
            Console.WriteLine("Columns: " + string.Join(", ", cols));
            Console.WriteLine("Values: " + string.Join(", ", vals.Select(v => v?.ToString() ?? "NULL")));

            // Create the command object with the insert query
            using (OleDbCommand cmd = new OleDbCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@TDate", lpc_dtp_memodate.Value.Date);  // TDate
                cmd.Parameters.AddWithValue("@Supplier", lpc_tb_supplier.Text);         // Supplier
                cmd.Parameters.AddWithValue("@PromoTitle", lpc_tb_promotitle.Text);       // PromoTitle
                cmd.Parameters.AddWithValue("@StartDate", lpc_dtp_startdate.Value);      // StartDate
                cmd.Parameters.AddWithValue("@EndDate", lpc_dtp_enddate.Value);
                for (int i = 0; i < vals.Count; i++)
                {
                    // Check if the value is null or empty and handle accordingly
                    var value = vals[i] == null || string.IsNullOrWhiteSpace(vals[i]?.ToString())
                                    ? DBNull.Value
                                    : vals[i];

                    cmd.Parameters.AddWithValue("?", value); // Add the value as a parameter

                    // Debugging: Log each parameter added
                    Console.WriteLine($"Parameter {i + 1}: {value}");
                }
                
                // Execute the query
                cmd.ExecuteNonQuery();
            }

            // Optional: Show a message after inserting
            MessageBox.Show($"Inserted: {vals[0]}");

         
        }

        void UpdateRow(string prodC, string[] cols, List<object> vals)
        {
            // Build the SELECT query to get the current values from the database
            string selectQuery = $"SELECT {string.Join(", ", cols.Skip(1))} FROM tbl_logpricechange WHERE PROD_C = ?";

            // Debugging: Log the SELECT query
            Console.WriteLine("Select Query: " + selectQuery);
            Console.WriteLine($"PROD_C: {prodC}");

            object[] dbValues = null;
            using (OleDbCommand selectCmd = new OleDbCommand(selectQuery, connection))
            {
                selectCmd.Parameters.AddWithValue("?", prodC);

                // Execute the SELECT query and read the current row
                using (OleDbDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dbValues = new object[cols.Length - 1];
                        for (int i = 1; i < cols.Length; i++)
                            dbValues[i - 1] = reader.GetValue(i - 1);
                    }
                }
            }

            // Compare values and prepare the UPDATE query if necessary
            bool hasChanges = false;
            List<object> updateValues = new List<object>();

            for (int i = 1; i < vals.Count; i++)
            {
                var inputValue = vals[i];
                var dbValue = dbValues[i - 1];

                // Check for differences and add updated values
                if (!object.Equals(inputValue, dbValue) && !string.IsNullOrWhiteSpace(inputValue?.ToString()))
                {
                    hasChanges = true;
                    updateValues.Add(inputValue ?? DBNull.Value);  // Use DBNull if value is null
                }
                else
                {
                    updateValues.Add(dbValue ?? DBNull.Value);  // Use database value if no change
                }
            }

            if (hasChanges)
            {
                // Build the UPDATE query with placeholders for parameters
                string setClause = string.Join(", ", cols.Skip(1).Select((c, i) => $"{c} = ?"));
                string updateQuery = $"UPDATE tbl_logpricechange SET {setClause} WHERE PROD_C = ?";

                // Debugging: Log the UPDATE query
                Console.WriteLine("Update Query: " + updateQuery);
                Console.WriteLine("Values to Update: " + string.Join(", ", updateValues.Select(v => v?.ToString() ?? "NULL")));

                using (OleDbCommand cmd = new OleDbCommand(updateQuery, connection))
                {
                    // Add parameters for updated columns
                    for (int i = 0; i < updateValues.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("?", updateValues[i]);

                        // Debugging: Log each parameter being added
                        Console.WriteLine($"Update Parameter {i + 1}: {updateValues[i]}");
                    }

                    // Add PROD_C as the WHERE condition
                    cmd.Parameters.AddWithValue("?", prodC);

                    // Execute the UPDATE query
                    cmd.ExecuteNonQuery();
                }

                // Inform the user
                MessageBox.Show($"Updated: {prodC}");
            }
            else
            {
                MessageBox.Show($"No changes detected for {prodC}");
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


/*****************************************************************************End For DataGridview Insert Update *******************************************************************************************************************************************************************************************************************************************************************************************************************************/



    }
}
