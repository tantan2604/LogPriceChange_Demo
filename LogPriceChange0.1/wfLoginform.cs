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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LogPriceChange0._1
{
    public partial class wfLoginform : Form
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\TanTan\Desktop\VisualStudio\LogPriceChange0.1\LogPriceChange0.1\pricematrix.accdb;");
        ctrLogPriceChange ctrLogPriceChange = new ctrLogPriceChange();


        public wfLoginform()
        {
            InitializeComponent();


            logf_tb_password.UseSystemPasswordChar = true;
            logf_pb_eyeopen.Click += TogglePasswordVisibility;
            logf_pb_eyeclose.Click += TogglePasswordVisibility;
        }
        private void wfLoginform_Load(object sender, EventArgs e)
        {

        }
        private void logf_btn_login_Click(object sender, EventArgs e)

        {
          
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM tbl_employee WHERE EmployeeUsername='" + logf_tb_username.Text + "'AND EmployeePassword='" + logf_tb_password.Text + "'", conn); // Use Check if userName and Password are correct or stored in the database
                OleDbDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    this.Hide();
                    MainForm mainForm = new MainForm();

                    mainForm.Show();
                    string username = mainForm.dashb_lbl_userlogged.Text = dr["Lastname"].ToString() + " " + dr["Firstname"].ToString(); 
                    UserSession.Username = username;
                    this.DialogResult = DialogResult.OK; // Let Program.cs know login succeeded
                }
                else
                {
                    MessageBox.Show("Login Failed. Please check your username and password.");
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            
            
        }
        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            
            // Check if the password is currently hidden
            if (logf_tb_password.UseSystemPasswordChar)
            {
                // If hidden, show the password by disabling the PasswordChar
                logf_tb_password.UseSystemPasswordChar = false;

                // Hide the open eye icon and show the closed eye icon
                logf_pb_eyeopen.Visible = false;
                logf_pb_eyeclose.Visible = true;
            }
            else
            {
                // If shown, hide the password by enabling the PasswordChar
                logf_tb_password.UseSystemPasswordChar = true;

                // Show the open eye icon and hide the closed eye icon
                logf_pb_eyeopen.Visible = true;
                logf_pb_eyeclose.Visible = false;
            }
        }
    }
}
