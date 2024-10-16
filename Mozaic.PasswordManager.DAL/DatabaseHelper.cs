using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.DAL
{
    internal static class DatabaseHelper
    {
        public static readonly string ConnectionString = null;

        static DatabaseHelper()
        {
            //ToD0 : need to read it from Config file
            ConnectionString = "server=desktop-o738fn0\\mssqlserver1;database=PasswordManager;user=sa;password=11111111;trustservercertificate=true";
        }

    }
}
