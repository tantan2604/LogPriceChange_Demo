using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceMatrix.Class
{
    public class ProductInserter
    {
        private OleDbConnection conn;
        private OleDbTransaction tx;
        private string insertQuery;

        public ProductInserter(OleDbConnection connection, OleDbTransaction transaction, string query)
        {
            conn = connection;
            tx = transaction;
            insertQuery = query;
        }

        public void InsertProductRow(
            string docIdToUse,
            int dbRow, int suppRow, int promoRow,
            string promoName,
            string promoType,
            DateTime startDate,
            DateTime endDate,
            string loggedUser,
            string docStatus,
            DateTime memoDate,
            string supplier,
            DataGridView dgv_Main)
            
        {
            using (var cmd = new OleDbCommand(insertQuery, conn, tx))
            {
                cmd.Parameters.AddWithValue("@DocID", docIdToUse);
                cmd.Parameters.AddWithValue("@DocStatus", docStatus);
                cmd.Parameters.AddWithValue("@CreatedBy", loggedUser);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@ApprovedBy", DBNull.Value);     // <-- add this
                cmd.Parameters.AddWithValue("@ApprovedDate", DBNull.Value);
                var paramTDate = cmd.Parameters.Add("@TDate", OleDbType.Date);
                paramTDate.Value = memoDate;
                cmd.Parameters.AddWithValue("@Supplier", supplier); // or from form
                cmd.Parameters.AddWithValue("@PromoTitle", promoName);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", promoType == "Permanent" ? (object)DBNull.Value : endDate);
                cmd.Parameters.AddWithValue("@PromoType", promoType);

                // Add a few sample fields
                AddParam(cmd, "@PROD_C", dgv_Main, dbRow, "PROD_C");
                AddParam(cmd, "@PROD_N", dgv_Main, dbRow, "PROD_N");

                AddParam(cmd, "@FREE", dgv_Main, dbRow, "FREE");
                AddParam(cmd, "@FREE_SUP", dgv_Main, suppRow ,"FREE");
                AddParam(cmd, "@FREE_USRINT", dgv_Main, promoRow, "FREE");

                AddParam(cmd, "@PLFOB", dgv_Main, dbRow, "PLFOB");
                AddParam(cmd, "@PLFOB_SUP", dgv_Main, suppRow, "PLFOB");
                AddParam(cmd, "@PLFOB_USRINT", dgv_Main, promoRow, "PLFOB");

                AddParam(cmd, "@NWF", dgv_Main, dbRow, "NWF");
                AddParam(cmd, "@NWF_SUP", dgv_Main, suppRow, "NWF");
                AddParam(cmd, "@NWF_USRINT", dgv_Main, promoRow, "NWF");
                

                AddParam(cmd, "@NWFR", dgv_Main, dbRow, "NWFR");
                AddParam(cmd, "@NWFR_SUP", dgv_Main, suppRow, "NWFR");
                AddParam(cmd, "@NWFR_USRINT", dgv_Main, promoRow, "NWFR");

                AddParam(cmd, "@PC_PF", dgv_Main, dbRow, "PC_PF");
                AddParam(cmd, "@PC_PF_SUP", dgv_Main, suppRow, "PC_PF");
                AddParam(cmd, "@PC_PF_USRINT", dgv_Main, promoRow, "PC_PF");

                AddParam(cmd, "@PC_PFL", dgv_Main, dbRow, "PC_PFL");
                AddParam(cmd, "@PC_PFL_SUP", dgv_Main, suppRow, "PC_PFL");
                AddParam(cmd, "@PC_PFL_USRINT", dgv_Main, promoRow, "PC_PFL");

                AddParam(cmd, "@PC_RP", dgv_Main, dbRow, "PC_RP");
                AddParam(cmd, "@PC_RP_SUP", dgv_Main, suppRow, "PC_RP");
                AddParam(cmd, "@PC_RP_USRINT", dgv_Main, promoRow, "PC_RP");

                AddParam(cmd, "@PC_PA", dgv_Main, dbRow, "PC_PA");
                AddParam(cmd, "@PC_PA_SUP", dgv_Main, suppRow, "PC_PA");
                AddParam(cmd, "@PC_PA_USRINT", dgv_Main, promoRow, "PC_PA");

                AddParam(cmd, "@PC_PLSRP", dgv_Main, dbRow, "PC_PLSRP");
                AddParam(cmd, "@PC_PLSRP_SUP", dgv_Main, suppRow, "PC_PLSRP");
                AddParam(cmd, "@PC_PLSRP_USRINT", dgv_Main, promoRow, "PC_PLSRP");

                AddParam(cmd, "@PC_LSRP", dgv_Main, dbRow, "PC_LSRP");
                AddParam(cmd, "@PC_LSRP_SUP", dgv_Main, suppRow, "PC_LSRP");
                AddParam(cmd, "@PC_LSRP_USRINT", dgv_Main, promoRow, "PC_LSRP");

                AddParam(cmd, "@PC_PPA2LP", dgv_Main, dbRow, "PC_PPA2LP");
                AddParam(cmd, "@PC_PPA2LP_SUP", dgv_Main, suppRow, "PC_PPA2LP");
                AddParam(cmd, "@PC_PPA2LP_USRINT", dgv_Main, promoRow, "PC_PPA2LP");

                AddParam(cmd, "@PC_LP", dgv_Main, dbRow, "PC_LP");
                AddParam(cmd, "@PC_LP_SUP", dgv_Main, suppRow, "PC_LP");
                AddParam(cmd, "@PC_LP_USRINT", dgv_Main, promoRow, "PC_LP");

                AddParam(cmd, "@PC_PPA2WA", dgv_Main, dbRow, "PC_PPA2WA");
                AddParam(cmd, "@PC_PPA2WA_SUP", dgv_Main, suppRow, "PC_PPA2WA");
                AddParam(cmd, "@PC_PPA2WA_USRINT", dgv_Main, promoRow, "PC_PPA2WA");

                AddParam(cmd, "@PC_WA", dgv_Main, dbRow, "PC_WA");
                AddParam(cmd, "@PC_WA_SUP", dgv_Main, suppRow, "PC_WA");
                AddParam(cmd, "@PC_WA_USRINT", dgv_Main, promoRow, "PC_WA");

                AddParam(cmd, "@PC_PPA2WB", dgv_Main, dbRow, "PC_PPA2WB");
                AddParam(cmd, "@PC_PPA2WB_SUP", dgv_Main, suppRow, "PC_PPA2WB");
                AddParam(cmd, "@PC_PPA2WB_USRINT", dgv_Main, promoRow, "PC_PPA2WB");

                AddParam(cmd, "@PC_WB", dgv_Main, dbRow, "PC_WB");
                AddParam(cmd, "@PC_WB_SUP", dgv_Main, suppRow, "PC_WB");
                AddParam(cmd, "@PC_WB_USRINT", dgv_Main, promoRow, "PC_WB");

                AddParam(cmd, "@PC_PPA2WC", dgv_Main, dbRow, "PC_PPA2WC");
                AddParam(cmd, "@PC_PPA2WC_SUP", dgv_Main, suppRow, "PC_PPA2WC");
                AddParam(cmd, "@PC_PPA2WC_USRINT", dgv_Main, promoRow, "PC_PPA2WC");

                AddParam(cmd, "@PC_WC", dgv_Main, dbRow, "PC_WC");
                AddParam(cmd, "@PC_WC_SUP", dgv_Main, suppRow, "PC_WC");
                AddParam(cmd, "@PC_WC_USRINT", dgv_Main, promoRow, "PC_WC");

                AddParam(cmd, "@PC_PPA2LC", dgv_Main, dbRow, "PC_PPA2LC");
                AddParam(cmd, "@PC_PPA2LC_SUP", dgv_Main, suppRow, "PC_PPA2LC");
                AddParam(cmd, "@PC_PPA2LC_USRINT", dgv_Main, promoRow, "PC_PPA2LC");

                AddParam(cmd, "@PC_LC", dgv_Main, dbRow, "PC_LC");
                AddParam(cmd, "@PC_LC_SUP", dgv_Main, suppRow, "PC_LC");
                AddParam(cmd, "@PC_LC_USRINT", dgv_Main, promoRow, "PC_LC");

                AddParam(cmd, "@PC_PPA2PG", dgv_Main, dbRow, "PC_PPA2PG");
                AddParam(cmd, "@PC_PPA2PG_SUP", dgv_Main, suppRow, "PC_PPA2PG");
                AddParam(cmd, "@PC_PPA2PG_USRINT", dgv_Main, promoRow, "PC_PPA2PG");

                AddParam(cmd, "@PC_PG", dgv_Main, dbRow, "PC_PG");
                AddParam(cmd, "@PC_PG_SUP", dgv_Main, suppRow, "PC_PG");
                AddParam(cmd, "@PC_PG_USRINT", dgv_Main, promoRow, "PC_PG");

                AddParam(cmd, "@PC_PPA2PH", dgv_Main, dbRow, "PC_PPA2PH");
                AddParam(cmd, "@PC_PPA2PH_SUP", dgv_Main, suppRow, "PC_PPA2PH");
                AddParam(cmd, "@PC_PPA2PH_USRINT", dgv_Main, promoRow, "PC_PPA2PH");

                AddParam(cmd, "@PC_PH", dgv_Main, dbRow, "PC_PH");
                AddParam(cmd, "@PC_PH_SUP", dgv_Main, suppRow, "PC_PH");
                AddParam(cmd, "@PC_PH_USRINT", dgv_Main, promoRow, "PC_PH");

                AddParam(cmd, "@PC_PPA2PB", dgv_Main, dbRow, "PC_PPA2PB");
                AddParam(cmd, "@PC_PPA2PB_SUP", dgv_Main, suppRow, "PC_PPA2PB");
                AddParam(cmd, "@PC_PPA2PB_USRINT", dgv_Main, promoRow, "PC_PPA2PB");

                AddParam(cmd, "@PC_PB", dgv_Main, dbRow, "PC_PB");
                AddParam(cmd, "@PC_PB_SUP", dgv_Main, suppRow, "PC_PB");
                AddParam(cmd, "@PC_PB_USRINT", dgv_Main, promoRow, "PC_PB");

                AddParam(cmd, "@PC_PPA2PD", dgv_Main, dbRow, "PC_PPA2PD");
                AddParam(cmd, "@PC_PPA2PD_SUP", dgv_Main, suppRow, "PC_PPA2PD");
                AddParam(cmd, "@PC_PPA2PD_USRINT", dgv_Main, promoRow, "PC_PPA2PD");

                AddParam(cmd, "@PC_PD", dgv_Main, dbRow, "PC_PD");
                AddParam(cmd, "@PC_PD_SUP", dgv_Main, suppRow, "PC_PD");
                AddParam(cmd, "@PC_PD_USRINT", dgv_Main, promoRow, "PC_PD");

                AddParam(cmd, "@LPP_AMT", dgv_Main, dbRow, "LPP_AMT");
                AddParam(cmd, "@LPP_AMT_SUP", dgv_Main, suppRow, "LPP_AMT");
                AddParam(cmd, "@LPP_AMT_USRINT", dgv_Main, promoRow, "LPP_AMT");

                AddParam(cmd, "@LPP_REF", dgv_Main, dbRow, "LPP_REF");
                AddParam(cmd, "@LPP_REF_SUP", dgv_Main, suppRow, "LPP_REF");
                AddParam(cmd, "@LPP_REF_USRINT", dgv_Main, promoRow, "LPP_REF");

                AddParam(cmd, "@PC_PPA2PC", dgv_Main, dbRow, "PC_PPA2PC");
                AddParam(cmd, "@PC_PPA2PC_SUP", dgv_Main, suppRow, "PC_PPA2PC");
                AddParam(cmd, "@PC_PPA2PC_USRINT", dgv_Main, promoRow, "PC_PPA2PC");

                AddParam(cmd, "@PC_PC", dgv_Main, dbRow, "PC_PC");
                AddParam(cmd, "@PC_PC_SUP", dgv_Main, suppRow, "PC_PC");
                AddParam(cmd, "@PC_PC_USRINT", dgv_Main, promoRow, "PC_PC");

                AddParam(cmd, "@Claim", dgv_Main, dbRow, "Claim");
                AddParam(cmd, "@Claim_SUP", dgv_Main, suppRow, "Claim");
                AddParam(cmd, "@Claim_USRINT", dgv_Main, promoRow, "Claim");

                AddParam(cmd, "@ClaimK", dgv_Main, dbRow, "ClaimK");
                AddParam(cmd, "@ClaimK_SUP", dgv_Main, suppRow, "ClaimK");
                AddParam(cmd, "@ClaimK_USRINT", dgv_Main, promoRow, "ClaimK");

                AddParam(cmd, "@Remarks", dgv_Main, dbRow, "Remarks");
                AddParam(cmd, "@Remarks_SUP", dgv_Main, suppRow, "Remarks");
                AddParam(cmd, "@Remarks_USRINT", dgv_Main, promoRow, "Remarks");


                

                cmd.ExecuteNonQuery();
            }
        }

        private void AddParam(OleDbCommand cmd, string paramName, DataGridView dgv_Main, int rowIndex, string columnName)
        {
            object val = dgv_Main.Rows[rowIndex].Cells[columnName].Value;
            cmd.Parameters.AddWithValue(paramName, val ?? DBNull.Value);
        }
    }

    public class ProductUpdater
    {
        private OleDbConnection conn;
        private OleDbTransaction tx;
        private string updateQuery;

        public ProductUpdater(OleDbConnection connection, OleDbTransaction transaction, string query)
        {
            conn = connection;
            tx = transaction;
            updateQuery = query;
        }

        public void UpdateProductRow(

            string docIdToUse,
            int dbRow, int suppRow, int promoRow,
            string promoName,
            string promoType,
            DateTime startDate,
            DateTime endDate,
            string loggedUser,
            string docStatus,
            DateTime memoDate,
            string supplier,
            DataGridView dgv_Main)
        {
            using (var cmd = new OleDbCommand(updateQuery, conn, tx))
            {
                try 
                {
                    cmd.Parameters.AddWithValue("@DocStatus", docStatus);
                    cmd.Parameters.AddWithValue("@PromoTitle", promoName);
                    cmd.Parameters.AddWithValue("@PromoType", promoType);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", promoType == "Permanent" ? (object)DBNull.Value : endDate);
                    cmd.Parameters.AddWithValue("@CreatedBy", loggedUser);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@MemoDate", memoDate);
                    cmd.Parameters.AddWithValue("@Supplier", supplier);


                    //Sample fields to update:
                    AddParam(cmd, "@PROD_N", dgv_Main, dbRow, "PROD_N");

                    AddParam(cmd, "@FREE", dgv_Main, dbRow, "FREE");
                    AddParam(cmd, "@FREE_SUP", dgv_Main, suppRow, "FREE");
                    AddParam(cmd, "@FREE_USRINT", dgv_Main, promoRow, "FREE");

                    AddParam(cmd, "@PLFOB", dgv_Main, dbRow, "PLFOB");
                    AddParam(cmd, "@PLFOB_SUP", dgv_Main, suppRow, "PLFOB");
                    AddParam(cmd, "@PLFOB_USRINT", dgv_Main, promoRow, "PLFOB");

                    AddParam(cmd, "@NWF", dgv_Main, dbRow, "NWF");
                    AddParam(cmd, "@NWF_SUP", dgv_Main, suppRow, "NWF");
                    AddParam(cmd, "@NWF_USRINT", dgv_Main, promoRow, "NWF");

                    AddParam(cmd, "@NWFR", dgv_Main, dbRow, "NWFR");
                    AddParam(cmd, "@NWFR_SUP", dgv_Main, suppRow, "NWFR");
                    AddParam(cmd, "@NWFR_USRINT", dgv_Main, promoRow, "NWFR");

                    AddParam(cmd, "@PC_PF", dgv_Main, dbRow, "PC_PF");
                    AddParam(cmd, "@PC_PF_SUP", dgv_Main, suppRow, "PC_PF");
                    AddParam(cmd, "@PC_PF_USRINT", dgv_Main, promoRow, "PC_PF");

                    AddParam(cmd, "@PC_PFL", dgv_Main, dbRow, "PC_PFL");
                    AddParam(cmd, "@PC_PFL_SUP", dgv_Main, suppRow, "PC_PFL");
                    AddParam(cmd, "@PC_PFL_USRINT", dgv_Main, promoRow, "PC_PFL");

                    AddParam(cmd, "@PC_RP", dgv_Main, dbRow, "PC_RP");
                    AddParam(cmd, "@PC_RP_SUP", dgv_Main, suppRow, "PC_RP");
                    AddParam(cmd, "@PC_RP_USRINT", dgv_Main, promoRow, "PC_RP");

                    AddParam(cmd, "@PC_PA", dgv_Main, dbRow, "PC_PA");
                    AddParam(cmd, "@PC_PA_SUP", dgv_Main, suppRow, "PC_PA");
                    AddParam(cmd, "@PC_PA_USRINT", dgv_Main, promoRow, "PC_PA");

                    AddParam(cmd, "@PC_PLSRP", dgv_Main, dbRow, "PC_PLSRP");
                    AddParam(cmd, "@PC_PLSRP_SUP", dgv_Main, suppRow, "PC_PLSRP");
                    AddParam(cmd, "@PC_PLSRP_USRINT", dgv_Main, promoRow, "PC_PLSRP");

                    AddParam(cmd, "@PC_LSRP", dgv_Main, dbRow, "PC_LSRP");
                    AddParam(cmd, "@PC_LSRP_SUP", dgv_Main, suppRow, "PC_LSRP");
                    AddParam(cmd, "@PC_LSRP_USRINT", dgv_Main, promoRow, "PC_LSRP");

                    AddParam(cmd, "@PC_PPA2LP", dgv_Main, dbRow, "PC_PPA2LP");
                    AddParam(cmd, "@PC_PPA2LP_SUP", dgv_Main, suppRow, "PC_PPA2LP");
                    AddParam(cmd, "@PC_PPA2LP_USRINT", dgv_Main, promoRow, "PC_PPA2LP");

                    AddParam(cmd, "@PC_LP", dgv_Main, dbRow, "PC_LP");
                    AddParam(cmd, "@PC_LP_SUP", dgv_Main, suppRow, "PC_LP");
                    AddParam(cmd, "@PC_LP_USRINT", dgv_Main, promoRow, "PC_LP");

                    AddParam(cmd, "@PC_PPA2WA", dgv_Main, dbRow, "PC_PPA2WA");
                    AddParam(cmd, "@PC_PPA2WA_SUP", dgv_Main, suppRow, "PC_PPA2WA");
                    AddParam(cmd, "@PC_PPA2WA_USRINT", dgv_Main, promoRow, "PC_PPA2WA");

                    AddParam(cmd, "@PC_WA", dgv_Main, dbRow, "PC_WA");
                    AddParam(cmd, "@PC_WA_SUP", dgv_Main, suppRow, "PC_WA");
                    AddParam(cmd, "@PC_WA_USRINT", dgv_Main, promoRow, "PC_WA");

                    AddParam(cmd, "@PC_PPA2WB", dgv_Main, dbRow, "PC_PPA2WB");
                    AddParam(cmd, "@PC_PPA2WB_SUP", dgv_Main, suppRow, "PC_PPA2WB");
                    AddParam(cmd, "@PC_PPA2WB_USRINT", dgv_Main, promoRow, "PC_PPA2WB");

                    AddParam(cmd, "@PC_WB", dgv_Main, dbRow, "PC_WB");
                    AddParam(cmd, "@PC_WB_SUP", dgv_Main, suppRow, "PC_WB");
                    AddParam(cmd, "@PC_WB_USRINT", dgv_Main, promoRow, "PC_WB");

                    AddParam(cmd, "@PC_PPA2WC", dgv_Main, dbRow, "PC_PPA2WC");
                    AddParam(cmd, "@PC_PPA2WC_SUP", dgv_Main, suppRow, "PC_PPA2WC");
                    AddParam(cmd, "@PC_PPA2WC_USRINT", dgv_Main, promoRow, "PC_PPA2WC");

                    AddParam(cmd, "@PC_WC", dgv_Main, dbRow, "PC_WC");
                    AddParam(cmd, "@PC_WC_SUP", dgv_Main, suppRow, "PC_WC");
                    AddParam(cmd, "@PC_WC_USRINT", dgv_Main, promoRow, "PC_WC");

                    AddParam(cmd, "@PC_PPA2LC", dgv_Main, dbRow, "PC_PPA2LC");
                    AddParam(cmd, "@PC_PPA2LC_SUP", dgv_Main, suppRow, "PC_PPA2LC");
                    AddParam(cmd, "@PC_PPA2LC_USRINT", dgv_Main, promoRow, "PC_PPA2LC");

                    AddParam(cmd, "@PC_LC", dgv_Main, dbRow, "PC_LC");
                    AddParam(cmd, "@PC_LC_SUP", dgv_Main, suppRow, "PC_LC");
                    AddParam(cmd, "@PC_LC_USRINT", dgv_Main, promoRow, "PC_LC");

                    AddParam(cmd, "@PC_PPA2PG", dgv_Main, dbRow, "PC_PPA2PG");
                    AddParam(cmd, "@PC_PPA2PG_SUP", dgv_Main, suppRow, "PC_PPA2PG");
                    AddParam(cmd, "@PC_PPA2PG_USRINT", dgv_Main, promoRow, "PC_PPA2PG");

                    AddParam(cmd, "@PC_PG", dgv_Main, dbRow, "PC_PG");
                    AddParam(cmd, "@PC_PG_SUP", dgv_Main, suppRow, "PC_PG");
                    AddParam(cmd, "@PC_PG_USRINT", dgv_Main, promoRow, "PC_PG");

                    AddParam(cmd, "@PC_PPA2PH", dgv_Main, dbRow, "PC_PPA2PH");
                    AddParam(cmd, "@PC_PPA2PH_SUP", dgv_Main, suppRow, "PC_PPA2PH");
                    AddParam(cmd, "@PC_PPA2PH_USRINT", dgv_Main, promoRow, "PC_PPA2PH");

                    AddParam(cmd, "@PC_PH", dgv_Main, dbRow, "PC_PH");
                    AddParam(cmd, "@PC_PH_SUP", dgv_Main, suppRow, "PC_PH");
                    AddParam(cmd, "@PC_PH_USRINT", dgv_Main, promoRow, "PC_PH");

                    AddParam(cmd, "@PC_PPA2PB", dgv_Main, dbRow, "PC_PPA2PB");
                    AddParam(cmd, "@PC_PPA2PB_SUP", dgv_Main, suppRow, "PC_PPA2PB");
                    AddParam(cmd, "@PC_PPA2PB_USRINT", dgv_Main, promoRow, "PC_PPA2PB");

                    AddParam(cmd, "@PC_PB", dgv_Main, dbRow, "PC_PB");
                    AddParam(cmd, "@PC_PB_SUP", dgv_Main, suppRow, "PC_PB");
                    AddParam(cmd, "@PC_PB_USRINT", dgv_Main, promoRow, "PC_PB");

                    AddParam(cmd, "@PC_PPA2PD", dgv_Main, dbRow, "PC_PPA2PD");
                    AddParam(cmd, "@PC_PPA2PD_SUP", dgv_Main, suppRow, "PC_PPA2PD");
                    AddParam(cmd, "@PC_PPA2P_USRINT", dgv_Main, promoRow, "PC_PPA2PD");

                    AddParam(cmd, "@PC_PD", dgv_Main, dbRow, "PC_PD");
                    AddParam(cmd, "@PC_PD_SUP", dgv_Main, suppRow, "PC_PD");
                    AddParam(cmd, "@PC_PD_USRINT", dgv_Main, promoRow, "PC_PD");

                    AddParam(cmd, "@LPP_AMT", dgv_Main, dbRow, "LPP_AMT");
                    AddParam(cmd, "@LPP_AMT_SUP", dgv_Main, suppRow, "LPP_AMT");
                    AddParam(cmd, "@LPP_AMT_USRINT", dgv_Main, promoRow, "LPP_AMT");

                    AddParam(cmd, "@LPP_REF", dgv_Main, dbRow, "LPP_REF");
                    AddParam(cmd, "@LPP_REF_SUP", dgv_Main, suppRow, "LPP_REF");
                    AddParam(cmd, "@LPP_REF_USRINT", dgv_Main, promoRow, "LPP_REF");

                    AddParam(cmd, "@PC_PPA2PC", dgv_Main, dbRow, "PC_PPA2PC");
                    AddParam(cmd, "@PC_PPA2PC_SUP", dgv_Main, suppRow, "PC_PPA2PC");
                    AddParam(cmd, "@PC_PPA2PC_USRINT", dgv_Main, promoRow, "PC_PPA2PC");

                    AddParam(cmd, "@PC_PC", dgv_Main, dbRow, "PC_PC");
                    AddParam(cmd, "@PC_PC_SUP", dgv_Main, suppRow, "PC_PC");
                    AddParam(cmd, "@PC_PC_USRINT", dgv_Main, promoRow, "PC_PC");

                    AddParam(cmd, "@Claim", dgv_Main, dbRow, "Claim");
                    AddParam(cmd, "@Claim_SUP", dgv_Main, suppRow, "Claim");
                    AddParam(cmd, "@Claim_USRINT", dgv_Main, promoRow, "Claim");

                    AddParam(cmd, "@ClaimK", dgv_Main, dbRow, "ClaimK");
                    AddParam(cmd, "@ClaimK_SUP", dgv_Main, suppRow, "ClaimK");
                    AddParam(cmd, "@Claim_USRINT", dgv_Main, promoRow, "ClaimK");

                    AddParam(cmd, "@Remarks", dgv_Main, dbRow, "Remarks");
                    AddParam(cmd, "@Remarks_SUP", dgv_Main, suppRow, "Remarks");
                    AddParam(cmd, "@Remarks_USRINT", dgv_Main, promoRow, "Remarks");



                    // Add more fields as needed...

                    // WHERE clause identifiers
                    AddParam(cmd, "@DocID", dgv_Main, dbRow, "DocID"); // or use passed docIdToUse
                    AddParam(cmd, "@PROD_C", dgv_Main, dbRow, "PROD_C");

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ProdInstcs Error adding DocID parameter: " + ex.Message);
                    return;
                }
                
            }
        }

        private void AddParam(OleDbCommand cmd, string paramName, DataGridView dgv_Main, int rowIndex, string columnName)
        {
            object val = dgv_Main.Rows[rowIndex].Cells[columnName].Value;
            cmd.Parameters.AddWithValue(paramName, val ?? DBNull.Value);
        }
    }


    #region ************************** DataGridView Cell Edit Logic **************************
public class AutoComputeRate
    {
        private readonly DataGridView dgv_main;

        public AutoComputeRate(DataGridView dgv)
        {
            this.dgv_main = dgv ?? throw new ArgumentNullException(nameof(dgv));
        }

        public class ColumnMapping
        {
            public string BaseName { get; set; }
            public string ValueName { get; set; }
            public string RateName { get; set; }
        }

        public readonly List<ColumnMapping> columnMappings = new List<ColumnMapping>
    {
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_LSRP", RateName = "PC_PLSRP" },
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_LP",   RateName = "PC_PPA2LP" },
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_WA",   RateName = "PC_PPA2WA" },
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_WB",   RateName = "PC_PPA2WB" },
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_WC",   RateName = "PC_PPA2WC" },
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_LC",   RateName = "PC_PPA2LC" },
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_PG",   RateName = "PC_PPA2PG" },
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_PH",   RateName = "PC_PPA2PH" },
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_PB",   RateName = "PC_PPA2PB" },
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_PD",   RateName = "PC_PPA2PD" },
        new ColumnMapping { BaseName = "PC_PA", ValueName = "PC_PC",   RateName = "PC_PPA2PC" }
    };

        public void UpdateDependentValues(int rowIndex, int editedColumnIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgv_main.Rows.Count)
                return;

            var row = dgv_main.Rows[rowIndex];

            foreach (var mapping in columnMappings)
            {
                // Resolve column indices from names
                int baseIndex = dgv_main.Columns[mapping.BaseName].Index;
                int valueIndex = dgv_main.Columns[mapping.ValueName].Index;
                int rateIndex = dgv_main.Columns[mapping.RateName].Index;

                bool isBaseEdited = editedColumnIndex == baseIndex;
                bool isValueEdited = editedColumnIndex == valueIndex;
                bool isRateEdited = editedColumnIndex == rateIndex;

                if (!isBaseEdited && !isValueEdited && !isRateEdited)
                    continue;

                bool hasBase = decimal.TryParse(row.Cells[baseIndex]?.Value?.ToString(), out decimal baseValue);
                bool hasValue = decimal.TryParse(row.Cells[valueIndex]?.Value?.ToString(), out decimal value);
                bool hasRate = decimal.TryParse(row.Cells[rateIndex]?.Value?.ToString(), out decimal rate);

                if (!hasBase || baseValue == 0)
                    continue;

                if (isValueEdited && hasValue)
                {
                    // Value changed → update Rate
                    decimal newRate = Math.Round((value / baseValue) * 100, 2);
                    SetCellValue(row, rateIndex, newRate);
                }
                else if (isRateEdited && hasRate)
                {
                    // Rate changed → update Value
                    decimal newValue = Math.Round((baseValue * rate) / 100, 2);
                    SetCellValue(row, valueIndex, newValue);
                }
                else if (isBaseEdited && hasRate)
                {
                    // Base changed → update Value using current Rate
                    decimal newValue = Math.Round((baseValue * rate) / 100, 2);
                    SetCellValue(row, valueIndex, newValue);
                }
            }
        }

        // Helper method to handle setting values safely
        private void SetCellValue(DataGridViewRow row, int columnIndex, object value)
        {
            if (row.DataBoundItem is DataRowView drv)
                drv[columnIndex] = value;
            else
                row.Cells[columnIndex].Value = value;
        }

        public void dgv_Main_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgv_main.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgv_main.EndEdit();

                UpdateDependentValues(e.RowIndex, e.ColumnIndex);
            }
        }
    }


    #endregion


}
