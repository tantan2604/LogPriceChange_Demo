using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogPriceChange0._1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //using (wfLoginform loginForm = new wfLoginform())
            //{
            //    if (loginForm.ShowDialog() == DialogResult.OK)
            //    {
            //        // If login is successful, get the username and role
            //        string username = loginForm.LoggedInUsername;
            //        string role = loginForm.LoggedInUserRole;
            //        // Navigate to the appropriate form based on the role
            //        if (role.ToLower() == "admin")
            //        {
            //            Application.Run(new AdminForm(username));
            //        }
            //        else if (role.ToLower() == "user")
            //        {
            //            Application.Run(new MainForm(username));
            //        }
            //        else
            //        {
            //            MessageBox.Show("Unknown user role. Access denied.");
            //        }
            //    }
            //}
            Application.Run(new wfLoginform());

        }

    }
}
