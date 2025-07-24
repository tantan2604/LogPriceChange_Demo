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
        private static Dictionary<string, int> lastNumbers = new Dictionary<string, int>();


        
        string primaryKeyColumn = "ID";
        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\VisualStudio\LogPriceChange0.1\LogPriceChange0.1\pricematrix.accdb;");
        DataTable dataTable;
        OleDbDataAdapter dataAdapter;

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
            connection.Close();
        }

        /*****************************************************************************Star For DataGridview Insert Update *******************************************************************************************************************************************************************************************************************************************************************************************************************************/
        private void lpc_dgv_searchbycode_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                connection.Open();
                string query = "SELECT * FROM tbl_billptmp";
                OleDbDataAdapter da = new OleDbDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = lpc_dgv_searchbycode.Rows[e.RowIndex];


                    // Create three rows
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
        public void InsertData()
        {

            try
            {
                string promoDate = DateTime.Now.ToString("yyyyMM");
                int docIdstart = 1;

                string maxSequenceIdQuery = "SELECT MAX(CInt(Mid(DocID, 8))) FROM tbl_logpricechange WHERE DocID LIKE @promoDate";
                using (OleDbCommand maxCmd = new OleDbCommand(maxSequenceIdQuery, connection))
                {
                    connection.Open();
                    maxCmd.Parameters.AddWithValue("@promoDate", promoDate + "-%");
                    object result = maxCmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {

                        docIdstart = Convert.ToInt32(result) + 1;
                    }
                    connection.Close();
                }

                string formattedSequenceId = docIdstart.ToString("D6");
                string docId = $"{promoDate}-{formattedSequenceId}";

                string insQuery = "INSERT INTO  tbl_logpricechange" +
                           "(TDate, Supplier, PromoTitle,  PROD_C, PROD_N, FREE, FREE_SUP, FREE_USRINT, PLFOB, PLFOB_SUP, PLFOB_USRINT, NWF, NWF_SUP, NWF_USRINT, NWFR, PC_PF, PC_PF_SUP, PC_PF_USRINT, PC_PFL, PC_PFL_SUP, PC_PFL_USRINT, PC_RP, PC_RP_SUP, PC_RP_USRINT, PC_PA, PC_PA_SUP, PC_PA_USRINT, PC_PLSRP, PC_PLSRP_SUP, PC_PLSRP_USRINT, PC_LSRP, PC_LSRP_SUP, PC_LSRP_USRINT, PC_PPA2LP, PC_PPA2LP_SUP, PC_PPA2LP_USRINT, PC_LP, PC_LP_SUP, PC_LP_USRINT, PC_PPA2WA, PC_PPA2WA_SUP, PC_PPA2WA_USRINT, PC_WA, PC_WA_SUP, PC_WA_USRINT, PC_PPA2WB, PC_PPA2WB_SUP, PC_PPA2WB_USRINT, PC_WB, PC_WB_SUP, PC_WB_USRINT, PC_PPA2WC, PC_PPA2WC_SUP, PC_PPA2WC_USRINT, PC_WC, PC_WC_SUP, PC_WC_USRINT, PC_PPA2LC, PC_PPA2LC_SUP, PC_PPA2LC_USRINT, PC_LC, PC_LC_SUP, PC_LC_USRINT, PC_PPA2PG, PC_PPA2PG_SUP, PC_PPA2PG_USRINT, PC_PG, PC_PG_SUP, PC_PG_USRINT, PC_PPA2PH, PC_PPA2PH_SUP, PC_PPA2PH_USRINT, PC_PH, PC_PH_SUP, PC_PH_USRINT, PC_PPA2PB, PC_PPA2PB_SUP, PC_PPA2PB_USRINT, PC_PB, PC_PB_SUP, PC_PB_USRINT, PC_PPA2PD, PC_PPA2PD_SUP, PC_PPA2P_USRINT, PC_PD, PC_PD_SUP, PC_PD_USRINT, LPP_AMT, LPP_AMT_SUP, LPP_AMT_USRINT, LPP_REF, LPP_REF_SUP, LPP_REF_USRINT, PC_PPA2PC, PC_PPA2PC_SUP, PC_PPA2PC_USRINT, PC_PC, PC_PC_SUP, PC_PC_USRINT, Claim1, Claim2, ClaimK1, ClaimK2, Remarks1, Remarks2, DocID, CreatedDate ) " +
                    "VALUES (@TDate,@Supplier,@PromoTitle, @PROD_C,@PROD_N,@FREE,@FREE_SUP,@FREE_USRINT,@PLFOB,@PLFOB_SUP,@PLFOB_USRINT,@NWF,@NWF_SUP,@NWF_USRINT,@NWFR,@PC_PF,@PC_PF_SUP,@PC_PF_USRINT,@PC_PFL,@PC_PFL_SUP,@PC_PFL_USRINT,@PC_RP,@PC_RP_SUP,@PC_RP_USRINT,@PC_PA,@PC_PA_SUP,@PC_PA_USRINT,@PC_PLSRP,@PC_PLSRP_SUP,@PC_PLSRP_USRINT,@PC_LSRP,@PC_LSRP_SUP,@PC_LSRP_USRINT,@PC_PPA2LP,@PC_PPA2LP_SUP,@PC_PPA2LP_USRINT,@PC_LP,@PC_LP_SUP,@PC_LP_USRINT,@PC_PPA2WA,@PC_PPA2WA_SUP,@PC_PPA2WA_USRINT,@PC_WA,@PC_WA_SUP,@PC_WA_USRINT,@PC_PPA2WB,@PC_PPA2WB_SUP,@PC_PPA2WB_USRINT,@PC_WB,@PC_WB_SUP,@PC_WB_USRINT,@PC_PPA2WC,@PC_PPA2WC_SUP,@PC_PPA2WC_USRINT,@PC_WC,@PC_WC_SUP,@PC_WC_USRINT,@PC_PPA2LC,@PC_PPA2LC_SUP,@PC_PPA2LC_USRINT,@PC_LC,@PC_LC_SUP,@PC_LC_USRINT,@PC_PPA2PG,@PC_PPA2PG_SUP,@PC_PPA2PG_USRINT,@PC_PG,@PC_PG_SUP,@PC_PG_USRINT,@PC_PPA2PH,@PC_PPA2PH_SUP,@PC_PPA2PH_USRINT,@PC_PH,@PC_PH_SUP,@PC_PH_USRINT,@PC_PPA2PB,@PC_PPA2PB_SUP,@PC_PPA2PB_USRINT,@PC_PB,@PC_PB_SUP,@PC_PB_USRINT,@PC_PPA2PD,@PC_PPA2PD_SUP,@PC_PPA2P_USRINT,@PC_PD,@PC_PD_SUP,@PC_PD_USRINT,@LPP_AMT,@LPP_AMT_SUP,@LPP_AMT_USRINT,@LPP_REF,@LPP_REF_SUP,@LPP_REF_USRINT,@PC_PPA2PC,@PC_PPA2PC_SUP,@PC_PPA2PC_USRINT,@PC_PC,@PC_PC_SUP,@PC_PC_USRINT,@Claim1,@Claim2,@ClaimK1,@ClaimK2,@Remarks1,@Remarks2, @DocID , @CreatedDate)";
                connection.Open();
                OleDbCommand cmd = new OleDbCommand(insQuery, connection);
                object GetCellValue(int rowIndex, string columnHeader)
                {
                    if (rowIndex < 0 || rowIndex >= lpc_dgv_dbvalue.Rows.Count)
                    {
                        return DBNull.Value;
                    }
                    if (!lpc_dgv_dbvalue.Columns.Contains(columnHeader))
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
                //TEXBOX & DATETIME PICKER
                cmd.Parameters.AddWithValue("@TDate", lpc_dtp_memodate.Value.Date);
                cmd.Parameters.AddWithValue("@Supplier", string.IsNullOrEmpty(lpc_tb_supplier.Text) ? (object)DBNull.Value : lpc_tb_supplier.Text);
                cmd.Parameters.AddWithValue("@PromoTitle", string.IsNullOrEmpty(lpc_tb_promotitle.Text) ? (object)DBNull.Value : lpc_tb_promotitle.Text);
                //cmd.Parameters.AddWithValue("@StartDate", string.IsNullOrEmpty(lpc_dtp_startdate.Text) ? (object)DBNull.Value : lpc_dtp_startdate.Text);
                //cmd.Parameters.AddWithValue("@EndDate", string.IsNullOrEmpty(lpc_dtp_enddate.Text) ? (object)DBNull.Value : lpc_dtp_enddate.Text);

                //DATAGRIDVIEW
                cmd.Parameters.AddWithValue("@PROD_C", GetCellValue(0, "PROD_C"));
                cmd.Parameters.AddWithValue("@PROD_N", GetCellValue(0, "PROD_N"));
                cmd.Parameters.AddWithValue("@FREE", GetCellValue(0, "SFREE"));
                cmd.Parameters.AddWithValue("@FREE_SUP", GetCellValue(1, "FREE"));
                cmd.Parameters.AddWithValue("@FREE_USRINT", GetCellValue(2, "FREE"));
                cmd.Parameters.AddWithValue("@PLFOB", GetCellValue(0, "PLFOB"));
                cmd.Parameters.AddWithValue("@PLFOB_SUP", GetCellValue(1, "PLFOB"));
                cmd.Parameters.AddWithValue("@PLFOB_USRINT", GetCellValue(2, "PLFOB"));
                cmd.Parameters.AddWithValue("@NWF", GetCellValue(0, "NWF"));
                cmd.Parameters.AddWithValue("@NWF_SUP", GetCellValue(1, "NWF"));
                cmd.Parameters.AddWithValue("@NWF_USRINT", GetCellValue(2, "NWF"));
                cmd.Parameters.AddWithValue("@NWFR", GetCellValue(0, "NWFR"));
                cmd.Parameters.AddWithValue("@PC_PF", GetCellValue(0, "PC_PF"));
                cmd.Parameters.AddWithValue("@PC_PF_SUP", GetCellValue(1, "PC_PF"));
                cmd.Parameters.AddWithValue("@PC_PF_USRINT", GetCellValue(2, "PC_PF"));
                cmd.Parameters.AddWithValue("@PC_PFL", GetCellValue(0, "PC_PFL"));
                cmd.Parameters.AddWithValue("@PC_PFL_SUP", GetCellValue(1, "PC_PFL"));
                cmd.Parameters.AddWithValue("@PC_PFL_USRINT", GetCellValue(2, "PC_PFL"));
                cmd.Parameters.AddWithValue("@PC_RP", GetCellValue(0, "PC_RP"));
                cmd.Parameters.AddWithValue("@PC_RP_SUP", GetCellValue(1, "PC_RP"));
                cmd.Parameters.AddWithValue("@PC_RP_USRINT", GetCellValue(2, "PC_RP"));
                cmd.Parameters.AddWithValue("@PC_PA", GetCellValue(0, "PC_PA"));
                cmd.Parameters.AddWithValue("@PC_PA_SUP", GetCellValue(1, "PC_PA"));
                cmd.Parameters.AddWithValue("@PC_PA_USRINT", GetCellValue(2, "PC_PA"));
                cmd.Parameters.AddWithValue("@PC_PLSRP", GetCellValue(0, "PC_PLSRP"));
                cmd.Parameters.AddWithValue("@PC_PLSRP_SUP", GetCellValue(1, "PC_PLSRP"));
                cmd.Parameters.AddWithValue("@PC_PLSRP_USRINT", GetCellValue(2, "PC_PLSRP"));
                cmd.Parameters.AddWithValue("@PC_LSRP", GetCellValue(0, "PC_LSRP"));
                cmd.Parameters.AddWithValue("@PC_LSRP_SUP", GetCellValue(1, "PC_LSRP"));
                cmd.Parameters.AddWithValue("@PC_LSRP_USRINT", GetCellValue(2, "PC_LSRP"));
                cmd.Parameters.AddWithValue("@PC_PPA2LP", GetCellValue(0, "PC_PPA2LP"));
                cmd.Parameters.AddWithValue("@PC_PPA2LP_SUP", GetCellValue(1, "PC_PPA2LP"));
                cmd.Parameters.AddWithValue("@PC_PPA2LP_USRINT", GetCellValue(2, "PC_PPA2LP"));
                cmd.Parameters.AddWithValue("@PC_LP", GetCellValue(0, "PC_LP"));
                cmd.Parameters.AddWithValue("@PC_LP_SUP", GetCellValue(1, "PC_LP"));
                cmd.Parameters.AddWithValue("@PC_LP_USRINT", GetCellValue(2, "PC_LP"));
                cmd.Parameters.AddWithValue("@PC_PPA2WA", GetCellValue(0, "PC_PPA2WA"));
                cmd.Parameters.AddWithValue("@PC_PPA2WA_SUP", GetCellValue(1, "PC_PPA2WA"));
                cmd.Parameters.AddWithValue("@PC_PPA2WA_USRINT", GetCellValue(2, "PC_PPA2WA"));
                cmd.Parameters.AddWithValue("@PC_WA", GetCellValue(0, "PC_WA"));
                cmd.Parameters.AddWithValue("@PC_WA_SUP", GetCellValue(1, "PC_WA"));
                cmd.Parameters.AddWithValue("@PC_WA_USRINT", GetCellValue(2, "PC_WA"));
                cmd.Parameters.AddWithValue("@PC_PPA2WB", GetCellValue(0, "PC_PPA2WB"));
                cmd.Parameters.AddWithValue("@PC_PPA2WB_SUP", GetCellValue(1, "PC_PPA2WB"));
                cmd.Parameters.AddWithValue("@PC_PPA2WB_USRINT", GetCellValue(2, "PC_PPA2WB"));
                cmd.Parameters.AddWithValue("@PC_WB", GetCellValue(0, "PC_WB"));
                cmd.Parameters.AddWithValue("@PC_WB_SUP", GetCellValue(1, "PC_WB"));
                cmd.Parameters.AddWithValue("@PC_WB_USRINT", GetCellValue(2, "PC_WB"));
                cmd.Parameters.AddWithValue("@PC_PPA2WC", GetCellValue(0, "PC_PPA2WC"));
                cmd.Parameters.AddWithValue("@PC_PPA2WC_SUP", GetCellValue(1, "PC_PPA2WC"));
                cmd.Parameters.AddWithValue("@PC_PPA2WC_USRINT", GetCellValue(2, "PC_PPA2WC"));
                cmd.Parameters.AddWithValue("@PC_WC", GetCellValue(0, "PC_WC"));
                cmd.Parameters.AddWithValue("@PC_WC_SUP", GetCellValue(1, "PC_WC"));
                cmd.Parameters.AddWithValue("@PC_WC_USRINT", GetCellValue(2, "PC_WC"));
                cmd.Parameters.AddWithValue("@PC_PPA2LC", GetCellValue(0, "PC_PPA2LC"));
                cmd.Parameters.AddWithValue("@PC_PPA2LC_SUP", GetCellValue(1, "PC_PPA2LC"));
                cmd.Parameters.AddWithValue("@PC_PPA2LC_USRINT", GetCellValue(2, "PC_PPA2LC"));
                cmd.Parameters.AddWithValue("@PC_LC", GetCellValue(0, "PC_LC"));
                cmd.Parameters.AddWithValue("@PC_LC_SUP", GetCellValue(1, "PC_LC"));
                cmd.Parameters.AddWithValue("@PC_LC_USRINT", GetCellValue(2, "PC_LC"));
                cmd.Parameters.AddWithValue("@PC_PPA2PG", GetCellValue(0, "PC_PPA2PG"));
                cmd.Parameters.AddWithValue("@PC_PPA2PG_SUP", GetCellValue(1, "PC_PPA2PG"));
                cmd.Parameters.AddWithValue("@PC_PPA2PG_USRINT", GetCellValue(2, "PC_PPA2PG"));
                cmd.Parameters.AddWithValue("@PC_PG", GetCellValue(0, "PC_PG"));
                cmd.Parameters.AddWithValue("@PC_PG_SUP", GetCellValue(1, "PC_PG"));
                cmd.Parameters.AddWithValue("@PC_PG_USRINT", GetCellValue(2, "PC_PG"));
                cmd.Parameters.AddWithValue("@PC_PPA2PH", GetCellValue(0, "PC_PPA2PH"));
                cmd.Parameters.AddWithValue("@PC_PPA2PH_SUP", GetCellValue(1, "PC_PPA2PH"));
                cmd.Parameters.AddWithValue("@PC_PPA2PH_USRINT", GetCellValue(2, "PC_PPA2PH"));
                cmd.Parameters.AddWithValue("@PC_PH", GetCellValue(0, "PC_PH"));
                cmd.Parameters.AddWithValue("@PC_PH_SUP", GetCellValue(1, "PC_PH"));
                cmd.Parameters.AddWithValue("@PC_PH_USRINT", GetCellValue(2, "PC_PH"));
                cmd.Parameters.AddWithValue("@PC_PPA2PB", GetCellValue(0, "PC_PPA2PB"));
                cmd.Parameters.AddWithValue("@PC_PPA2PB_SUP", GetCellValue(1, "PC_PPA2PB"));
                cmd.Parameters.AddWithValue("@PC_PPA2PB_USRINT", GetCellValue(2, "PC_PPA2PB"));
                cmd.Parameters.AddWithValue("@PC_PB", GetCellValue(0, "PC_PB"));
                cmd.Parameters.AddWithValue("@PC_PB_SUP", GetCellValue(1, "PC_PB"));
                cmd.Parameters.AddWithValue("@PC_PB_USRINT", GetCellValue(2, "PC_PB"));
                cmd.Parameters.AddWithValue("@PC_PPA2PD", GetCellValue(0, "PC_PPA2PD"));
                cmd.Parameters.AddWithValue("@PC_PPA2PD_SUP", GetCellValue(1, "PC_PPA2PD"));
                cmd.Parameters.AddWithValue("@PC_PPA2P_USRINT", GetCellValue(2, "PC_PPA2P"));
                cmd.Parameters.AddWithValue("@PC_PD", GetCellValue(0, "PC_PD"));
                cmd.Parameters.AddWithValue("@PC_PD_SUP", GetCellValue(1, "PC_PD"));
                cmd.Parameters.AddWithValue("@PC_PD_USRINT", GetCellValue(2, "PC_PD"));
                cmd.Parameters.AddWithValue("@LPP_AMT", GetCellValue(0, "LPP_AMT"));
                cmd.Parameters.AddWithValue("@LPP_AMT_SUP", GetCellValue(1, "LPP_AMT"));
                cmd.Parameters.AddWithValue("@LPP_AMT_USRINT", GetCellValue(2, "LPP_AMT"));
                cmd.Parameters.AddWithValue("@LPP_REF", GetCellValue(0, "LPP_REF"));
                cmd.Parameters.AddWithValue("@LPP_REF_SUP", GetCellValue(1, "LPP_REF"));
                cmd.Parameters.AddWithValue("@LPP_REF_USRINT", GetCellValue(2, "LPP_REF"));
                cmd.Parameters.AddWithValue("@PC_PPA2PC", GetCellValue(0, "PC_PPA2PC"));
                cmd.Parameters.AddWithValue("@PC_PPA2PC_SUP", GetCellValue(1, "PC_PPA2PC"));
                cmd.Parameters.AddWithValue("@PC_PPA2PC_USRINT", GetCellValue(2, "PC_PPA2PC"));
                cmd.Parameters.AddWithValue("@PC_PC", GetCellValue(0, "PC_PC"));
                cmd.Parameters.AddWithValue("@PC_PC_SUP", GetCellValue(1, "PC_PC"));
                cmd.Parameters.AddWithValue("@PC_PC_USRINT", GetCellValue(2, "PC_PC"));
                cmd.Parameters.AddWithValue("@Claim1", GetCellValue(1, "Claim1"));
                cmd.Parameters.AddWithValue("@Claim2", GetCellValue(2, "Claim2"));
                cmd.Parameters.AddWithValue("@ClaimK1", GetCellValue(1, "ClaimK1"));
                cmd.Parameters.AddWithValue("@ClaimK2", GetCellValue(2, "ClaimK2"));
                cmd.Parameters.AddWithValue("@Remarks1", GetCellValue(1, "Remarks1"));
                cmd.Parameters.AddWithValue("@Remarks2", GetCellValue(2, "Remarks2"));
                cmd.Parameters.AddWithValue("@DocID", docId);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString("yyyy/MM/dd/HH:mm"));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Data inserted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Inserting: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }



        }
        /*****************************************************************************End For DataGridview Insert Update *******************************************************************************************************************************************************************************************************************************************************************************************************************************/

    }
}

