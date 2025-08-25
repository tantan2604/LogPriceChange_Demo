using PriceMatrix.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace LogPriceChange0._1
{

    public partial class ctrLogPriceChange : UserControl
    {

        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Desktop\CameraHaus\LogPriceChange_Demo\pricematrix.accdb;";
        private static Dictionary<string, int> lastNumbers = new Dictionary<string, int>();
        private DataGridViewRow rightClickedRow;
        private string _docId;
        private DataTable mainTable = new DataTable();
        private AutoComputeRate autoRate;
        private string selectedId;
        private int _idValue;
        private string _promoType;

        private void InitializeMainTable()
        {
            string[] columns = {
        "ID","DocID", "PROD_C", "PROD_N", "FREE", "PLFOB", "NWF", "NWFR", "PC_PF", "PC_PFL", "PC_RP",
        "PC_PA", "PC_PLSRP", "PC_LSRP", "PC_PPA2LP", "PC_LP", "PC_PPA2WA", "PC_WA", "PC_PPA2WB", "PC_WB",
        "PC_PPA2WC", "PC_WC", "PC_PPA2LC", "PC_LC", "PC_PPA2PG", "PC_PG", "PC_PPA2PH", "PC_PH",
        "PC_PPA2PB", "PC_PB", "PC_PPA2PD", "PC_PD", "LPP_AMT", "LPP_REF", "PC_PPA2PC", "PC_PC",
        "Claim", "ClaimK", "Remarks"
         };

            foreach (string col in columns)
                mainTable.Columns.Add(col);

            dgv_Main.DataSource = mainTable;

            foreach (DataGridViewColumn column in dgv_Main.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            }

            typeof(DataGridView).InvokeMember("DoubleBuffered",
              System.Reflection.BindingFlags.NonPublic |
              System.Reflection.BindingFlags.Instance |
              System.Reflection.BindingFlags.SetProperty,
              null, dgv_Main, new object[] { true });
            lpc_dgv_searchbycode.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            autoRate = new AutoComputeRate(dgv_Main);
            dgv_Main.CellEndEdit += autoRate.dgv_Main_CellEndEdit;

           
        }

        private void ClearForm()
        {
            foreach (DataGridViewRow row in dgv_Main.Rows)
            {
                if (row.HeaderCell.Value?.ToString() == "DB Price" ||
                    row.HeaderCell.Value?.ToString() == "Supplier Price" ||
                    row.HeaderCell.Value?.ToString() == "Promo Price")
                {
                    dgv_Main.Rows.Remove(row);
                }
            }
            lpc_tb_searchbycode.Clear();
            lpc_tb_promotitle.Clear();
            lpc_tb_supplier.Clear();
            lpc_rbtn_permanent.Checked = true;
            lpc_dtp_startdate.Value = DateTime.Now;
            lpc_dtp_enddate.Value = DateTime.Now.AddDays(7);
            lpc_dtp_memodate.Value = DateTime.Now;
            mainTable.Clear();
            tb_docID.Clear();

        }

        private string GetFullName(string username)
        {
            string fullName = string.Empty;
            string query = "SELECT Lastname, Firstname FROM tbl_employee WHERE Username = ?";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
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
        }

        private void RemoveGroupRowsByProdCode(string prodCode)
        {
            // Find all rows in mainTable with matching PROD_C and header "DB Price"
            var rowsToRemove = mainTable.AsEnumerable()
                .Where(r => r.Table.Columns.Contains("PROD_C") &&
                            r["PROD_C"].ToString() == prodCode)
                .ToList();

            foreach (var row in rowsToRemove)
            {
                mainTable.Rows.Remove(row);
            }
        }

        private DataRow LoadBillpDataToRow(int id)
        {

            string query = @"SELECT  ID, PROD_C, PROD_N, FREE, PLFOB, NWF, NWFR, PC_PF, PC_PFL, PC_RP, PC_PA, PC_PLSRP, PC_LSRP, 
                         PC_PPA2LP, PC_LP, PC_PPA2WA, PC_WA, PC_PPA2WB, PC_WB, PC_PPA2WC, PC_WC, PC_PPA2LC, PC_LC, 
                         PC_PPA2PG, PC_PG, PC_PPA2PH, PC_PH, PC_PPA2PB, PC_PB, PC_PPA2PD, PC_PD, 
                         LPP_AMT, LPP_REF, PC_PPA2PC, PC_PC
                         FROM tbl_billptmp 
                         WHERE ID = ?";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("?", id);

                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    connection.Open();
                    adapter.Fill(dt);
                    connection.Close();

                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }



        }

        private string GenerateNewDocID()
        {
            string prefix = DateTime.Now.ToString("yyyyMM"); // e.g. "202508"
            string newDocID = "";
            string maxDocID = null;

            // Query to find max DocID for this year-month
            string query = "SELECT MAX(DocID) FROM tbl_logpricechange WHERE DocID LIKE ?";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@prefix", prefix + "%");
                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        maxDocID = result.ToString();

                    }
                    if (maxDocID == null)
                    {
                        // No record yet for this month, start at 000001
                        newDocID = prefix + "-000001";
                    }
                    else
                    {
                        // Extract the last 6 digits from maxDocID
                        string lastSix = maxDocID.Substring(maxDocID.Length - 6);
                        int lastNumber = int.Parse(lastSix);

                        int newNumber = lastNumber + 1;
                        string newNumberStr = newNumber.ToString("D6"); // pad with zeros

                        newDocID = prefix + "-" + newNumberStr;
                    }

                    return newDocID;
                }
            }
        }

        private DataTable LoadlpcUpdateAll(string docID)
        {
            string query = "SELECT * FROM tbl_logpricechange WHERE DocID = ?";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("?", docID);

                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    connection.Open();
                    adapter.Fill(dt);
                    connection.Close();

                    return dt;
                }
            }

        }

        private void Add(string docStatus)
        {

            string docIdToUse = tb_docID.Text;
            string promoName = lpc_tb_promotitle.Text.ToString();
            string promoType = lpc_rbtn_permanent.Checked ? "Permanent" :
                               lpc_rbtn_temporary.Checked ? "Temporary" : null; ;
            DateTime startDate = lpc_dtp_startdate.Value;
            DateTime endDate = lpc_dtp_enddate.Value;
            string loggedUser = Environment.UserName;
            int rowCount = dgv_Main.AllowUserToAddRows ? dgv_Main.Rows.Count - 1 : dgv_Main.Rows.Count;
            DateTime memoDate = lpc_dtp_memodate.Value;
            string supplier = lpc_tb_supplier.Text.Trim();
            string insertQuery = @"
            INSERT INTO tbl_logpricechange (
                DocID, DocStatus, CreatedBy, CreatedDate, ApprovedBy, ApprovedDate, TDate, Supplier, PromoTitle, StartDate, EndDate, Promotype, PROD_C, PROD_N, FREE, FREE_SUP, FREE_USRINT, PLFOB, PLFOB_SUP, PLFOB_USRINT, NWF, NWF_SUP, NWF_USRINT, NWFR, NWFR_SUP, NWFR_USRINT, PC_PF, PC_PF_SUP, PC_PF_USRINT, PC_PFL, PC_PFL_SUP, PC_PFL_USRINT, PC_RP, PC_RP_SUP, PC_RP_USRINT, PC_PA, PC_PA_SUP, PC_PA_USRINT, PC_PLSRP, PC_PLSRP_SUP, PC_PLSRP_USRINT, PC_LSRP, PC_LSRP_SUP, PC_LSRP_USRINT, PC_PPA2LP, PC_PPA2LP_SUP, PC_PPA2LP_USRINT, PC_LP, PC_LP_SUP, PC_LP_USRINT, PC_PPA2WA, PC_PPA2WA_SUP, PC_PPA2WA_USRINT, PC_WA, PC_WA_SUP, PC_WA_USRINT, PC_PPA2WB, PC_PPA2WB_SUP, PC_PPA2WB_USRINT, PC_WB, PC_WB_SUP, PC_WB_USRINT, PC_PPA2WC, PC_PPA2WC_SUP, PC_PPA2WC_USRINT, PC_WC, PC_WC_SUP, PC_WC_USRINT, PC_PPA2LC, PC_PPA2LC_SUP, PC_PPA2LC_USRINT, PC_LC, PC_LC_SUP, PC_LC_USRINT, PC_PPA2PG, PC_PPA2PG_SUP, PC_PPA2PG_USRINT, PC_PG, PC_PG_SUP, PC_PG_USRINT, PC_PPA2PH, PC_PPA2PH_SUP, PC_PPA2PH_USRINT, PC_PH, PC_PH_SUP, PC_PH_USRINT, PC_PPA2PB, PC_PPA2PB_SUP, PC_PPA2PB_USRINT, PC_PB, PC_PB_SUP, PC_PB_USRINT, PC_PPA2PD, PC_PPA2PD_SUP, PC_PPA2P_USRINT, PC_PD, PC_PD_SUP, PC_PD_USRINT, LPP_AMT, LPP_AMT_SUP, LPP_AMT_USRINT, LPP_REF, LPP_REF_SUP, LPP_REF_USRINT, PC_PPA2PC, PC_PPA2PC_SUP, PC_PPA2PC_USRINT, PC_PC, PC_PC_SUP, PC_PC_USRINT, Claim, Claim_SUP, Claim_USRINT, ClaimK, ClaimK_SUP, ClaimK_USRINT, Remarks, Remarks_SUP, Remarks_USRINT)
            VALUES
                (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";


            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbTransaction tx = connection.BeginTransaction();


                var inserter = new ProductInserter(connection, tx, insertQuery);

                try
                {
                    int validRowCount = 0;
                    for (int i = 0; i < rowCount; i++)
                    {
                        var prodCode = dgv_Main.Rows[i].Cells["PROD_C"].Value?.ToString();
                        if (!string.IsNullOrWhiteSpace(prodCode))
                        {
                            validRowCount++;
                        }
                    }

                    for (int i = 0; i < validRowCount; i += 3)
                    {
                        string prodCode = dgv_Main.Rows[i].Cells["PROD_C"].Value?.ToString();

                        if (!string.IsNullOrWhiteSpace(prodCode))
                        {
                            // ✅ Check if product already exists in DB
                            if (!ProductExists(connection, tx, prodCode, docIdToUse))
                            {
                                inserter.InsertProductRow(
                                    docIdToUse,
                                    i,
                                    i + 1,
                                    i + 2,
                                    promoName,
                                    promoType,
                                    startDate,
                                    endDate,
                                    loggedUser,
                                    docStatus,
                                    memoDate,
                                    supplier,
                                    dgv_Main
                                );
                            }
                            else
                            {
                                Console.WriteLine($"meron na Skipped duplicate PROD_C: {prodCode}");
                            }
                        }
                    }

                    MessageBox.Show($"Valid rows with PROD_C: {validRowCount}");
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    MessageBox.Show("Insert failed: " + ex.Message);
                }

            }
        }

        private bool ProductExists(OleDbConnection conn, OleDbTransaction tx, string prodCode, string docId)
        {
            // 🔹 OPTION A: Check globally (all documents)
            string checkQuery = "SELECT COUNT(*) FROM tbl_logpricechange WHERE PROD_C = ?";

            // 🔹 OPTION B: Check only within the same DocID
            // string checkQuery = "SELECT COUNT(*) FROM tbl_logpricechange WHERE PROD_C = ? AND DocID = ?";

            using (OleDbCommand cmd = new OleDbCommand(checkQuery, conn, tx))
            {
                cmd.Parameters.AddWithValue("?", prodCode);

                // If using OPTION B, uncomment this:
                // cmd.Parameters.AddWithValue("?", docId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }


        private void PopulateHeaderFields(string docID)
        {
            DataTable dt = LoadlpcUpdateAll(docID);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No data found.");
                return;
            }

            DataRow row = dt.Rows[0];

            if (DateTime.TryParse(row["Tdate"]?.ToString(), out DateTime tdate))
                lpc_dtp_memodate.Value = tdate;

            lpc_tb_supplier.Text = row["Supplier"]?.ToString();
            lpc_tb_promotitle.Text = row["PromoTitle"]?.ToString();

            string promoType = row["PromoType"]?.ToString();
            lpc_rbtn_permanent.Checked = promoType == "Permanent";
            lpc_rbtn_temporary.Checked = promoType == "Temporary";

            if (DateTime.TryParse(row["StartDate"]?.ToString(), out DateTime startDate))
                lpc_dtp_startdate.Value = startDate;

            if (DateTime.TryParse(row["EndDate"]?.ToString(), out DateTime endDate))
                lpc_dtp_enddate.Value = endDate;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void Update(string docStatus)
        {
            string docIdToUse = selectedId;
            string promoName = lpc_tb_promotitle.Text.Trim();
            string promoType = lpc_rbtn_permanent.Checked ? "Permanent" :
                               lpc_rbtn_temporary.Checked ? "Temporary" : null;
            DateTime startDate = lpc_dtp_startdate.Value.Date;
            DateTime endDate = lpc_dtp_enddate.Value.Date;
            string loggedUser = Environment.UserName;
            DateTime memoDate = lpc_dtp_memodate.Value.Date;
            string supplier = lpc_tb_supplier.Text.Trim();
            int rowCount = dgv_Main.AllowUserToAddRows ? dgv_Main.Rows.Count - 1 : dgv_Main.Rows.Count;
            string updateQuery = @"
                                     UPDATE tbl_logpricechange SET
                                        DocStatus = ?,
                                        PromoTitle = ?, 
                                        PromoType = ?, 
                                        StartDate = ?, 
                                        EndDate = ?, 
                                        CreatedBy = ?, 
                                        CreatedDate = ?, 
                                        TDate = ?,
                                        Supplier = ?,
                                        PROD_N = ?,
                                        FREE = ?,
                                        FREE_SUP = ?,
                                        FREE_USRINT = ?,
                                        PLFOB = ?,
                                        PLFOB_SUP = ?,
                                        PLFOB_USRINT = ?,
                                        NWF = ?,
                                        NWF_SUP = ?,
                                        NWF_USRINT = ?,
                                        NWFR = ?,
                                        NWFR_SUP = ?,
                                        NWFR_USRINT = ?,
                                        PC_PF = ?,
                                        PC_PF_SUP = ?,
                                        PC_PF_USRINT = ?,
                                        PC_PFL = ?,
                                        PC_PFL_SUP = ?,
                                        PC_PFL_USRINT = ?,
                                        PC_RP = ?,
                                        PC_RP_SUP = ?,
                                        PC_RP_USRINT = ?,
                                        PC_PA = ?,
                                        PC_PA_SUP = ?,
                                        PC_PA_USRINT = ?,
                                        PC_PLSRP = ?,
                                        PC_PLSRP_SUP = ?,
                                        PC_PLSRP_USRINT = ?,
                                        PC_LSRP = ?,
                                        PC_LSRP_SUP = ?,
                                        PC_LSRP_USRINT = ?,
                                        PC_PPA2LP = ?,
                                        PC_PPA2LP_SUP = ?,
                                        PC_PPA2LP_USRINT = ?,
                                        PC_LP = ?,
                                        PC_LP_SUP = ?,
                                        PC_LP_USRINT = ?,
                                        PC_PPA2WA = ?,
                                        PC_PPA2WA_SUP = ?,
                                        PC_PPA2WA_USRINT = ?,
                                        PC_WA = ?,
                                        PC_WA_SUP = ?,
                                        PC_WA_USRINT = ?,
                                        PC_PPA2WB = ?,
                                        PC_PPA2WB_SUP = ?,
                                        PC_PPA2WB_USRINT = ?,
                                        PC_WB = ?,
                                        PC_WB_SUP = ?,
                                        PC_WB_USRINT = ?,
                                        PC_PPA2WC = ?,
                                        PC_PPA2WC_SUP = ?,
                                        PC_PPA2WC_USRINT = ?,
                                        PC_WC = ?,
                                        PC_WC_SUP = ?,
                                        PC_WC_USRINT = ?,
                                        PC_PPA2LC = ?,
                                        PC_PPA2LC_SUP = ?,
                                        PC_PPA2LC_USRINT = ?,
                                        PC_LC = ?,
                                        PC_LC_SUP = ?,
                                        PC_LC_USRINT = ?,
                                        PC_PPA2PG = ?,
                                        PC_PPA2PG_SUP = ?,
                                        PC_PPA2PG_USRINT = ?,
                                        PC_PG = ?,
                                        PC_PG_SUP = ?,
                                        PC_PG_USRINT = ?,
                                        PC_PPA2PH = ?,
                                        PC_PPA2PH_SUP = ?,
                                        PC_PPA2PH_USRINT = ?,
                                        PC_PH = ?,
                                        PC_PH_SUP = ?,
                                        PC_PH_USRINT = ?,
                                        PC_PPA2PB = ?,
                                        PC_PPA2PB_SUP = ?,
                                        PC_PPA2PB_USRINT = ?,
                                        PC_PB = ?,
                                        PC_PB_SUP = ?,
                                        PC_PB_USRINT = ?,
                                        PC_PPA2PD = ?,
                                        PC_PPA2PD_SUP = ?,
                                        PC_PPA2P_USRINT = ?,
                                        PC_PD = ?,
                                        PC_PD_SUP = ?,
                                        PC_PD_USRINT = ?,
                                        LPP_AMT = ?,
                                        LPP_AMT_SUP = ?,
                                        LPP_AMT_USRINT = ?,
                                        LPP_REF = ?,
                                        LPP_REF_SUP = ?,
                                        LPP_REF_USRINT = ?,
                                        PC_PPA2PC = ?,
                                        PC_PPA2PC_SUP = ?,
                                        PC_PPA2PC_USRINT = ?,
                                        PC_PC = ?,
                                        PC_PC_SUP = ?,
                                        PC_PC_USRINT = ?,
                                        Claim = ?,
                                        Claim_SUP = ?,
                                        Claim_USRINT = ?,
                                        ClaimK = ?,
                                        ClaimK_SUP = ?,
                                        ClaimK_USRINT = ?,
                                        Remarks = ?,
                                        Remarks_SUP = ?,
                                        Remarks_USRINT = ?
                                    WHERE DocID = ? AND PROD_C = ?;";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbTransaction tx = connection.BeginTransaction();

                try
                {
                    var updater = new ProductUpdater(connection, tx, updateQuery);

                    for (int i = 0; i < rowCount; i += 3)
                    {
                        if (i + 2 >= rowCount) break; // prevent out of range

                        string prodCode = dgv_Main.Rows[i].Cells["PROD_C"].Value?.ToString();
                        if (!string.IsNullOrWhiteSpace(prodCode))
                        {
                            updater.UpdateProductRow(
                                docIdToUse,
                                i,
                                i + 1,
                                i + 2,
                                promoName,
                                promoType,
                                startDate,
                                endDate,
                                loggedUser,
                                docStatus,
                                memoDate,
                                supplier,
                                dgv_Main
                            );
                        }
                    }

                    tx.Commit();
                    MessageBox.Show("Draft updated successfully.\nDocID: " + docIdToUse);
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    MessageBox.Show("Update failed: " + ex.Message);
                }
            }
        }

        public ctrLogPriceChange()
        {
            InitializeComponent();
            InitializeMainTable();
        }

        private void ctrLogPriceChange_Load(object sender, EventArgs e)
        {

        }

        public ctrLogPriceChange(string docId) : this()
        {
            _docId = docId;
            tb_docID.Text = _docId;
            dgv_Main.Columns["ID"].Visible = false;
           
        }

        private void Insert(string docStatus)
        {

            string docIdToUse = GenerateNewDocID();
            string promoName = lpc_tb_promotitle.Text.ToString();
            string promoType = lpc_rbtn_permanent.Checked ? "Permanent" :
                               lpc_rbtn_temporary.Checked ? "Temporary" : null; ;
            DateTime startDate = lpc_dtp_startdate.Value;
            DateTime endDate = lpc_dtp_enddate.Value;
            string loggedUser = GetFullName(UserSession.Username); 
            int rowCount = dgv_Main.AllowUserToAddRows ? dgv_Main.Rows.Count - 1 : dgv_Main.Rows.Count;
            DateTime memoDate = lpc_dtp_memodate.Value;
            string supplier = lpc_tb_supplier.Text.Trim();
            string insertQuery = @"
            INSERT INTO tbl_logpricechange (
                DocID, DocStatus, CreatedBy, CreatedDate, ApprovedBy, ApprovedDate, TDate, Supplier, PromoTitle, StartDate, EndDate, Promotype, PROD_C, PROD_N, FREE, FREE_SUP, FREE_USRINT, PLFOB, PLFOB_SUP, PLFOB_USRINT, NWF, NWF_SUP, NWF_USRINT, NWFR, NWFR_SUP, NWFR_USRINT, PC_PF, PC_PF_SUP, PC_PF_USRINT, PC_PFL, PC_PFL_SUP, PC_PFL_USRINT, PC_RP, PC_RP_SUP, PC_RP_USRINT, PC_PA, PC_PA_SUP, PC_PA_USRINT, PC_PLSRP, PC_PLSRP_SUP, PC_PLSRP_USRINT, PC_LSRP, PC_LSRP_SUP, PC_LSRP_USRINT, PC_PPA2LP, PC_PPA2LP_SUP, PC_PPA2LP_USRINT, PC_LP, PC_LP_SUP, PC_LP_USRINT, PC_PPA2WA, PC_PPA2WA_SUP, PC_PPA2WA_USRINT, PC_WA, PC_WA_SUP, PC_WA_USRINT, PC_PPA2WB, PC_PPA2WB_SUP, PC_PPA2WB_USRINT, PC_WB, PC_WB_SUP, PC_WB_USRINT, PC_PPA2WC, PC_PPA2WC_SUP, PC_PPA2WC_USRINT, PC_WC, PC_WC_SUP, PC_WC_USRINT, PC_PPA2LC, PC_PPA2LC_SUP, PC_PPA2LC_USRINT, PC_LC, PC_LC_SUP, PC_LC_USRINT, PC_PPA2PG, PC_PPA2PG_SUP, PC_PPA2PG_USRINT, PC_PG, PC_PG_SUP, PC_PG_USRINT, PC_PPA2PH, PC_PPA2PH_SUP, PC_PPA2PH_USRINT, PC_PH, PC_PH_SUP, PC_PH_USRINT, PC_PPA2PB, PC_PPA2PB_SUP, PC_PPA2PB_USRINT, PC_PB, PC_PB_SUP, PC_PB_USRINT, PC_PPA2PD, PC_PPA2PD_SUP, PC_PPA2P_USRINT, PC_PD, PC_PD_SUP, PC_PD_USRINT, LPP_AMT, LPP_AMT_SUP, LPP_AMT_USRINT, LPP_REF, LPP_REF_SUP, LPP_REF_USRINT, PC_PPA2PC, PC_PPA2PC_SUP, PC_PPA2PC_USRINT, PC_PC, PC_PC_SUP, PC_PC_USRINT, Claim, Claim_SUP, Claim_USRINT, ClaimK, ClaimK_SUP, ClaimK_USRINT, Remarks, Remarks_SUP, Remarks_USRINT)
            VALUES
                (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";



            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbTransaction tx = connection.BeginTransaction();

                var inserter = new ProductInserter(connection, tx, insertQuery);

                try
                {
                    int validRowCount = 0;
                    for (int i = 0; i < rowCount; i++)
                    {
                        var prodCode = dgv_Main.Rows[i].Cells["PROD_C"].Value?.ToString();
                        if (!string.IsNullOrWhiteSpace(prodCode))
                        {
                            validRowCount++;
                        }
                    }

                    for (int i = 0; i < validRowCount; i += 3)
                    {
                        inserter.InsertProductRow(
                            docIdToUse,
                            i,
                            i + 1,
                            i + 2,
                            promoName,
                            promoType,
                            startDate,
                            endDate,
                            loggedUser,
                            docStatus,
                            memoDate,
                            supplier,
                            dgv_Main
                        );
                    }


                    MessageBox.Show($"Valid rows with PROD_C: {validRowCount}");
                    tx.Commit();

                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    MessageBox.Show("Insert failed: " + ex.Message);
                }
            }
        }

        private void LoadSearchInsert()
        {
            string searchCode = lpc_tb_searchbycode.Text.Trim();
            string query = @"SELECT ID, PROD_C,PROD_CN, PROD_BRAND, PROD_N 
                         FROM tbl_billptmp
                         WHERE PROD_C LIKE ? OR PROD_BRAND LIKE ? OR PROD_N LIKE ?";


            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, connection))
                {
                    string likeParam = "%" + searchCode + "%";

                    cmd.Parameters.AddWithValue("?", likeParam); // PROD_C
                    cmd.Parameters.AddWithValue("?", likeParam); // PROD_BRAND
                    cmd.Parameters.AddWithValue("?", likeParam); // PROD_N

                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    connection.Open();
                    adapter.Fill(dt);
                    connection.Close();

                    lpc_dgv_searchbycode.DataSource = dt;
                    if (lpc_dgv_searchbycode.Columns.Contains("Select"))
                    {
                        lpc_dgv_searchbycode.Columns.Remove("Select");
                    }

                    DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn
                    {
                        Name = "Select",
                        HeaderText = "✓",
                        Width = 40
                    };
                    lpc_dgv_searchbycode.Columns.Insert(0, chk);



                    if (lpc_dgv_searchbycode.Columns.Contains("ID"))
                        lpc_dgv_searchbycode.Columns["ID"].Visible = false;

                    foreach (DataGridViewColumn column in lpc_dgv_searchbycode.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;


                    }
                }
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
            else 
            {
                lpc_dgv_searchbycode.Visible = true;
                LoadSearchInsert();
            }
          
        }

        #region

        private void btnlpcsubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb_docID.Text))
            {
                Update("For Approval");
                MessageBox.Show("Update sucessfully! for approval");
                ClearForm();
            }
            else
            {
                Insert("For Approval");
                MessageBox.Show("Submit sucessfully!");
                ClearForm();
            }
        }

        private void btn_draft_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb_docID.Text))
            {
                Update("Draft");
                Add("Draft");
                MessageBox.Show("Draft updated successfully.");
                
            }
            else
            {
                Insert("Draft");
                MessageBox.Show("Draft saved successfully.");
            }

            ClearForm();
        }

        #endregion
        private void lpc_dgv_searchbycode_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (lpc_dgv_searchbycode.IsCurrentCellDirty)
            {
                lpc_dgv_searchbycode.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
   
        private void lpc_dgv_searchbycode_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = lpc_dgv_searchbycode.Columns[e.ColumnIndex];

                if (column.Name == "Select")
                {
                    DataGridViewRow selectedRow = lpc_dgv_searchbycode.Rows[e.RowIndex];
                    object value = selectedRow.Cells["Select"].Value;

                    bool isChecked = false;

                    if (value != null && bool.TryParse(value.ToString(), out bool parsed))
                    {
                        isChecked = parsed;
                    }

                    string prodCode = selectedRow.Cells["PROD_C"].Value?.ToString();
                    if (string.IsNullOrEmpty(prodCode)) return;

                    if (isChecked)
                    {
                        if (int.TryParse(selectedRow.Cells["ID"].Value?.ToString(), out int id))
                        {
                            _idValue = id;
                            selectedId = _idValue.ToString();

                            DataRow dbRow = LoadBillpDataToRow(_idValue);
                            if (dbRow == null)
                            {
                                MessageBox.Show("Data not found.");
                                return;
                            }

                            // Check if product already exists
                            bool alreadyExists = dgv_Main.Rows
                                .Cast<DataGridViewRow>()
                                .Any(r => r.HeaderCell.Value?.ToString() == "DB Price" &&
                                          r.Cells["PROD_C"].Value?.ToString() == dbRow["PROD_C"].ToString());

                            if (alreadyExists)
                            {
                                var confirm = MessageBox.Show("This product already exists. Remove the 3 rows?", "Confirm", MessageBoxButtons.YesNo);
                                if (confirm == DialogResult.Yes)
                                {
                                    for (int i = 0; i < dgv_Main.Rows.Count; i++)
                                    {
                                        if (dgv_Main.Rows[i].HeaderCell.Value?.ToString() == "DB Price" &&
                                            dgv_Main.Rows[i].Cells["PROD_C"].Value?.ToString() == dbRow["PROD_C"].ToString())
                                        {
                                            RemoveGroupRowsByProdCode(prodCode);
                                            selectedRow.Cells["Select"].Value = false;
                                            return;
                                        }
                                    }
                                }
                                else return;
                            }

                            // Insert 3 rows
                            dgv_Main.SuspendLayout();

                            int startRowIndex = mainTable.Rows.Count;

                            DataRow dbPriceRow = mainTable.NewRow();
                            foreach (DataColumn col in mainTable.Columns)
                            {
                                if (dbRow.Table.Columns.Contains(col.ColumnName))
                                    dbPriceRow[col.ColumnName] = dbRow[col.ColumnName];
                            }

                            mainTable.Rows.Add(dbPriceRow);
                            DataRow supplierRow = mainTable.NewRow();
                            supplierRow["PROD_C"] = dbRow["PROD_C"];
                            supplierRow["PROD_N"] = dbRow["PROD_N"];
                            mainTable.Rows.Add(supplierRow);

                            DataRow promoRow = mainTable.NewRow();
                            promoRow["PROD_C"] = dbRow["PROD_C"];
                            promoRow["PROD_N"] = dbRow["PROD_N"];
                            mainTable.Rows.Add(promoRow);



                            dgv_Main.RowHeadersWidth = 200;
                            dgv_Main.ResumeLayout();
                            
                        }
                    }
                    else
                    {
                        // Remove rows
                        for (int i = 0; i < dgv_Main.Rows.Count; i++)
                        {
                            if (dgv_Main.Rows[i].HeaderCell.Value?.ToString() == "Db Price" &&
                                dgv_Main.Rows[i].Cells["PROD_C"].Value?.ToString() == prodCode)
                            {
                                RemoveGroupRowsByProdCode(prodCode);
                                break;
                            }
                            
                        }
                    }
                }
            
                dgv_Main.Columns["ID"].Visible = false;
                dgv_Main.Columns["DocID"].Visible = false;
            }


        }

        private void lpc_rbtn_permanent_CheckedChanged(object sender, EventArgs e)
        {
            if (lpc_rbtn_permanent.Checked)
            {
                _promoType = "Permanent";
                lpc_dtp_enddate.Enabled = false;
            }
            else if (lpc_rbtn_temporary.Checked)
            {
                _promoType = "Temporary";
                lpc_dtp_enddate.Enabled = true;
            }
        }

        private void tb_docID_TextChanged(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(tb_docID.Text))
            {
                tb_docID.Visible = false;
                return;
            }
            else
            {
                tb_docID.Visible = true;
            }

            if (string.IsNullOrEmpty(_docId))
            {
              
                return;
            }

            var selectedDocID = _docId;
            MessageBox.Show(selectedDocID);

            PopulateHeaderFields(selectedDocID);
            DataTable dbRows = LoadlpcUpdateAll(selectedDocID);

            if (dbRows.Rows.Count == 0)
            {
                MessageBox.Show("No products found for this DocID.");
                return;
            }

            foreach (DataRow dbRow in dbRows.Rows)
            {
        
                // ✅ Insert 3 grouped rows
                dgv_Main.SuspendLayout();

                int startRowIndex = mainTable.Rows.Count;
                string[] headerNames = { "Normal", "Supplier Price", "Promo Price" };

                for (int i = 0; i < 3; i++)
                {
                    DataRow newRow = mainTable.NewRow();

                    foreach (DataColumn col in mainTable.Columns)
                    {
                        string colName = col.ColumnName;

                        if (i == 0 && dbRow.Table.Columns.Contains(colName) &&
                            !colName.EndsWith("_SUP") && !colName.EndsWith("_USRINT"))
                        {
                            newRow[colName] = dbRow[colName];
                        }
                        else if (i == 1 && dbRow.Table.Columns.Contains(colName + "_SUP"))
                        {
                            newRow[colName] = dbRow[colName + "_SUP"];
                        }
                        else if (i == 2 && dbRow.Table.Columns.Contains(colName + "_USRINT"))
                        {
                            newRow[colName] = dbRow[colName + "_USRINT"];
                        }
                    }

                    newRow["PROD_C"] = dbRow["PROD_C"];
                    newRow["PROD_N"] = dbRow["PROD_N"];
                    newRow["DocID"] = dbRow["DocID"];

                    mainTable.Rows.Add(newRow);

                    // ✅ Assign header here
                    dgv_Main.Rows[startRowIndex + i].HeaderCell.Value = headerNames[i];
                }

                dgv_Main.RowHeadersWidth = 200;
                dgv_Main.ResumeLayout();
            }
        }

        private void dgv_Main_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            for (int i = 0; i < dgv_Main.Rows.Count; i += 3)
            {
                dgv_Main.Rows[i].ReadOnly = true;
                dgv_Main.Rows[i].DefaultCellStyle.BackColor = Color.LightCoral;

            }
            if (e.RowIndex >= 0)
            {
                string[] headerNames = { "Db Price", "Supplier Price", "Promo Price" };

                // assign header based on modulo
                int groupIndex = e.RowIndex % 3;
                dgv_Main.Rows[e.RowIndex].HeaderCell.Value = headerNames[groupIndex];

               
            }
        }
    }

}
