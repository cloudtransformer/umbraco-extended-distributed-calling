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
    public class ExtendedDistributedCallingService
    {
        private static readonly ExtendedDistributedCallingConfigSection s_Config;
        private static IExtendedDistributedCallingProvider s_Provider;

        private static string s_Login;
        private static string s_Password;

        static ExtendedDistributedCallingService()
        {
            s_Config = ConfigurationManager.GetSection("extendedDistributedCalling") as ExtendedDistributedCallingConfigSection;
        }

        private ExtendedDistributedCallingService()
        {
        }

        public static void Refresh()
        {
            if (s_Config == null || !s_Config.Enabled)
                return;

            GetProvider();
            if (s_Provider == null) 
                return;

            GetUser();
            if (string.IsNullOrEmpty(s_Login) || string.IsNullOrWhiteSpace(s_Password))
                return;

            var servers = s_Provider.GetServers();
            if (servers == null || !servers.Any())
                return;

            LogHelper.Debug<ExtendedDistributedCallingService>("Submitting calls to Extended Distributed Calling");

            foreach (var server in servers)
                RefreshTarget(server, s_Login, s_Password);

            LogHelper.Debug<ExtendedDistributedCallingService>("Finished submitting calls to Extended Distributed Calling");
        }

        private static void GetProvider()
        {
            if (s_Provider != null) 
                return;

            try
            {
                var obj = Activator.CreateInstance(s_Config.Assembly, s_Config.Type);
                s_Provider = obj.Unwrap() as IExtendedDistributedCallingProvider;
            }
            catch (Exception ex)
            {
                LogHelper.Error<ExtendedDistributedCallingService>("Could not create the Extended Distributed Calling provider instance", ex);
                throw new ExtendedDistributedCallingProviderNotFoundException(ex);
            }
        }

        private static void GetUser()
        {
            // Always find the user login and password as
            // the user's password may have changed

            try
            {
                var user = User.GetUser(s_Config.User);

                s_Login = user.LoginName;
                s_Password = user.GetPassword();
            }
            catch (Exception ex)
            {
                LogHelper.Error<ExtendedDistributedCallingService>("Could not find the user for the Extended Distributed Calling service", ex);
                throw new ExtendedDistributedCallingUserException(ex);
            }
        }

        private static void RefreshTarget(string server, string login, string password)
        {
            var cleanedServer = server.Trim().ToLower();

            // Ignore the current server
            if (HttpContext.Current.Request.Url.Host.ToLower().Equals(cleanedServer))
                return;

            try
            {
                LogHelper.Debug<ExtendedDistributedCallingService>("Refreshing " + server);

                using (var cacheRefresher = new ServerSyncWebServiceClient(cleanedServer))
                    cacheRefresher.RefreshAll(new Guid(DistributedCache.PageCacheRefresherId), login, password);
            }
            catch (Exception ex)
            {
                LogHelper.Error<ExtendedDistributedCallingService>("Could not refresh " + server, ex);
            }
        }
    }
}