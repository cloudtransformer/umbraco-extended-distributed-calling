using System;

namespace AgeBase.ExtendedDistributedCalling.Exceptions
{
    public class DistributedCallingProviderNotFoundException : Exception
    {
        public DistributedCallingProviderNotFoundException()
            : base(string.Empty, null)
        {
        }

        public DistributedCallingProviderNotFoundException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }

        public DistributedCallingProviderNotFoundException(string message)
            : base(message, null)
        {
        }

        public DistributedCallingProviderNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}