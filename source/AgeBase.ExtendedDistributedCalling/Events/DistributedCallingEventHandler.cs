using AgeBase.ExtendedDistributedCalling.Services;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace AgeBase.ExtendedDistributedCalling.Events
{
    public class DistributedCallingEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Copied += delegate { DistributedCallingService.Refresh(); };
            ContentService.Deleted += delegate { DistributedCallingService.Refresh(); };
            ContentService.EmptiedRecycleBin += delegate { DistributedCallingService.Refresh(); };
            ContentService.Moved += delegate { DistributedCallingService.Refresh(); };
            ContentService.Published += delegate { DistributedCallingService.Refresh(); };
            ContentService.RolledBack += delegate { DistributedCallingService.Refresh(); };
            ContentService.Trashed += delegate { DistributedCallingService.Refresh(); };
            ContentService.UnPublished += delegate { DistributedCallingService.Refresh(); };

            MediaService.Deleted += delegate { DistributedCallingService.Refresh(); };
            MediaService.EmptiedRecycleBin += delegate { DistributedCallingService.Refresh(); };
            MediaService.Saved += delegate { DistributedCallingService.Refresh(); };
            MediaService.Trashed += delegate { DistributedCallingService.Refresh(); };
        }
    }
}