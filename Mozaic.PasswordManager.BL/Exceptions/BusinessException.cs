using System;

namespace Mozaic.PasswordManager.BL.Exceptions
{
    public class BusinessException : BusinessExceptionBase
    {
        public BusinessException() { }

        public BusinessException(string message) : base(message) { }

        public BusinessException(string message, Exception inner) : base(message, inner) { }
    }
}
