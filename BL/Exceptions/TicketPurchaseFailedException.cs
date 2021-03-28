using System;
using System.Runtime.Serialization;

namespace BL.Exceptions
{
    [Serializable]
    public class TicketPurchaseFailedException : Exception
    {
        public TicketPurchaseFailedException()
        {
        }

        public TicketPurchaseFailedException(string message) : base(message)
        {
        }

        public TicketPurchaseFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TicketPurchaseFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}