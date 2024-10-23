using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;


namespace Mozaic.PasswordManager.Common
{
    public static class LogingHelper
    {
        public static void LogError(string message, Exception ex) {


            Log.Error(ex, message);
        }
    }
}
