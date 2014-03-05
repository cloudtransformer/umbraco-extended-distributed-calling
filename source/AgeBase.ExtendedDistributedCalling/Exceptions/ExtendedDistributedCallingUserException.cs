using System;

namespace AgeBase.ExtendedDistributedCalling.Exceptions
{
    public class ExtendedDistributedCallingUserException : Exception
    {
        public ExtendedDistributedCallingUserException()
            : base(string.Empty, null)
        {
        }

        public ExtendedDistributedCallingUserException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }

        public ExtendedDistributedCallingUserException(string message)
            : base(message, null)
        {
        }

        public ExtendedDistributedCallingUserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}