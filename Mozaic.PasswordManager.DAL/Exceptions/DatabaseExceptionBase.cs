using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.DAL.Exceptions
{
    internal class DatabaseExceptionBase : Exception
    {
        protected DatabaseExceptionBase()
        {
        }

        public DatabaseExceptionBase(string message)
            : base(message)
        {
        }

        public DatabaseExceptionBase(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
