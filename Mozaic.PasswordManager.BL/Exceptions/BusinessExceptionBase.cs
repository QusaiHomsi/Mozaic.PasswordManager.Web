using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.BL.Exceptions
{
    public class BusinessExceptionBase:Exception
    {
        protected BusinessExceptionBase()
        {
        }

        public BusinessExceptionBase(string message)
            : base(message)
        {
        }

        public BusinessExceptionBase(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
