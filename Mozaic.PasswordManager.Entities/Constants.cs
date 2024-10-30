using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.Entities
{
    public static class SystemTransactionStatus
    {
        public const string Success = "0000";
        public const string Failed = "9999";
        public const string ExistingRecord = "0001";
    }
}
