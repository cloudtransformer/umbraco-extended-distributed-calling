using System;

namespace AgeBase.ExtendedDistributedCalling.Exceptions
{
    public class ExtendedDistributedCallingProviderNotFoundException : Exception
    {
        public ExtendedDistributedCallingProviderNotFoundException()
            : base(string.Empty, null)
        {
        }

        public ExtendedDistributedCallingProviderNotFoundException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }

        public ExtendedDistributedCallingProviderNotFoundException(string message)
            : base(message, null)
        {
        }

        public ExtendedDistributedCallingProviderNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}