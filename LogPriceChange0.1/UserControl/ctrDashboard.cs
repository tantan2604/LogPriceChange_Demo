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
    public partial class ctrDashboard : UserControl
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\SharedDB\pricematrix.accdb;";
        private void UpdateStatusCounts()
        {
            // Define the query to count all statuses
            string query = "SELECT DocStatus, COUNT(DocStatus) AS StatusCount FROM tbl_logpricechange GROUP BY DocStatus;";

            // Use a 'using' statement for the connection for proper resource management
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand(query, connection);
                    OleDbDataReader reader = command.ExecuteReader();

                    // Reset all labels to zero before updating
                    lblApprovedCount.Text = "0";
                    lblForApproval.Text = "0";
                    lblRejectedCount.Text = "0";
                    lblDraftCount.Text = "0";


                    // Loop through the query results
                    while (reader.Read())
                    {
                        string status = reader["DocStatus"].ToString();
                        int count = Convert.ToInt32(reader["StatusCount"]);

                        // Update the appropriate label based on the status
                        switch (status)
                        {
                            case "ForApproval":
                                lblForApproval.Text = count.ToString();
                                break;

                            case "Approved":
                                lblApprovedCount.Text = count.ToString();
                                break;

                            case "Rejected":
                                lblRejectedCount.Text = count.ToString();
                                break;

                            case "Draft":
                                lblDraftCount.Text = count.ToString();
                                break;
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating status counts: " + ex.Message);
                }
            }
        }
        public ctrDashboard()
        {
            InitializeComponent();
            UpdateStatusCounts();
        }
    }
}
