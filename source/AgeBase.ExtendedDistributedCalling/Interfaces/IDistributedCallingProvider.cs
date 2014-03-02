using System.Collections.Generic;

namespace AgeBase.ExtendedDistributedCalling.Interfaces
{
    public interface IDistributedCallingProvider
    {
        List<string> GetAddresses();
    }
}