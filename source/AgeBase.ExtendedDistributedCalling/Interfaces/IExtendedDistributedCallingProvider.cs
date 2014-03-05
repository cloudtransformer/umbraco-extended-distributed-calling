using System.Collections.Generic;

namespace AgeBase.ExtendedDistributedCalling.Interfaces
{
    public interface IExtendedDistributedCallingProvider
    {
        List<string> GetServers();
    }
}