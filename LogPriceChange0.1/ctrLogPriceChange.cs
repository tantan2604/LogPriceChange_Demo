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

        private void btnlpcsubmit_Click(object sender, EventArgs e)

        {
            

            //Insert data to database
            object endDate = DBNull.Value;
            object lpc_createdD = DateTime.Now;


            if (lpc_rbtn_permanent.Checked)
            {
                lpc_dtp_enddate.Value = DateTime.Now;
                lpc_dtp_enddate.Enabled = false;
                lpc_dtp_enddate.Checked = false; // Uncheck the DateTimePicker

            }
            else if (lpc_rbtn_temporary.Checked)
            {
                endDate = lpc_dtp_enddate.Value;
            }
            else
            {
                MessageBox.Show("Please select a promotion type.");
                return;
            }

            if (string.IsNullOrWhiteSpace(lpc_tb_supplier.Text) || string.IsNullOrWhiteSpace(lpc_tb_promotitle.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            try
            {
                MainForm mainForm = (MainForm)this.FindForm();
                connection.Open();
                string dbinsert = "INSERT INTO tbl_logPriceChange (TDate, Supplier, PromoTitle, SDate, EDate, CreatedBy, Promotype, Timestamp1 )VALUES (?,?,?,?,?,?,?,?)";
                OleDbCommand cmd = new OleDbCommand(dbinsert, connection);
                cmd.Parameters.AddWithValue("?", lpc_dtp_memodate.Value.Date);
                cmd.Parameters.AddWithValue("?", lpc_tb_supplier.Text);
                cmd.Parameters.AddWithValue("?", lpc_tb_promotitle.Text);
                cmd.Parameters.AddWithValue("?", lpc_dtp_startdate.Value.Date);
                cmd.Parameters.AddWithValue("?", endDate == DBNull.Value ? (object)DBNull.Value : endDate.ToString());
                cmd.Parameters.AddWithValue("?", mainForm.dashb_lbl_userlogged.Text);
                cmd.Parameters.AddWithValue("?", lpc_rbtn_permanent.Checked ? lpc_rbtn_permanent.Text : lpc_rbtn_temporary.Text); // Assuming the logged-in user is the current user
                cmd.Parameters.AddWithValue("?", lpc_createdD.ToString());



                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Inserted Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally

            {
                lpc_tb_supplier.Clear();
                lpc_tb_promotitle.Clear();

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
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
       

        private void lpc_dgv_searchbycode_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                //string query = "SELECT * FROM tbl_supplier";
                //OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                connection.Open();
                DataTable dt = new DataTable();
                //da.Fill(dt);


                if (e.RowIndex >= 0)
                {

                    DataGridViewRow selectedRow = lpc_dgv_searchbycode.Rows[e.RowIndex];

                    int dbvalrow = lpc_dgv_dbvalue.Rows.Add(
                        selectedRow.Cells["PROD_C"].Value,
                        selectedRow.Cells["PROD_N"].Value,
                        selectedRow.Cells["FREE"].Value,
                        selectedRow.Cells["PLFOB"].Value,
                        selectedRow.Cells["NWF"].Value,
                        selectedRow.Cells["NWFR"].Value,
                        selectedRow.Cells["PC_PF"].Value,
                        selectedRow.Cells["PC_PFL"].Value,
                        selectedRow.Cells["PC_RP"].Value,
                        selectedRow.Cells["PC_PA"].Value,
                        selectedRow.Cells["PC_PLSRP"].Value,
                        selectedRow.Cells["PC_LSRP"].Value,
                        selectedRow.Cells["PC_PPA2LP"].Value,
                        selectedRow.Cells["PC_LP"].Value,
                        selectedRow.Cells["PC_PPA2WA"].Value,
                        selectedRow.Cells["PC_WA"].Value,
                        selectedRow.Cells["PC_PPA2WB"].Value,
                        selectedRow.Cells["PC_WB"].Value,
                        selectedRow.Cells["PC_PPA2WC"].Value,
                        selectedRow.Cells["PC_WC"].Value,
                        selectedRow.Cells["PC_PPA2LC"].Value,
                        selectedRow.Cells["PC_LC"].Value,
                        selectedRow.Cells["PC_PPA2PG"].Value,
                        selectedRow.Cells["PC_PG"].Value,
                        selectedRow.Cells["PC_PPA2PH"].Value,
                        selectedRow.Cells["PC_PH"].Value,
                        selectedRow.Cells["PC_PPA2PB"].Value,
                        selectedRow.Cells["PC_PB"].Value,
                        selectedRow.Cells["PC_PPA2PD"].Value,
                        selectedRow.Cells["PC_PD"].Value,
                        selectedRow.Cells["LPP_AMT"].Value,
                        selectedRow.Cells["LPP_REF"].Value,
                        selectedRow.Cells["PC_PPA2PC"].Value,
                        selectedRow.Cells["PC_PC"].Value
                    );

                    int supprow = lpc_dgv_dbvalue.Rows.Add(
                        //dt.Rows[0]["Price"].ToString(),
                        //dt.Rows[0]["PROD_C"].ToString()
                        );

                    int promoRow = lpc_dgv_dbvalue.Rows.Add("", "", "", "", "", "", "", "");
                    lpc_dgv_dbvalue.Rows[supprow].HeaderCell.Value = "Supplier Price";
                    lpc_dgv_dbvalue.Rows[dbvalrow].HeaderCell.Value = "Database Value"; // Set the header for the new row
                    lpc_dgv_dbvalue.Rows[promoRow].HeaderCell.Value = "Promo Value";

                    // Make the header cell of the specified row read-only
                    // Remove or comment out this line, as DataGridViewRow.HeaderCell.ReadOnly cannot be set
                    lpc_dgv_dbvalue.Rows[dbvalrow].ReadOnly = true; // Disable the entire row
                    lpc_dgv_dbvalue.Rows[promoRow].Cells[0].ReadOnly = true;  // Disable cell 0
                    lpc_dgv_dbvalue.Rows[promoRow].Cells[1].ReadOnly = true;  // Disable cell 2


                        DataGridViewButtonCell lpc_btnsup = new DataGridViewButtonCell();
                    DataGridViewButtonCell lpc_btnprom = new DataGridViewButtonCell();
                    lpc_btnsup.Value = "Supplier Price";  // Set button text for supplier price
                    lpc_btnprom.Value = "Promo Price";  // Set button text for promo price
                    // Add the button cells to the last column of the respective rows
                    lpc_dgv_dbvalue.Rows[supprow].Cells[lpc_dgv_dbvalue.Columns.Count - 1] = lpc_btnsup;
                    lpc_dgv_dbvalue.Rows[promoRow].Cells[lpc_dgv_dbvalue.Columns.Count - 1] = lpc_btnprom;
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

        private void lpc_dgv_dbvalue_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            
        }


        private void SaveDataFromDataGridView()
        {
            try
            {
                // Handle the second row (tbl_supplier)
                DataGridViewRow supplierRow = lpc_dgv_dbvalue.Rows[1];
                SaveSupplierData(supplierRow);

                // Handle the third row (tbl_promo)
                DataGridViewRow promoRow = lpc_dgv_dbvalue.Rows[2];
                SavePromoData(promoRow);

                MessageBox.Show("Data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveSupplierData(DataGridViewRow row)
        {
            // Data validation for SupplierID
            if (row.Cells["SupplierID"].Value == null || string.IsNullOrWhiteSpace(row.Cells["SupplierID"].Value.ToString()))
            {
                MessageBox.Show("Supplier ID cannot be empty for the second row.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int supplierId = Convert.ToInt32(row.Cells["SupplierID"].Value);
            string supplierName = row.Cells["SupplierName"].Value?.ToString() ?? string.Empty;
            string contactPerson = row.Cells["ContactPerson"].Value?.ToString() ?? string.Empty;

            {
                connection.Open();

                // Check if supplier exists
                string checkSql = "SELECT COUNT(*) FROM tbl_supplier WHERE SupplierID = ?"; // OLEDB uses '?' for parameters
                using (OleDbCommand checkCmd = new OleDbCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("?", supplierId); // Parameters added in order for OLEDB
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Update if exists
                        string updateSql = "UPDATE tbl_supplier SET SupplierName = ?, ContactPerson = ? WHERE SupplierID = ?";
                        using (OleDbCommand updateCmd = new OleDbCommand(updateSql, connection))
                        {
                            updateCmd.Parameters.AddWithValue("?", supplierName);
                            updateCmd.Parameters.AddWithValue("?", contactPerson);
                            updateCmd.Parameters.AddWithValue("?", supplierId);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Insert if not exists
                        string insertSql = "INSERT INTO tbl_supplier (SupplierID, SupplierName, ContactPerson) VALUES (?, ?, ?)";
                        using (OleDbCommand insertCmd = new OleDbCommand(insertSql, connection))
                        {
                            insertCmd.Parameters.AddWithValue("?", supplierId);
                            insertCmd.Parameters.AddWithValue("?", supplierName);
                            insertCmd.Parameters.AddWithValue("?", contactPerson);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void SavePromoData(DataGridViewRow row)
        {
            // Data validation for PromoID
            if (row.Cells["PromoID"].Value == null || string.IsNullOrWhiteSpace(row.Cells["PromoID"].Value.ToString()))
            {
                MessageBox.Show("Promo ID cannot be empty for the third row.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int promoId = Convert.ToInt32(row.Cells["PromoID"].Value);
            string promoName = row.Cells["PromoName"].Value?.ToString() ?? string.Empty;
            // Handle potential DBNull or conversion issues for DiscountPercentage
            decimal discountPercentage = 0m;
            if (row.Cells["DiscountPercentage"].Value != null && decimal.TryParse(row.Cells["DiscountPercentage"].Value.ToString(), out discountPercentage))
            {
                // Successfully parsed
            }
            else
            {
                MessageBox.Show("Invalid or empty value for Discount Percentage in the third row. Defaulting to 0.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            {
                connection.Open();

                // Check if promo exists
                string checkSql = "SELECT COUNT(*) FROM tbl_promo WHERE PromoID = ?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("?", promoId);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Update if exists
                        string updateSql = "UPDATE tbl_promo SET PromoName = ?, DiscountPercentage = ? WHERE PromoID = ?";
                        using (OleDbCommand updateCmd = new OleDbCommand(updateSql, connection))
                        {
                            updateCmd.Parameters.AddWithValue("?", promoName);
                            updateCmd.Parameters.AddWithValue("?", discountPercentage);
                            updateCmd.Parameters.AddWithValue("?", promoId);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Insert if not exists
                        string insertSql = "INSERT INTO tbl_promo (PromoID, PromoName, DiscountPercentage) VALUES (?, ?, ?)";
                        using (OleDbCommand insertCmd = new OleDbCommand(insertSql, connection))
                        {
                            insertCmd.Parameters.AddWithValue("?", promoId);
                            insertCmd.Parameters.AddWithValue("?", promoName);
                            insertCmd.Parameters.AddWithValue("?", discountPercentage);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

    }
}
