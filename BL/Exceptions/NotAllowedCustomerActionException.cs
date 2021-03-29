using System;
using System.Runtime.Serialization;

namespace BL.Exceptions
{
    [Serializable]
    public class NotAllowedCustomerActionException : Exception
    {
        public NotAllowedCustomerActionException()
        {
        }

        public NotAllowedCustomerActionException(string message) : base(message)
        {
        }

        public NotAllowedCustomerActionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotAllowedCustomerActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
