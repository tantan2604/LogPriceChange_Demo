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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace LogPriceChange0._1
{
    public partial class ctrLogPriceChange : UserControl
    {
        string primaryKeyColumn = "ID";
        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\VisualStudio\LogPriceChange0.1\LogPriceChange0.1\pricematrix.accdb;");
        DataTable dataTable;
        OleDbDataAdapter dataAdapter;
        private static Dictionary<string, int> lastNumbers = new Dictionary<string, int>();

        //************************************************Column Mapping for DataGridView  lpc_dgv_dbvalue ****************************************************************
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

        //************************************************End Column Mapping for DataGridView  lpc_dgv_dbvalue ****************************************************************




        public ctrLogPriceChange()
        {
            InitializeComponent();
            lpc_dtp_enddate.CustomFormat = " ";
            this.lpc_dgv_dbvalue.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.lpc_dgv_dbvalue_CellEndEdit);


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

        private void btnlpcsubmit_Click(object sender, EventArgs e)
        {
            InsertData();
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
        /*****************************************************************************Start For DataGridview Insert Update *******************************************************************************************************************************************************************************************************************************************************************************************************************************/
        public void InsertData()
        {
            try
            {
                string promoName = lpc_tb_promotitle.Text.Trim();
                string promoType = "";
                string loggedUser = UserSession.Username;
                //string user = mainForm.dashb_lbl_userlogged.Text;
                DateTime startDate = lpc_dtp_startdate.Value;
                DateTime endDate = lpc_dtp_enddate.Value;

                // Validate Promo Name
                if (string.IsNullOrEmpty(promoName))
                {
                    MessageBox.Show("Please enter a Promo Name.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                // Determine Promo Type and End Date
                if (lpc_rbtn_permanent.Checked)
                {
                    promoType = "Permanent";
                }
                else if (lpc_rbtn_temporary.Checked)
                {
                    promoType = "Temporary";
                    endDate = lpc_dtp_enddate.Value; // Get value from enabled DateTimePicker
                }
                else
                {
                    // This case should ideally not happen if one radio button is always checked
                    MessageBox.Show("Please select a Promotion Type.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }





                // 1. Generate DocID once for all products
                string promoDate = DateTime.Now.ToString("yyyyMM");
                int docIdSequence = 1;


                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Open();
                string maxSequenceIdQuery = "SELECT MAX(CInt(Mid(DocID, 8))) FROM tbl_logpricechange WHERE DocID LIKE @promoDate";
                using (OleDbCommand maxCmd = new OleDbCommand(maxSequenceIdQuery, connection))
                {
                    maxCmd.Parameters.AddWithValue("@promoDate", promoDate + "-%");
                    object result = maxCmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        docIdSequence = Convert.ToInt32(result) + 1;
                    }
                }
                connection.Close();

                string formattedSequenceId = docIdSequence.ToString("D6");
                string commonDocId = $"{promoDate}-{formattedSequenceId}"; // This is the common DocID

                //*****************************************RADIO BUTTON*****************************************************************************************************

                // Check if there are any rows to insert
                if (lpc_dgv_dbvalue.Rows.Count == 0)
                {
                    MessageBox.Show("No products to insert. Please add products to the grid.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                for (int i = 0; i <= lpc_dgv_dbvalue.Rows.Count - 2; i += 3)
                {

                    int dbValueRowIndex = i;
                    int suppPriceRowIndex = i + 1;
                    int promoValueRowIndex = i + 2;

                    object GetCellValue(int rowIndex, string columnHeader)
                    {
                        if (rowIndex < 0 || rowIndex >= lpc_dgv_dbvalue.Rows.Count)
                        {
                            return DBNull.Value;
                        }

                        var cell = lpc_dgv_dbvalue.Rows[rowIndex].Cells[columnHeader];
                        if (cell == null || cell.Value == null || (cell.Value is string s && string.IsNullOrEmpty(s)))
                        {
                            return DBNull.Value;
                        }
                        return cell.Value;
                    }

                    string insQuery = "INSERT INTO tbl_logpricechange" +
                                      "(DocID,CreatedBy,  CreatedDate, TDate, Supplier, PromoTitle, StartDate, EndDate, Promotype, PROD_C, PROD_N, FREE, FREE_SUP, FREE_USRINT, PLFOB, PLFOB_SUP, PLFOB_USRINT, NWF, NWF_SUP, NWF_USRINT, NWFR, PC_PF, PC_PF_SUP, PC_PF_USRINT, PC_PFL, PC_PFL_SUP, PC_PFL_USRINT, PC_RP, PC_RP_SUP, PC_RP_USRINT, PC_PA, PC_PA_SUP, PC_PA_USRINT, PC_PLSRP, PC_PLSRP_SUP, PC_PLSRP_USRINT, PC_LSRP, PC_LSRP_SUP, PC_LSRP_USRINT, PC_PPA2LP, PC_PPA2LP_SUP, PC_PPA2LP_USRINT, PC_LP, PC_LP_SUP, PC_LP_USRINT, PC_PPA2WA, PC_PPA2WA_SUP, PC_PPA2WA_USRINT, PC_WA, PC_WA_SUP, PC_WA_USRINT, PC_PPA2WB, PC_PPA2WB_SUP, PC_PPA2WB_USRINT, PC_WB, PC_WB_SUP, PC_WB_USRINT, PC_PPA2WC, PC_PPA2WC_SUP, PC_PPA2WC_USRINT, PC_WC, PC_WC_SUP, PC_WC_USRINT, PC_PPA2LC, PC_PPA2LC_SUP, PC_PPA2LC_USRINT, PC_LC, PC_LC_SUP, PC_LC_USRINT, PC_PPA2PG, PC_PPA2PG_SUP, PC_PPA2PG_USRINT, PC_PG, PC_PG_SUP, PC_PG_USRINT, PC_PPA2PH, PC_PPA2PH_SUP, PC_PPA2PH_USRINT, PC_PH, PC_PH_SUP, PC_PH_USRINT, PC_PPA2PB, PC_PPA2PB_SUP, PC_PPA2PB_USRINT, PC_PB, PC_PB_SUP, PC_PB_USRINT, PC_PPA2PD, PC_PPA2PD_SUP, PC_PPA2P_USRINT, PC_PD, PC_PD_SUP, PC_PD_USRINT, LPP_AMT, LPP_AMT_SUP, LPP_AMT_USRINT, LPP_REF, LPP_REF_SUP, LPP_REF_USRINT, PC_PPA2PC, PC_PPA2PC_SUP, PC_PPA2PC_USRINT, PC_PC, PC_PC_SUP, PC_PC_USRINT, Claim1, Claim2, ClaimK1, ClaimK2, Remarks1, Remarks2) " +
                                      "VALUES ( @DocID, @CreatedBy, @CreatedDate, @TDate,@Supplier, @PromoTitle, @StartDate, @EndDate, @Promotype,  @PROD_C,@PROD_N,@FREE,@FREE_SUP,@FREE_USRINT,@PLFOB,@PLFOB_SUP,@PLFOB_USRINT,@NWF,@NWF_SUP,@NWF_USRINT,@NWFR,@PC_PF,@PC_PF_SUP,@PC_PF_USRINT,@PC_PFL,@PC_PFL_SUP,@PC_PFL_USRINT,@PC_RP,@PC_RP_SUP,@PC_RP_USRINT,@PC_PA,@PC_PA_SUP,@PC_PA_USRINT,@PC_PLSRP,@PC_PLSRP_SUP,@PC_PLSRP_USRINT,@PC_LSRP,@PC_LSRP_SUP,@PC_LSRP_USRINT,@PC_PPA2LP,@PC_PPA2LP_SUP,@PC_PPA2LP_USRINT,@PC_LP,@PC_LP_SUP,@PC_LP_USRINT,@PC_PPA2WA,@PC_PPA2WA_SUP,@PC_PPA2WA_USRINT,@PC_WA,@PC_WA_SUP,@PC_WA_USRINT,@PC_PPA2WB,@PC_PPA2WB_SUP,@PC_PPA2WB_USRINT,@PC_WB,@PC_WB_SUP,@PC_WB_USRINT,@PC_PPA2WC,@PC_PPA2WC_SUP,@PC_PPA2WC_USRINT,@PC_WC,@PC_WC_SUP,@PC_WC_USRINT,@PC_PPA2LC,@PC_PPA2LC_SUP,@PC_PPA2LC_USRINT,@PC_LC,@PC_LC_SUP,@PC_LC_USRINT,@PC_PPA2PG,@PC_PPA2PG_SUP,@PC_PPA2PG_USRINT,@PC_PG,@PC_PG_SUP,@PC_PG_USRINT,@PC_PPA2PH,@PC_PPA2PH_SUP,@PC_PPA2PH_USRINT,@PC_PH,@PC_PH_SUP,@PC_PH_USRINT,@PC_PPA2PB,@PC_PPA2PB_SUP,@PC_PPA2PB_USRINT,@PC_PB,@PC_PB_SUP,@PC_PB_USRINT,@PC_PPA2PD,@PC_PPA2PD_SUP,@PC_PPA2P_USRINT,@PC_PD,@PC_PD_SUP,@PC_PD_USRINT,@LPP_AMT,@LPP_AMT_SUP,@LPP_AMT_USRINT,@LPP_REF,@LPP_REF_SUP,@LPP_REF_USRINT,@PC_PPA2PC,@PC_PPA2PC_SUP,@PC_PPA2PC_USRINT,@PC_PC,@PC_PC_SUP,@PC_PC_USRINT,@Claim1,@Claim2,@ClaimK1,@ClaimK2,@Remarks1,@Remarks2)";


                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (OleDbCommand cmd = new OleDbCommand(insQuery, connection))
                    {
                        // TEXBOX & DATETIME PICKER (These are common for all products in this submission)
                        cmd.Parameters.AddWithValue("@DocID", commonDocId); // Use the common DocID
                        //DocStatus
                        cmd.Parameters.AddWithValue("@CreatedBy", loggedUser);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString("yyyy/MM/dd/HH:mm"));
                        //ApprovedBy
                        //ApprovedDate
                        cmd.Parameters.AddWithValue("@TDate", lpc_dtp_memodate.Value.Date);
                        cmd.Parameters.AddWithValue("@Supplier", string.IsNullOrEmpty(lpc_tb_supplier.Text) ? (object)DBNull.Value : lpc_tb_supplier.Text);
                        cmd.Parameters.AddWithValue("@PromoTitle", promoName);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", (promoType == "Permanent") ? (object)DBNull.Value : endDate);
                        cmd.Parameters.AddWithValue("@PromoType", promoType);

                        // DATAGRIDVIEW VALUES FOR CURRENT PRODUCT
                        cmd.Parameters.AddWithValue("@PROD_C", GetCellValue(dbValueRowIndex, "PROD_C"));
                        cmd.Parameters.AddWithValue("@PROD_N", GetCellValue(dbValueRowIndex, "PROD_N"));
                        cmd.Parameters.AddWithValue("@FREE", GetCellValue(dbValueRowIndex, "FREE")); // Assuming SFREE is actually FREE in the source DGV
                        cmd.Parameters.AddWithValue("@FREE_SUP", GetCellValue(suppPriceRowIndex, "FREE"));
                        cmd.Parameters.AddWithValue("@FREE_USRINT", GetCellValue(promoValueRowIndex, "FREE"));
                        cmd.Parameters.AddWithValue("@PLFOB", GetCellValue(dbValueRowIndex, "PLFOB"));
                        cmd.Parameters.AddWithValue("@PLFOB_SUP", GetCellValue(suppPriceRowIndex, "PLFOB"));
                        cmd.Parameters.AddWithValue("@PLFOB_USRINT", GetCellValue(promoValueRowIndex, "PLFOB"));
                        cmd.Parameters.AddWithValue("@NWF", GetCellValue(dbValueRowIndex, "NWF"));
                        cmd.Parameters.AddWithValue("@NWF_SUP", GetCellValue(suppPriceRowIndex, "NWF"));
                        cmd.Parameters.AddWithValue("@NWF_USRINT", GetCellValue(promoValueRowIndex, "NWF"));
                        cmd.Parameters.AddWithValue("@NWFR", GetCellValue(dbValueRowIndex, "NWFR"));
                        cmd.Parameters.AddWithValue("@PC_PF", GetCellValue(dbValueRowIndex, "PC_PF"));
                        cmd.Parameters.AddWithValue("@PC_PF_SUP", GetCellValue(suppPriceRowIndex, "PC_PF"));
                        cmd.Parameters.AddWithValue("@PC_PF_USRINT", GetCellValue(promoValueRowIndex, "PC_PF"));
                        cmd.Parameters.AddWithValue("@PC_PFL", GetCellValue(dbValueRowIndex, "PC_PFL"));
                        cmd.Parameters.AddWithValue("@PC_PFL_SUP", GetCellValue(suppPriceRowIndex, "PC_PFL"));
                        cmd.Parameters.AddWithValue("@PC_PFL_USRINT", GetCellValue(promoValueRowIndex, "PC_PFL"));
                        cmd.Parameters.AddWithValue("@PC_RP", GetCellValue(dbValueRowIndex, "PC_RP"));
                        cmd.Parameters.AddWithValue("@PC_RP_SUP", GetCellValue(suppPriceRowIndex, "PC_RP"));
                        cmd.Parameters.AddWithValue("@PC_RP_USRINT", GetCellValue(promoValueRowIndex, "PC_RP"));
                        cmd.Parameters.AddWithValue("@PC_PA", GetCellValue(dbValueRowIndex, "PC_PA"));
                        cmd.Parameters.AddWithValue("@PC_PA_SUP", GetCellValue(suppPriceRowIndex, "PC_PA"));
                        cmd.Parameters.AddWithValue("@PC_PA_USRINT", GetCellValue(promoValueRowIndex, "PC_PA"));
                        cmd.Parameters.AddWithValue("@PC_PLSRP", GetCellValue(dbValueRowIndex, "PC_PLSRP"));
                        cmd.Parameters.AddWithValue("@PC_PLSRP_SUP", GetCellValue(suppPriceRowIndex, "PC_PLSRP"));
                        cmd.Parameters.AddWithValue("@PC_PLSRP_USRINT", GetCellValue(promoValueRowIndex, "PC_PLSRP"));
                        cmd.Parameters.AddWithValue("@PC_LSRP", GetCellValue(dbValueRowIndex, "PC_LSRP"));
                        cmd.Parameters.AddWithValue("@PC_LSRP_SUP", GetCellValue(suppPriceRowIndex, "PC_LSRP"));
                        cmd.Parameters.AddWithValue("@PC_LSRP_USRINT", GetCellValue(promoValueRowIndex, "PC_LSRP"));
                        cmd.Parameters.AddWithValue("@PC_PPA2LP", GetCellValue(dbValueRowIndex, "PC_PPA2LP"));
                        cmd.Parameters.AddWithValue("@PC_PPA2LP_SUP", GetCellValue(suppPriceRowIndex, "PC_PPA2LP"));
                        cmd.Parameters.AddWithValue("@PC_PPA2LP_USRINT", GetCellValue(promoValueRowIndex, "PC_PPA2LP"));
                        cmd.Parameters.AddWithValue("@PC_LP", GetCellValue(dbValueRowIndex, "PC_LP"));
                        cmd.Parameters.AddWithValue("@PC_LP_SUP", GetCellValue(suppPriceRowIndex, "PC_LP"));
                        cmd.Parameters.AddWithValue("@PC_LP_USRINT", GetCellValue(promoValueRowIndex, "PC_LP"));
                        cmd.Parameters.AddWithValue("@PC_PPA2WA", GetCellValue(dbValueRowIndex, "PC_PPA2WA"));
                        cmd.Parameters.AddWithValue("@PC_PPA2WA_SUP", GetCellValue(suppPriceRowIndex, "PC_PPA2WA"));
                        cmd.Parameters.AddWithValue("@PC_PPA2WA_USRINT", GetCellValue(promoValueRowIndex, "PC_PPA2WA"));
                        cmd.Parameters.AddWithValue("@PC_WA", GetCellValue(dbValueRowIndex, "PC_WA"));
                        cmd.Parameters.AddWithValue("@PC_WA_SUP", GetCellValue(suppPriceRowIndex, "PC_WA"));
                        cmd.Parameters.AddWithValue("@PC_WA_USRINT", GetCellValue(promoValueRowIndex, "PC_WA"));
                        cmd.Parameters.AddWithValue("@PC_PPA2WB", GetCellValue(dbValueRowIndex, "PC_PPA2WB"));
                        cmd.Parameters.AddWithValue("@PC_PPA2WB_SUP", GetCellValue(suppPriceRowIndex, "PC_PPA2WB"));
                        cmd.Parameters.AddWithValue("@PC_PPA2WB_USRINT", GetCellValue(promoValueRowIndex, "PC_PPA2WB"));
                        cmd.Parameters.AddWithValue("@PC_WB", GetCellValue(dbValueRowIndex, "PC_WB"));
                        cmd.Parameters.AddWithValue("@PC_WB_SUP", GetCellValue(suppPriceRowIndex, "PC_WB"));
                        cmd.Parameters.AddWithValue("@PC_WB_USRINT", GetCellValue(promoValueRowIndex, "PC_WB"));
                        cmd.Parameters.AddWithValue("@PC_PPA2WC", GetCellValue(dbValueRowIndex, "PC_PPA2WC"));
                        cmd.Parameters.AddWithValue("@PC_PPA2WC_SUP", GetCellValue(suppPriceRowIndex, "PC_PPA2WC"));
                        cmd.Parameters.AddWithValue("@PC_PPA2WC_USRINT", GetCellValue(promoValueRowIndex, "PC_PPA2WC"));
                        cmd.Parameters.AddWithValue("@PC_WC", GetCellValue(dbValueRowIndex, "PC_WC"));
                        cmd.Parameters.AddWithValue("@PC_WC_SUP", GetCellValue(suppPriceRowIndex, "PC_WC"));
                        cmd.Parameters.AddWithValue("@PC_WC_USRINT", GetCellValue(promoValueRowIndex, "PC_WC"));
                        cmd.Parameters.AddWithValue("@PC_PPA2LC", GetCellValue(dbValueRowIndex, "PC_PPA2LC"));
                        cmd.Parameters.AddWithValue("@PC_PPA2LC_SUP", GetCellValue(suppPriceRowIndex, "PC_PPA2LC"));
                        cmd.Parameters.AddWithValue("@PC_PPA2LC_USRINT", GetCellValue(promoValueRowIndex, "PC_PPA2LC"));
                        cmd.Parameters.AddWithValue("@PC_LC", GetCellValue(dbValueRowIndex, "PC_LC"));
                        cmd.Parameters.AddWithValue("@PC_LC_SUP", GetCellValue(suppPriceRowIndex, "PC_LC"));
                        cmd.Parameters.AddWithValue("@PC_LC_USRINT", GetCellValue(promoValueRowIndex, "PC_LC"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PG", GetCellValue(dbValueRowIndex, "PC_PPA2PG"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PG_SUP", GetCellValue(suppPriceRowIndex, "PC_PPA2PG"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PG_USRINT", GetCellValue(promoValueRowIndex, "PC_PPA2PG"));
                        cmd.Parameters.AddWithValue("@PC_PG", GetCellValue(dbValueRowIndex, "PC_PG"));
                        cmd.Parameters.AddWithValue("@PC_PG_SUP", GetCellValue(suppPriceRowIndex, "PC_PG"));
                        cmd.Parameters.AddWithValue("@PC_PG_USRINT", GetCellValue(promoValueRowIndex, "PC_PG"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PH", GetCellValue(dbValueRowIndex, "PC_PPA2PH"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PH_SUP", GetCellValue(suppPriceRowIndex, "PC_PPA2PH"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PH_USRINT", GetCellValue(promoValueRowIndex, "PC_PPA2PH"));
                        cmd.Parameters.AddWithValue("@PC_PH", GetCellValue(dbValueRowIndex, "PC_PH"));
                        cmd.Parameters.AddWithValue("@PC_PH_SUP", GetCellValue(suppPriceRowIndex, "PC_PH"));
                        cmd.Parameters.AddWithValue("@PC_PH_USRINT", GetCellValue(promoValueRowIndex, "PC_PH"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PB", GetCellValue(dbValueRowIndex, "PC_PPA2PB"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PB_SUP", GetCellValue(suppPriceRowIndex, "PC_PPA2PB"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PB_USRINT", GetCellValue(promoValueRowIndex, "PC_PPA2PB"));
                        cmd.Parameters.AddWithValue("@PC_PB", GetCellValue(dbValueRowIndex, "PC_PB"));
                        cmd.Parameters.AddWithValue("@PC_PB_SUP", GetCellValue(suppPriceRowIndex, "PC_PB"));
                        cmd.Parameters.AddWithValue("@PC_PB_USRINT", GetCellValue(promoValueRowIndex, "PC_PB"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PD", GetCellValue(dbValueRowIndex, "PC_PPA2PD"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PD_SUP", GetCellValue(suppPriceRowIndex, "PC_PPA2PD"));
                        cmd.Parameters.AddWithValue("@PC_PPA2P_USRINT", GetCellValue(promoValueRowIndex, "PC_PPA2PD"));
                        cmd.Parameters.AddWithValue("@PC_PD", GetCellValue(dbValueRowIndex, "PC_PD"));
                        cmd.Parameters.AddWithValue("@PC_PD_SUP", GetCellValue(suppPriceRowIndex, "PC_PD"));
                        cmd.Parameters.AddWithValue("@PC_PD_USRINT", GetCellValue(promoValueRowIndex, "PC_PD"));
                        cmd.Parameters.AddWithValue("@LPP_AMT", GetCellValue(dbValueRowIndex, "LPP_AMT"));
                        cmd.Parameters.AddWithValue("@LPP_AMT_SUP", GetCellValue(suppPriceRowIndex, "LPP_AMT"));
                        cmd.Parameters.AddWithValue("@LPP_AMT_USRINT", GetCellValue(promoValueRowIndex, "LPP_AMT"));
                        cmd.Parameters.AddWithValue("@LPP_REF", GetCellValue(dbValueRowIndex, "LPP_REF"));
                        cmd.Parameters.AddWithValue("@LPP_REF_SUP", GetCellValue(suppPriceRowIndex, "LPP_REF"));
                        cmd.Parameters.AddWithValue("@LPP_REF_USRINT", GetCellValue(promoValueRowIndex, "LPP_REF"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PC", GetCellValue(dbValueRowIndex, "PC_PPA2PC"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PC_SUP", GetCellValue(suppPriceRowIndex, "PC_PPA2PC"));
                        cmd.Parameters.AddWithValue("@PC_PPA2PC_USRINT", GetCellValue(promoValueRowIndex, "PC_PPA2PC"));
                        cmd.Parameters.AddWithValue("@PC_PC", GetCellValue(dbValueRowIndex, "PC_PC"));
                        cmd.Parameters.AddWithValue("@PC_PC_SUP", GetCellValue(suppPriceRowIndex, "PC_PC"));
                        cmd.Parameters.AddWithValue("@PC_PC_USRINT", GetCellValue(promoValueRowIndex, "PC_PC"));
                        cmd.Parameters.AddWithValue("@Claim1", GetCellValue(suppPriceRowIndex, "Claim1"));
                        cmd.Parameters.AddWithValue("@Claim2", GetCellValue(promoValueRowIndex, "Claim2"));
                        cmd.Parameters.AddWithValue("@ClaimK1", GetCellValue(suppPriceRowIndex, "ClaimK1"));
                        cmd.Parameters.AddWithValue("@ClaimK2", GetCellValue(promoValueRowIndex, "ClaimK2"));
                        cmd.Parameters.AddWithValue("@Remarks1", GetCellValue(suppPriceRowIndex, "Remarks1"));
                        cmd.Parameters.AddWithValue("@Remarks2", GetCellValue(promoValueRowIndex, "Remarks2"));



                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show($"Data for {lpc_dgv_dbvalue.Rows.Count / 3} product(s) inserted successfully with DocID: {commonDocId}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lpc_dgv_dbvalue.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Inserting: " + ex.Message, "Insertion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                lpc_dtp_startdate.Value = DateTime.Now; // Reset to current date/time
                lpc_dtp_startdate.Checked = true; // Reset to default permanent selection
                lpc_tb_promotitle.Clear();
                lpc_tb_supplier.Clear();
                if (connection.State == ConnectionState.Open)
                {

                    connection.Close();
                }
            }
        }
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
                        decimal rate = val / baseValue;
                        row.Cells[mapping.RateColumn].Value = rate.ToString("0.00");
                    }
                }
                else if (editedColumnName == mapping.RateColumn)
                {
                    if (decimal.TryParse(row.Cells[mapping.RateColumn]?.Value?.ToString(), out decimal rate))
                    {
                        decimal val = baseValue * rate;
                        row.Cells[mapping.ValueColumn].Value = val.ToString("0.00");
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
        private void lpc_dgv_dbvalue_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex < 0) return;

            //var dgv = lpc_dgv_dbvalue;

            //// Only proceed if not in edit mode to avoid partial data
            //if (dgv.IsCurrentCellInEditMode) return;

            //var row = dgv.Rows[e.RowIndex];
            //string changedColumn = dgv.Columns[e.ColumnIndex].Name;

            //CalculateLSRPValues(row, changedColumn);
        }
    }
}
