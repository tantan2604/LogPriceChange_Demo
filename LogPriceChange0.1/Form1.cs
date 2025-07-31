using System;
using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace LogPriceChange0._1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {

            InitializeComponent();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dashb_lbl_userlogged.Text = $"{UserSession.Username}!";
            
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

  


        // End of event handlers for button clicks loading different user controls
    }
}
