using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogPriceChange0._1
{

    public partial class AdminForm : Form
    {
        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\VisualStudio\LogPriceChange0.1\LogPriceChange0.1\pricematrix.accdb;");
        public AdminForm(string username)
        {
            InitializeComponent();
            this.Text = "Admin Dashboard";
            LoadDataFromAccess();
        }

        private void LoadDataFromAccess()
        {
            string DocStatus = "ForApproval";
            string query = $"SELECT * FROM tbl_logpricechange WHERE DocStatus LIKE '{DocStatus}' ;" ; 

           
            
                try
                {  
                    connection.Open();
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgv_forApproval.DataSource = dataTable;
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            finally
            {
                if (ConnectionState.Closed != connection.State)
                {
                    connection.Close();
                }
            }
            
        }
    }
}
