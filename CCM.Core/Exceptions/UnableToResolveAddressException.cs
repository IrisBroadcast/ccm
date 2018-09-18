using System;

namespace CCM.Core.Exceptions
{
    public class UnableToResolveAddressException : Exception
    {
        public UnableToResolveAddressException()
        {
        }

        public UnableToResolveAddressException(string message) : base(message)
        {   
        }

        public UnableToResolveAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}