using AgeBase.ExtendedDistributedCalling.Services;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace AgeBase.ExtendedDistributedCalling.Events
{
    public class ExtendedDistributedCallingEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Copied += delegate { ExtendedDistributedCallingService.Refresh(); };
            ContentService.Deleted += delegate { ExtendedDistributedCallingService.Refresh(); };
            ContentService.EmptiedRecycleBin += delegate { ExtendedDistributedCallingService.Refresh(); };
            ContentService.Moved += delegate { ExtendedDistributedCallingService.Refresh(); };
            ContentService.Published += delegate { ExtendedDistributedCallingService.Refresh(); };
            ContentService.RolledBack += delegate { ExtendedDistributedCallingService.Refresh(); };
            ContentService.Trashed += delegate { ExtendedDistributedCallingService.Refresh(); };
            ContentService.UnPublished += delegate { ExtendedDistributedCallingService.Refresh(); };

            MediaService.Deleted += delegate { ExtendedDistributedCallingService.Refresh(); };
            MediaService.EmptiedRecycleBin += delegate { ExtendedDistributedCallingService.Refresh(); };
            MediaService.Saved += delegate { ExtendedDistributedCallingService.Refresh(); };
            MediaService.Trashed += delegate { ExtendedDistributedCallingService.Refresh(); };
        }
    }
}