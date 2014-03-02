using System;
using System.Configuration;
using System.Linq;
using System.Web;
using AgeBase.ExtendedDistributedCalling.Configuration;
using AgeBase.ExtendedDistributedCalling.Exceptions;
using AgeBase.ExtendedDistributedCalling.Interfaces;
using AgeBase.ExtendedDistributedCalling.Sync;
using umbraco.BusinessLogic;
using Umbraco.Core.Logging;
using Umbraco.Web.Cache;

namespace AgeBase.ExtendedDistributedCalling.Services
{
    public class DistributedCallingService
    {
        private static readonly DistributedCallingConfigSection s_Config;
        private static IDistributedCallingProvider s_Provider;

        static DistributedCallingService()
        {
            s_Config = ConfigurationManager.GetSection("extendedDistributedCalling") as DistributedCallingConfigSection;
        }

        private DistributedCallingService()
        {
        }

        public static void Refresh()
        {
            if (s_Config == null || !s_Config.Enabled)
                return;

            try
            {
                var obj = Activator.CreateInstance(s_Config.Assembly, s_Config.Type);
                s_Provider = obj.Unwrap() as IDistributedCallingProvider;
            }
            catch (Exception ex)
            {
                throw new DistributedCallingProviderNotFoundException(ex);
            }

            if (s_Provider == null) 
                return;

            var addresses = s_Provider.GetAddresses();
            if (addresses == null || !addresses.Any()) 
                return;

            var user = User.GetUser(s_Config.User);
            if (user == null)
                return;

            foreach (var address in addresses)
            {
                var cleanedAddress = address.Trim().ToLower();

                if (HttpContext.Current.Request.Url.Host.ToLower().Equals(cleanedAddress))
                    continue;

                try
                {
                    using (var cacheRefresher = new ServerSyncWebServiceClient(cleanedAddress))
                    {
                        cacheRefresher.RefreshAll(new Guid(DistributedCache.PageCacheRefresherId), user.LoginName,
                            user.GetPassword());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<DistributedCallingService>("Refresh: Could not refresh " + address, ex);
                }
            }
        }
    }
}