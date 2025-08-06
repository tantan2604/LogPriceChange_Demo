using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LogPriceChange0._1
{
    
    public partial class ctrLogPriceChange : UserControl
    {
        string username = UserSession.Username;

        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\SharedDB\pricematrix.accdb;");
        private static Dictionary<string, int> lastNumbers = new Dictionary<string, int>();
        private DataGridViewRow rightClickedRow;
        
        private string GetFullName(string username)
        {
            string fullName = string.Empty;
            string query = "SELECT Lastname, Firstname FROM tbl_employee WHERE Username = ?";
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                // The parameter is for the username
                command.Parameters.Add("?", username);

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
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving full name: " + ex.Message, "Database Error");
                }
            }
            return fullName;
        }
        public ctrLogPriceChange()
        {
            InitializeComponent();
            

            // Set custom format for your DateTimePicker
            lpc_dtp_enddate.CustomFormat = " ";

            // Hook up events
            this.lpc_dgv_dbvalue.CellEndEdit += lpc_dgv_dbvalue_CellEndEdit;
            this.lpc_dgv_dbvalue.CellMouseDown += lpc_dgv_dbvalue_CellMouseDown;

            // Ensure context menu item is connected
            cmsRemoveGroupItem.Click += cmsRemoveGroupItem_Click;
            cmsRemoveGroup.Opening += cmsRemoveGroup_Opening;

            // Assign the ContextMenuStrip to the DataGridView (if not done in designer)
            lpc_dgv_dbvalue.ContextMenuStrip = cmsRemoveGroup;
        }
        //***********************************************Remove Rows in datagridview**********************************************************************************************************************************************************************************************
        #region RightClick remove in lpc_dgv_dbvalue 
        private void lpc_dgv_dbvalue_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                rightClickedRow = lpc_dgv_dbvalue.Rows[e.RowIndex];
                lpc_dgv_dbvalue.ClearSelection();
                lpc_dgv_dbvalue.CurrentCell = rightClickedRow.Cells[e.ColumnIndex];
                rightClickedRow.Selected = true;
            }
        }

        private void cmsRemoveGroup_Opening(object sender, CancelEventArgs e)
        {
            // Disable context menu if no valid row was right-clicked
            if (rightClickedRow == null)
                e.Cancel = true;
        }

        private void cmsRemoveGroupItem_Click(object sender, EventArgs e)
        {
            if (rightClickedRow == null)
                return;

            int rowIndex = rightClickedRow.Index;
            int groupStart = rowIndex - (rowIndex % 3);

            // Safety: Ensure 3 rows exist in group
            if (groupStart + 2 >= lpc_dgv_dbvalue.Rows.Count)
            {
                MessageBox.Show("Unable to remove group. Not enough rows.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Remove from bottom up to avoid index shifting
            for (int i = 2; i >= 0; i--)
            {
                int rowToRemove = groupStart + i;
                if (rowToRemove < lpc_dgv_dbvalue.Rows.Count)
                    lpc_dgv_dbvalue.Rows.RemoveAt(rowToRemove);
            }

            rightClickedRow = null;
        }
        #endregion 
        //***********************************************End of Remove Rows in datagridview*************************************************************************************************************************************************************************************************************************

        //************************************************Column Mapping for DataGridView  lpc_dgv_dbvalue ****************************************************************
        #region ColumnMapping for Auto Compute
        private class ColumnMapping
        {
            public string BaseColumn { get; set; }
            public string ValueColumn { get; set; }
            public string RateColumn { get; set; }
        }

        private readonly List<ColumnMapping> columnMappings = new List<ColumnMapping>
    {
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_LSRP", RateColumn = "PC_PLSRP"},
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_LP", RateColumn = "PC_PPA2LP"},
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_WA", RateColumn = "PC_PPA2WA"},
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_WB", RateColumn = "PC_PPA2WB"},
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_WC", RateColumn = "PC_PPA2WC"},
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_LC", RateColumn = "PC_PPA2LC"},
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_PG", RateColumn = "PC_PPA2PG"},
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_PH", RateColumn = "PC_PPA2PH"},
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_PB", RateColumn = "PC_PPA2PB"},
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_PD", RateColumn = "PC_PPA2PD"},
      new ColumnMapping { BaseColumn = "PC_PA", ValueColumn = "PC_PC", RateColumn = "PC_PPA2PC"}
        // Add more mappings here if needed
    };
       
        #endregion
        //************************************************End Column Mapping for DataGridView  lpc_dgv_dbvalue ****************************************************************

        private void lpc_rbtn_permanent_CheckedChanged(object sender, EventArgs e)
        {
            if (lpc_rbtn_permanent.Checked)
            {
                lpc_dtp_enddate.Enabled = false;
                lpc_dtp_enddate.CustomFormat = " "; // Use a space to effectively hide the date display
                lpc_dtp_enddate.Format = DateTimePickerFormat.Custom;
            }
            else if (lpc_rbtn_temporary.Checked)
            {
                lpc_dtp_enddate.Enabled = true;
                lpc_dtp_enddate.CustomFormat = "MM / dd / yyyy hh: mm: ss tt";
                lpc_dtp_enddate.Format = DateTimePickerFormat.Custom;
            }
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
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void lpc_dgv_searchbycode_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Ensure connection is closed before opening again for the query
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Open();
                string query = "SELECT * FROM tbl_billptmp";
                OleDbDataAdapter da = new OleDbDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = lpc_dgv_searchbycode.Rows[e.RowIndex];

                    // Check if the product is already added to prevent duplicates
                    string prodCode = selectedRow.Cells["PROD_C"].Value?.ToString();
                    bool productAlreadyAdded = false;
                    for (int i = 0; i < lpc_dgv_dbvalue.Rows.Count; i += 3)
                    {
                        if (lpc_dgv_dbvalue.Rows[i].Cells["PROD_C"].Value?.ToString() == prodCode)
                        {
                            productAlreadyAdded = true;
                            break;
                        }
                    }

                    if (productAlreadyAdded)
                    {
                        MessageBox.Show("This product has already been added.", "Duplicate Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Create three rows for the selected product
                    int mainRowIndex = lpc_dgv_dbvalue.Rows.Add();
                    int suppRowIndex = lpc_dgv_dbvalue.Rows.Add();
                    int promoRowIndex = lpc_dgv_dbvalue.Rows.Add();

                    // Fill mainRow with selected data
                    DataGridViewRow mainRow = lpc_dgv_dbvalue.Rows[mainRowIndex];
                    for (int i = 0; i < selectedRow.Cells.Count && i < lpc_dgv_dbvalue.Columns.Count; i++)
                    {
                        mainRow.Cells[i].Value = selectedRow.Cells[i].Value;
                    }

                    // Set row header labels
                    lpc_dgv_dbvalue.Rows[mainRowIndex].HeaderCell.Value = "Database Value";
                    lpc_dgv_dbvalue.Rows[suppRowIndex].HeaderCell.Value = "Supplier Price";
                    lpc_dgv_dbvalue.Rows[promoRowIndex].HeaderCell.Value = "Promo Value";

                    // Optionally make main row read-only
                    lpc_dgv_dbvalue.Rows[mainRowIndex].ReadOnly = true;

                    // Clear the search textbox after adding
                    lpc_tb_searchbycode.Clear();
                    lpc_dgv_searchbycode.Visible = false; // Hide search results
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /**************************************************Start For DataGridview Insert Update *******************************************************************************************************************************************************************************************************************************************************************************************************************************/
        
        private void UpdateDependentValues(int rowIndex, string editedColumnName)
        {
            if (rowIndex < 0 || rowIndex >= lpc_dgv_dbvalue.Rows.Count)
                return;

            var row = lpc_dgv_dbvalue.Rows[rowIndex];

            foreach (var mapping in columnMappings)
            {
                if (editedColumnName != mapping.ValueColumn && editedColumnName != mapping.RateColumn)
                    continue;

                if (!decimal.TryParse(row.Cells[mapping.BaseColumn]?.Value?.ToString(), out decimal baseValue) || baseValue == 0)
                    return;

                if (editedColumnName == mapping.ValueColumn)
                {
                    if (decimal.TryParse(row.Cells[mapping.ValueColumn]?.Value?.ToString(), out decimal val))
                    {
                        decimal rate = val / baseValue * 100;
                        int roundedRate = (int)Math.Round(rate);
                        row.Cells[mapping.RateColumn].Value = roundedRate.ToString();
                    }
                }
                else if (editedColumnName == mapping.RateColumn)
                {
                    if (decimal.TryParse(row.Cells[mapping.RateColumn]?.Value?.ToString(), out decimal rate))
                    {
                        decimal val = (baseValue * rate) / 100;
                        int roundedVal = (int)Math.Round(val);
                        row.Cells[mapping.ValueColumn].Value = roundedVal.ToString();
                    }
                }

                break;
            }
        }

        private void lpc_dgv_dbvalue_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string columnName = lpc_dgv_dbvalue.Columns[e.ColumnIndex].Name;
            UpdateDependentValues(e.RowIndex, columnName);
        }

        //************************************************Upsert*******************************************************************************************************************
        #region
        public void InsertData(string docStatus)
        {
            // The DocStatus is now passed as a parameter.
            // Call this method with "ForApproval" for the Submit button
            // and "Draft" for the Draft button.

            if (lpc_tb_promotitle.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a Promo Name.");
                return;
            }
            string promoName = lpc_tb_promotitle.Text.Trim();
            string promoType = lpc_rbtn_permanent.Checked ? "Permanent"
                                : lpc_rbtn_temporary.Checked ? "Temporary"
                                : null;
            if (promoType == null)
            {
                MessageBox.Show("Please select a Promotion Type.");
                return;
            }
            DateTime startDate = lpc_dtp_startdate.Value;
            DateTime endDate = lpc_dtp_enddate.Value;
            string loggedUser = GetFullName(UserSession.Username);
            string promoDate = DateTime.Now.ToString("yyyyMM");
            int docIdSequence = 1;
            connection.Open();
            using (var maxCmd = new OleDbCommand("SELECT MAX(CInt(Mid(DocID,8))) FROM tbl_logpricechange WHERE DocID LIKE ?", connection))
            {
                maxCmd.Parameters.AddWithValue("?", promoDate + "-%");
                object res = maxCmd.ExecuteScalar();
                if (res != null && res != DBNull.Value && int.TryParse(res.ToString(), out int mx))
                    docIdSequence = mx + 1;
            }
            string formattedSeq = docIdSequence.ToString("D6");
            string commonDocId = $"{promoDate}-{formattedSeq}";

            int totalRows = lpc_dgv_dbvalue.Rows.Count;
            if (totalRows < 3 || totalRows % 3 != 0)
            {
                MessageBox.Show("Grid must contain rows in exact multiples of 3.");
                connection.Close();
                return;
            }

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    string insQuery = "INSERT INTO tbl_logpricechange (DocID, DocStatus, CreatedBy,  CreatedDate, TDate, Supplier, PromoTitle, StartDate, EndDate, Promotype, PROD_C, PROD_N, FREE, FREE_SUP, FREE_USRINT, PLFOB, PLFOB_SUP, PLFOB_USRINT, NWF, NWF_SUP, NWF_USRINT, NWFR, PC_PF, PC_PF_SUP, PC_PF_USRINT, PC_PFL, PC_PFL_SUP, PC_PFL_USRINT, PC_RP, PC_RP_SUP, PC_RP_USRINT, PC_PA, PC_PA_SUP, PC_PA_USRINT, PC_PLSRP, PC_PLSRP_SUP, PC_PLSRP_USRINT, PC_LSRP, PC_LSRP_SUP, PC_LSRP_USRINT, PC_PPA2LP, PC_PPA2LP_SUP, PC_PPA2LP_USRINT, PC_LP, PC_LP_SUP, PC_LP_USRINT, PC_PPA2WA, PC_PPA2WA_SUP, PC_PPA2WA_USRINT, PC_WA, PC_WA_SUP, PC_WA_USRINT, PC_PPA2WB, PC_PPA2WB_SUP, PC_PPA2WB_USRINT, PC_WB, PC_WB_SUP, PC_WB_USRINT, PC_PPA2WC, PC_PPA2WC_SUP, PC_PPA2WC_USRINT, PC_WC, PC_WC_SUP, PC_WC_USRINT, PC_PPA2LC, PC_PPA2LC_SUP, PC_PPA2LC_USRINT, PC_LC, PC_LC_SUP, PC_LC_USRINT, PC_PPA2PG, PC_PPA2PG_SUP, PC_PPA2PG_USRINT, PC_PG, PC_PG_SUP, PC_PG_USRINT, PC_PPA2PH, PC_PPA2PH_SUP, PC_PPA2PH_USRINT, PC_PH, PC_PH_SUP, PC_PH_USRINT, PC_PPA2PB, PC_PPA2PB_SUP, PC_PPA2PB_USRINT, PC_PB, PC_PB_SUP, PC_PB_USRINT, PC_PPA2PD, PC_PPA2PD_SUP, PC_PPA2P_USRINT, PC_PD, PC_PD_SUP, PC_PD_USRINT, LPP_AMT, LPP_AMT_SUP, LPP_AMT_USRINT, LPP_REF, LPP_REF_SUP, LPP_REF_USRINT, PC_PPA2PC, PC_PPA2PC_SUP, PC_PPA2PC_USRINT, PC_PC, PC_PC_SUP, PC_PC_USRINT, Claim1, Claim2, ClaimK1, ClaimK2, Remarks1, Remarks2) " +
                                                                "VALUES ( @DocID,@DocStatus, @CreatedBy, @CreatedDate, @TDate,@Supplier, @PromoTitle, @StartDate, @EndDate, @Promotype,  @PROD_C,@PROD_N,@FREE,@FREE_SUP,@FREE_USRINT,@PLFOB,@PLFOB_SUP,@PLFOB_USRINT,@NWF,@NWF_SUP,@NWF_USRINT,@NWFR,@PC_PF,@PC_PF_SUP,@PC_PF_USRINT,@PC_PFL,@PC_PFL_SUP,@PC_PFL_USRINT,@PC_RP,@PC_RP_SUP,@PC_RP_USRINT,@PC_PA,@PC_PA_SUP,@PC_PA_USRINT,@PC_PLSRP,@PC_PLSRP_SUP,@PC_PLSRP_USRINT,@PC_LSRP,@PC_LSRP_SUP,@PC_LSRP_USRINT,@PC_PPA2LP,@PC_PPA2LP_SUP,@PC_PPA2LP_USRINT,@PC_LP,@PC_LP_SUP,@PC_LP_USRINT,@PC_PPA2WA,@PC_PPA2WA_SUP,@PC_PPA2WA_USRINT,@PC_WA,@PC_WA_SUP,@PC_WA_USRINT,@PC_PPA2WB,@PC_PPA2WB_SUP,@PC_PPA2WB_USRINT,@PC_WB,@PC_WB_SUP,@PC_WB_USRINT,@PC_PPA2WC,@PC_PPA2WC_SUP,@PC_PPA2WC_USRINT,@PC_WC,@PC_WC_SUP,@PC_WC_USRINT,@PC_PPA2LC,@PC_PPA2LC_SUP,@PC_PPA2LC_USRINT,@PC_LC,@PC_LC_SUP,@PC_LC_USRINT,@PC_PPA2PG,@PC_PPA2PG_SUP,@PC_PPA2PG_USRINT,@PC_PG,@PC_PG_SUP,@PC_PG_USRINT,@PC_PPA2PH,@PC_PPA2PH_SUP,@PC_PPA2PH_USRINT,@PC_PH,@PC_PH_SUP,@PC_PH_USRINT,@PC_PPA2PB,@PC_PPA2PB_SUP,@PC_PPA2PB_USRINT,@PC_PB,@PC_PB_SUP,@PC_PB_USRINT,@PC_PPA2PD,@PC_PPA2PD_SUP,@PC_PPA2P_USRINT,@PC_PD,@PC_PD_SUP,@PC_PD_USRINT,@LPP_AMT,@LPP_AMT_SUP,@LPP_AMT_USRINT,@LPP_REF,@LPP_REF_SUP,@LPP_REF_USRINT,@PC_PPA2PC,@PC_PPA2PC_SUP,@PC_PPA2PC_USRINT,@PC_PC,@PC_PC_SUP,@PC_PC_USRINT,@Claim1,@Claim2,@ClaimK1,@ClaimK2,@Remarks1,@Remarks2)";
                    for (int i = 0; i < totalRows; i += 3)
                    {
                        int dbRow = i, suppRow = i + 1, promoRow = i + 2;
                        string currentPROD_C = GetCellValue(dbRow, "PROD_C")?.ToString();

                        // case1: both PROD_C + PromoTitle exist?
                        using (var cmd = new OleDbCommand("SELECT COUNT(*) FROM tbl_logpricechange WHERE PROD_C = ? AND PromoTitle = ?", connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("?", currentPROD_C);
                            cmd.Parameters.AddWithValue("?", promoName);
                            int both = Convert.ToInt32(cmd.ExecuteScalar());
                            if (both > 0)
                            {
                                // UPDATE
                                using (var upd = new OleDbCommand(
                                    "UPDATE tbl_logpricechange SET CreatedBy = ?, CreatedDate = ?, TDate = ?, Supplier = ?, StartDate = ?, EndDate = ?, PromoType = ?, DocStatus = ?, FREE = ?, FREE_SUP = ?, FREE_USRINT = ? WHERE PROD_C = ? AND PromoTitle = ?",
                                    connection, transaction))
                                {
                                    upd.Parameters.AddWithValue("?", loggedUser);
                                    upd.Parameters.AddWithValue("?", DateTime.Now);
                                    upd.Parameters.AddWithValue("?", lpc_dtp_memodate.Value.Date);
                                    upd.Parameters.AddWithValue("?", string.IsNullOrEmpty(lpc_tb_supplier.Text) ? (object)DBNull.Value : lpc_tb_supplier.Text);
                                    upd.Parameters.AddWithValue("?", startDate);
                                    upd.Parameters.AddWithValue("?", promoType == "Permanent" ? (object)DBNull.Value : (object)endDate);
                                    upd.Parameters.AddWithValue("?", promoType);
                                    upd.Parameters.AddWithValue("?", docStatus); // Use the new parameter
                                    upd.Parameters.AddWithValue("?", GetCellValue(dbRow, "FREE"));
                                    upd.Parameters.AddWithValue("?", GetCellValue(suppRow, "FREE"));
                                    upd.Parameters.AddWithValue("?", GetCellValue(promoRow, "FREE"));
                                    upd.Parameters.AddWithValue("?", currentPROD_C);
                                    upd.Parameters.AddWithValue("?", promoName);
                                    upd.ExecuteNonQuery();
                                }
                                continue;
                            }
                        }

                        // case2 or case3
                        string docIdToUse = commonDocId;
                        using (var cmd2 = new OleDbCommand("SELECT TOP 1 DocID FROM tbl_logpricechange WHERE PromoTitle = ?", connection, transaction))
                        {
                            cmd2.Parameters.AddWithValue("?", promoName);
                            object f = cmd2.ExecuteScalar();
                            if (f != null) docIdToUse = f.ToString();
                        }

                        // INSERT with correct docId
                        InsertProductRow(connection, transaction, docIdToUse, dbRow, suppRow, promoRow, commonDocId, promoName, promoType, startDate, endDate, loggedUser, insQuery, docStatus); // Pass the new parameter

                    }

                    transaction.Commit();
                    MessageBox.Show($"Inserted/Updated successfully under DocID: {commonDocId}");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            connection.Close();
            // Reset controls
            lpc_dgv_dbvalue.Rows.Clear();
            lpc_tb_promotitle.Clear();
            lpc_tb_supplier.Clear();
            lpc_dtp_startdate.Value = DateTime.Now;
            lpc_dtp_startdate.Checked = true;
        }

        // Helper to extract value safely
        private object GetCellValue(int row, string col)
        {
            var cell = lpc_dgv_dbvalue.Rows[row].Cells[col];
            if (cell == null || cell.Value == null || (cell.Value is string s && s.Trim() == ""))
                return DBNull.Value;
            return cell.Value;
        }

        // Helper to perform insert for a product row
        // The signature has been updated to include the docStatus parameter
        private void InsertProductRow(OleDbConnection conn, OleDbTransaction tx,
        string docIdToUse, int dbRow, int suppRow, int promoRow,
        string commonDocId, string promoName, string promoType,
        DateTime startDate, DateTime endDate, string loggedUser,
        string insQuery, string docStatus)
        {
            using (var cmd = new OleDbCommand(insQuery, conn, tx))
            {
                cmd.Parameters.AddWithValue("@DocID", docIdToUse);
                cmd.Parameters.AddWithValue("@DocStatus", docStatus); // Use the new parameter
                cmd.Parameters.AddWithValue("@CreatedBy", loggedUser);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@TDate", lpc_dtp_memodate.Value.Date);
                cmd.Parameters.AddWithValue("@Supplier", string.IsNullOrEmpty(lpc_tb_supplier.Text) ? (object)DBNull.Value : lpc_tb_supplier.Text);
                cmd.Parameters.AddWithValue("@PromoTitle", promoName);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", promoType == "Permanent" ? (object)DBNull.Value : (object)endDate);
                cmd.Parameters.AddWithValue("@PromoType", promoType);

                cmd.Parameters.AddWithValue("@PROD_C", GetCellValue(dbRow, "PROD_C"));
                cmd.Parameters.AddWithValue("@PROD_N", GetCellValue(dbRow, "PROD_N"));
                cmd.Parameters.AddWithValue("@FREE", GetCellValue(dbRow, "FREE")); // Assuming SFREE is actually FREE in the source DGV
                cmd.Parameters.AddWithValue("@FREE_SUP", GetCellValue(suppRow, "FREE"));
                cmd.Parameters.AddWithValue("@FREE_USRINT", GetCellValue(promoRow, "FREE"));
                cmd.Parameters.AddWithValue("@PLFOB", GetCellValue(dbRow, "PLFOB"));
                cmd.Parameters.AddWithValue("@PLFOB_SUP", GetCellValue(suppRow, "PLFOB"));
                cmd.Parameters.AddWithValue("@PLFOB_USRINT", GetCellValue(promoRow, "PLFOB"));
                cmd.Parameters.AddWithValue("@NWF", GetCellValue(dbRow, "NWF"));
                cmd.Parameters.AddWithValue("@NWF_SUP", GetCellValue(suppRow, "NWF"));
                cmd.Parameters.AddWithValue("@NWF_USRINT", GetCellValue(promoRow, "NWF"));
                cmd.Parameters.AddWithValue("@NWFR", GetCellValue(dbRow, "NWFR"));
                cmd.Parameters.AddWithValue("@PC_PF", GetCellValue(dbRow, "PC_PF"));
                cmd.Parameters.AddWithValue("@PC_PF_SUP", GetCellValue(suppRow, "PC_PF"));
                cmd.Parameters.AddWithValue("@PC_PF_USRINT", GetCellValue(promoRow, "PC_PF"));
                cmd.Parameters.AddWithValue("@PC_PFL", GetCellValue(dbRow, "PC_PFL"));
                cmd.Parameters.AddWithValue("@PC_PFL_SUP", GetCellValue(suppRow, "PC_PFL"));
                cmd.Parameters.AddWithValue("@PC_PFL_USRINT", GetCellValue(promoRow, "PC_PFL"));
                cmd.Parameters.AddWithValue("@PC_RP", GetCellValue(dbRow, "PC_RP"));
                cmd.Parameters.AddWithValue("@PC_RP_SUP", GetCellValue(suppRow, "PC_RP"));
                cmd.Parameters.AddWithValue("@PC_RP_USRINT", GetCellValue(promoRow, "PC_RP"));
                cmd.Parameters.AddWithValue("@PC_PA", GetCellValue(dbRow, "PC_PA"));
                cmd.Parameters.AddWithValue("@PC_PA_SUP", GetCellValue(suppRow, "PC_PA"));
                cmd.Parameters.AddWithValue("@PC_PA_USRINT", GetCellValue(promoRow, "PC_PA"));
                cmd.Parameters.AddWithValue("@PC_PLSRP", GetCellValue(dbRow, "PC_PLSRP"));
                cmd.Parameters.AddWithValue("@PC_PLSRP_SUP", GetCellValue(suppRow, "PC_PLSRP"));
                cmd.Parameters.AddWithValue("@PC_PLSRP_USRINT", GetCellValue(promoRow, "PC_PLSRP"));
                cmd.Parameters.AddWithValue("@PC_LSRP", GetCellValue(dbRow, "PC_LSRP"));
                cmd.Parameters.AddWithValue("@PC_LSRP_SUP", GetCellValue(suppRow, "PC_LSRP"));
                cmd.Parameters.AddWithValue("@PC_LSRP_USRINT", GetCellValue(promoRow, "PC_LSRP"));
                cmd.Parameters.AddWithValue("@PC_PPA2LP", GetCellValue(dbRow, "PC_PPA2LP"));
                cmd.Parameters.AddWithValue("@PC_PPA2LP_SUP", GetCellValue(suppRow, "PC_PPA2LP"));
                cmd.Parameters.AddWithValue("@PC_PPA2LP_USRINT", GetCellValue(promoRow, "PC_PPA2LP"));
                cmd.Parameters.AddWithValue("@PC_LP", GetCellValue(dbRow, "PC_LP"));
                cmd.Parameters.AddWithValue("@PC_LP_SUP", GetCellValue(suppRow, "PC_LP"));
                cmd.Parameters.AddWithValue("@PC_LP_USRINT", GetCellValue(promoRow, "PC_LP"));
                cmd.Parameters.AddWithValue("@PC_PPA2WA", GetCellValue(dbRow, "PC_PPA2WA"));
                cmd.Parameters.AddWithValue("@PC_PPA2WA_SUP", GetCellValue(suppRow, "PC_PPA2WA"));
                cmd.Parameters.AddWithValue("@PC_PPA2WA_USRINT", GetCellValue(promoRow, "PC_PPA2WA"));
                cmd.Parameters.AddWithValue("@PC_WA", GetCellValue(dbRow, "PC_WA"));
                cmd.Parameters.AddWithValue("@PC_WA_SUP", GetCellValue(suppRow, "PC_WA"));
                cmd.Parameters.AddWithValue("@PC_WA_USRINT", GetCellValue(promoRow, "PC_WA"));
                cmd.Parameters.AddWithValue("@PC_PPA2WB", GetCellValue(dbRow, "PC_PPA2WB"));
                cmd.Parameters.AddWithValue("@PC_PPA2WB_SUP", GetCellValue(suppRow, "PC_PPA2WB"));
                cmd.Parameters.AddWithValue("@PC_PPA2WB_USRINT", GetCellValue(promoRow, "PC_PPA2WB"));
                cmd.Parameters.AddWithValue("@PC_WB", GetCellValue(dbRow, "PC_WB"));
                cmd.Parameters.AddWithValue("@PC_WB_SUP", GetCellValue(suppRow, "PC_WB"));
                cmd.Parameters.AddWithValue("@PC_WB_USRINT", GetCellValue(promoRow, "PC_WB"));
                cmd.Parameters.AddWithValue("@PC_PPA2WC", GetCellValue(dbRow, "PC_PPA2WC"));
                cmd.Parameters.AddWithValue("@PC_PPA2WC_SUP", GetCellValue(suppRow, "PC_PPA2WC"));
                cmd.Parameters.AddWithValue("@PC_PPA2WC_USRINT", GetCellValue(promoRow, "PC_PPA2WC"));
                cmd.Parameters.AddWithValue("@PC_WC", GetCellValue(dbRow, "PC_WC"));
                cmd.Parameters.AddWithValue("@PC_WC_SUP", GetCellValue(suppRow, "PC_WC"));
                cmd.Parameters.AddWithValue("@PC_WC_USRINT", GetCellValue(promoRow, "PC_WC"));
                cmd.Parameters.AddWithValue("@PC_PPA2LC", GetCellValue(dbRow, "PC_PPA2LC"));
                cmd.Parameters.AddWithValue("@PC_PPA2LC_SUP", GetCellValue(suppRow, "PC_PPA2LC"));
                cmd.Parameters.AddWithValue("@PC_PPA2LC_USRINT", GetCellValue(promoRow, "PC_PPA2LC"));
                cmd.Parameters.AddWithValue("@PC_LC", GetCellValue(dbRow, "PC_LC"));
                cmd.Parameters.AddWithValue("@PC_LC_SUP", GetCellValue(suppRow, "PC_LC"));
                cmd.Parameters.AddWithValue("@PC_LC_USRINT", GetCellValue(promoRow, "PC_LC"));
                cmd.Parameters.AddWithValue("@PC_PPA2PG", GetCellValue(dbRow, "PC_PPA2PG"));
                cmd.Parameters.AddWithValue("@PC_PPA2PG_SUP", GetCellValue(suppRow, "PC_PPA2PG"));
                cmd.Parameters.AddWithValue("@PC_PPA2PG_USRINT", GetCellValue(promoRow, "PC_PPA2PG"));
                cmd.Parameters.AddWithValue("@PC_PG", GetCellValue(dbRow, "PC_PG"));
                cmd.Parameters.AddWithValue("@PC_PG_SUP", GetCellValue(suppRow, "PC_PG"));
                cmd.Parameters.AddWithValue("@PC_PG_USRINT", GetCellValue(promoRow, "PC_PG"));
                cmd.Parameters.AddWithValue("@PC_PPA2PH", GetCellValue(dbRow, "PC_PPA2PH"));
                cmd.Parameters.AddWithValue("@PC_PPA2PH_SUP", GetCellValue(suppRow, "PC_PPA2PH"));
                cmd.Parameters.AddWithValue("@PC_PPA2PH_USRINT", GetCellValue(promoRow, "PC_PPA2PH"));
                cmd.Parameters.AddWithValue("@PC_PH", GetCellValue(dbRow, "PC_PH"));
                cmd.Parameters.AddWithValue("@PC_PH_SUP", GetCellValue(suppRow, "PC_PH"));
                cmd.Parameters.AddWithValue("@PC_PH_USRINT", GetCellValue(promoRow, "PC_PH"));
                cmd.Parameters.AddWithValue("@PC_PPA2PB", GetCellValue(dbRow, "PC_PPA2PB"));
                cmd.Parameters.AddWithValue("@PC_PPA2PB_SUP", GetCellValue(suppRow, "PC_PPA2PB"));
                cmd.Parameters.AddWithValue("@PC_PPA2PB_USRINT", GetCellValue(promoRow, "PC_PPA2PB"));
                cmd.Parameters.AddWithValue("@PC_PB", GetCellValue(dbRow, "PC_PB"));
                cmd.Parameters.AddWithValue("@PC_PB_SUP", GetCellValue(suppRow, "PC_PB"));
                cmd.Parameters.AddWithValue("@PC_PB_USRINT", GetCellValue(promoRow, "PC_PB"));
                cmd.Parameters.AddWithValue("@PC_PPA2PD", GetCellValue(dbRow, "PC_PPA2PD"));
                cmd.Parameters.AddWithValue("@PC_PPA2PD_SUP", GetCellValue(suppRow, "PC_PPA2PD"));
                cmd.Parameters.AddWithValue("@PC_PPA2P_USRINT", GetCellValue(promoRow, "PC_PPA2PD"));
                cmd.Parameters.AddWithValue("@PC_PD", GetCellValue(dbRow, "PC_PD"));
                cmd.Parameters.AddWithValue("@PC_PD_SUP", GetCellValue(suppRow, "PC_PD"));
                cmd.Parameters.AddWithValue("@PC_PD_USRINT", GetCellValue(promoRow, "PC_PD"));
                cmd.Parameters.AddWithValue("@LPP_AMT", GetCellValue(dbRow, "LPP_AMT"));
                cmd.Parameters.AddWithValue("@LPP_AMT_SUP", GetCellValue(suppRow, "LPP_AMT"));
                cmd.Parameters.AddWithValue("@LPP_AMT_USRINT", GetCellValue(promoRow, "LPP_AMT"));
                cmd.Parameters.AddWithValue("@LPP_REF", GetCellValue(dbRow, "LPP_REF"));
                cmd.Parameters.AddWithValue("@LPP_REF_SUP", GetCellValue(suppRow, "LPP_REF"));
                cmd.Parameters.AddWithValue("@LPP_REF_USRINT", GetCellValue(promoRow, "LPP_REF"));
                cmd.Parameters.AddWithValue("@PC_PPA2PC", GetCellValue(dbRow, "PC_PPA2PC"));
                cmd.Parameters.AddWithValue("@PC_PPA2PC_SUP", GetCellValue(suppRow, "PC_PPA2PC"));
                cmd.Parameters.AddWithValue("@PC_PPA2PC_USRINT", GetCellValue(promoRow, "PC_PPA2PC"));
                cmd.Parameters.AddWithValue("@PC_PC", GetCellValue(dbRow, "PC_PC"));
                cmd.Parameters.AddWithValue("@PC_PC_SUP", GetCellValue(suppRow, "PC_PC"));
                cmd.Parameters.AddWithValue("@PC_PC_USRINT", GetCellValue(promoRow, "PC_PC"));
                cmd.Parameters.AddWithValue("@Claim1", GetCellValue(suppRow, "Claim1"));
                cmd.Parameters.AddWithValue("@Claim2", GetCellValue(promoRow, "Claim2"));
                cmd.Parameters.AddWithValue("@ClaimK1", GetCellValue(suppRow, "ClaimK1"));
                cmd.Parameters.AddWithValue("@ClaimK2", GetCellValue(promoRow, "ClaimK2"));
                cmd.Parameters.AddWithValue("@Remarks1", GetCellValue(suppRow, "Remarks1"));
                cmd.Parameters.AddWithValue("@Remarks2", GetCellValue(promoRow, "Remarks2"));

                cmd.ExecuteNonQuery();

            }
        }
        private void btnlpcsubmit_Click(object sender, EventArgs e)
        {
            // Correct call for submitting for approval
            DialogResult result = MessageBox.Show("Are you sure you want to submit this for approval?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                InsertData("ForApproval");
            }
        }
        private void btn_draft_Click(object sender, EventArgs e)
        {

            //InsertData("Draft");

            MessageBox.Show("" + " " + UserSession.Username);
        }
        #endregion
        


    }
}
