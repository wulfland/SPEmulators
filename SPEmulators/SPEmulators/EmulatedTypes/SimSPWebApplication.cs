namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Administration.Fakes;

    internal class SimSPWebApplication : SimSPPersistedUpgradableObject, ICanIsolate<SPWebApplication, ShimSPWebApplication>, IInstanced<SPWebApplication>, IInstanced
    {
        private readonly Dictionary<SPUrlZone, Uri> responseUris = new Dictionary<SPUrlZone, Uri>();
        private readonly SimSPSiteCollection sites;
        private readonly Collection<SPWebConfigModification> webConfigModifications;

        public SimSPSiteCollection Sites
        {
            get
            {
                return this.sites;
            }
        }

        public Collection<SPWebConfigModification> WebConfigModifications
        {
            get
            {
                return this.webConfigModifications;
            }
        }

        public new ShimSPWebApplication Fake
        {
            get;
            private set;
        }

        public new SPWebApplication Instance
        {
            get
            {
                return (SPWebApplication)base.Instance;
            }
        }

        public SimSPWebApplication()
            : this(ShimRuntime.CreateUninitializedInstance<SPWebApplication>())
        {
        }

        public SimSPWebApplication(SPWebApplication instance)
            : base(instance, "Website")
        {
            this.webConfigModifications = new Collection<SPWebConfigModification>();
            this.sites = new SimSPSiteCollection
            {
                WebApplication = this.Instance
            };
            var shimSPWebApplication = new ShimSPWebApplication(instance);
            shimSPWebApplication.WebConfigModificationsGet = (() => this.webConfigModifications);
            shimSPWebApplication.SitesGet = (() => this.sites.Instance);
            shimSPWebApplication.GetResponseUriSPUrlZoneString = ((SPUrlZone zone, string path) => new Uri(instance.GetResponseUri(zone), path));
            shimSPWebApplication.GetResponseUriSPUrlZone = (delegate(SPUrlZone zone)
            {
                Uri uri;
                return this.responseUris.TryGetValue(zone, out uri) ? uri : this.responseUris[0];
            });
            this.Fake = shimSPWebApplication;
        }

        public static SimSPWebApplication FromInstance(SPWebApplication instance)
        {
            return InstancedPool.CastAsInstanced<SPWebApplication, SimSPWebApplication>(instance);
        }
    }
}
