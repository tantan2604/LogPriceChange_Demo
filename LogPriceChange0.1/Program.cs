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

            // Use a 'using' statement to ensure the login form's resources
            // are properly disposed of, which can prevent "Error creating window handle" issues.
            using (wfLoginform loginForm = new wfLoginform())
            {
                // Show the login form as a modal dialog.
                // This will block execution until the form is closed.
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // If login was successful, get the user's role and username
                    // from the public properties of the login form.
                    string username = loginForm.LoggedInUsername;
                    string role = loginForm.LoggedInUserRole;

                    Form mainForm = null;

                    // Validate that the role is not null or empty before proceeding.
                    // This prevents a System.NullReferenceException.
                    if (!string.IsNullOrWhiteSpace(role))
                    {
                        // Determine which form to show based on the user's role
                        switch (role.ToLower())
                        {
                            case "admin":
                                mainForm = new AdminForm(username);
                                break;
                            case "user":
                                mainForm = new MainForm(username);
                                break;
                            default:
                                // Handle unexpected roles.
                                MessageBox.Show("Unknown user role. Application will close.");
                                return; // Exit the application
                        }
                    }
                    else
                    {
                        MessageBox.Show("User role is missing or invalid. Application will close.");
                        return; // Exit the application
                    }

                    // If a valid main form was created, run it.
                    if (mainForm != null)
                    {
                        Application.Run(mainForm);
                    }
                }
            }
        }
    }
}
