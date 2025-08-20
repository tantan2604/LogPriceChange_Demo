using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogPriceChange0._1
{
    public static class UserSession
    {
        public static string Username { get; set; } = "Guest";
        public static string LoggedUser { get; set; } = "Guest";

        public static string DocId { get; set; }
    }

}
