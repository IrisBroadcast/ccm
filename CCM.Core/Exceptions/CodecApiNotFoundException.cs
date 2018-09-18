using System;

namespace CCM.Core.Exceptions
{
    public class CodecApiNotFoundException : Exception
    {
        public CodecApiNotFoundException()
        {
        }

        public CodecApiNotFoundException(string message) : base(message)
        {
        }

        public CodecApiNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}