using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.DAL.Exceptions
{
    internal class DataProviderException:DatabaseExceptionBase
    {
        

        public DataProviderException()
        {

        }

        public DataProviderException(string message) : base(message)
        {
        }

        public DataProviderException(string message, Exception ex) : base(message,ex) 
        {
            
        }
    }
}
