﻿using System;
using System.Runtime.Serialization;

namespace BL.Exceptions
{
    [Serializable]
    class WrongCredentialsException : Exception
    {
        public WrongCredentialsException()
        {
        }

        public WrongCredentialsException(string message) : base(message)
        {
        }

        public WrongCredentialsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongCredentialsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
