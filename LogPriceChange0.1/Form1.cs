using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogPriceChange0._1
{
    public partial class MainForm : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\VisualStudio\LogPriceChange0.1\LogPriceChange0.1\pricematrix.accdb;";
        private OleDbConnection connection;
        private OleDbDataAdapter dataAdapter;
        private DataTable dataTable;
        private string primaryKeyColumn = "ID"; // Assuming PROD_C is the primary key column
        

        public MainForm()
        {    

            InitializeComponent();
           
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dashb_lbl_userlogged.Text = $"Welcome, {UserSession.Username}!";
        }


        // Event handlers for button clicks to load different user controls
        private void btn_lpc_Click(object sender, EventArgs e)
        {
            pnl_main.Controls.Clear();
            ctrLogPriceChange ctrlpc = new ctrLogPriceChange();
            ctrlpc.Dock = DockStyle.Fill;
            pnl_main.Controls.Add(ctrlpc);
            
        }

        private void btn_claimbysellout_Click(object sender, EventArgs e)
        {
            pnl_main.Controls.Clear();
            ctrClaimBySellOut ctrcbso = new ctrClaimBySellOut();
            ctrcbso.Dock = DockStyle.Fill;
            pnl_main.Controls.Add(ctrcbso);
        }

        private void btn_claimbyinventory_Click(object sender, EventArgs e)
        {
            pnl_main.Controls.Clear();
            ctrClaimByInventory ctrcbi = new ctrClaimByInventory();
            ctrcbi.Dock = DockStyle.Fill;
            pnl_main.Controls.Add(ctrcbi);
        }

        private void lbl_userloggedin_Click(object sender, EventArgs e)
        {

        }

       
        // End of event handlers for button clicks loading different user controls
    }
}
