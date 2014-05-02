namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPSite : Isolator<SPSite, ShimSPSite>
    {
        public SimSPWebCollection AllWebs
        {
            get
            {
                var webCollection = new SimSPWebCollection(null);
                SimSPSite.CollectAllWebs(this.RootWeb.Instance, webCollection);
                return webCollection;
            }
        }

        internal SimSPFeatureCollection Features
        {
            get;
            private set;
        }

        public Guid ID
        {
            get;
            private set;
        }

        public SimSPWeb RootWeb
        {
            get;
            private set;
        }

        public string Url
        {
            get;
            private set;
        }

        internal SimSPWebApplication WebApplication
        {
            get;
            private set;
        }

        public SimSPSite(string siteUrl) : this(ShimRuntime.CreateUninitializedInstance<SPSite>(), siteUrl)
        {
        }

        internal SimSPSite(SPSite instance, string siteUrl) : base(instance)
        {
            base.Fake.AllWebsGet = (() => this.AllWebs.Instance);
            base.Fake.Dispose = (new FakesDelegates.Action(this.Dispose));
            base.Fake.FeaturesGet = (() => this.Features.Instance);
            base.Fake.IDGet = (() => this.ID);
            base.Fake.OpenWeb = (() => this.OpenWeb().Instance);
            base.Fake.OpenWebGuid = ((Guid webId) => this.OpenWeb(webId).Instance);
            base.Fake.OpenWebString = ((string url) => this.OpenWeb(url).Instance);
            base.Fake.RootWebGet = (() => this.RootWeb.Instance);
            base.Fake.UrlGet = (() => this.Url);
            base.Fake.WebApplicationGet = (() => this.WebApplication.Instance);
            this.Features = new SimSPFeatureCollection
            {
                ScopeParent = base.Instance
            };
            this.ID = Guid.NewGuid();
            this.Url = new Uri(siteUrl).ToString().TrimEnd(new char[]
            {
                '/'
            });
            this.RootWeb = new SimSPWeb
            {
                Site = base.Instance,
                Name = string.Empty,
                Title = "Team Site"
            };
            this.WebApplication = new SimSPWebApplication();
        }

        public void Dispose()
        {
        }

        public SimSPWeb OpenWeb()
        {
            return this.RootWeb;
        }

        public SimSPWeb OpenWeb(Guid webId)
        {
            using (IEnumerator<SPWeb> enumerator = (
                from web in this.AllWebs
                where web.ID == webId
                select web).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    SPWeb current = enumerator.Current;
                    return SimSPWeb.FromInstance(current);
                }
            }

            throw new FileNotFoundException();
        }

        public SimSPWeb OpenWeb(string url)
        {
            Uri baseUri = new Uri(this.Url);
            Uri uri = new Uri(baseUri, url);
            SimSPWeb result;
            foreach (SPWeb current in this.AllWebs)
            {
                if (uri == new Uri(current.Url))
                {
                    result = SimSPWeb.FromInstance(current);
                    return result;
                }
            }
            result = new SimSPWeb
            {
                Site = base.Instance,
                Name = url,
                Title = url,
                Exists = false
            };
            return result;
        }

        public static SimSPSite FromInstance(SPSite instance)
        {
            return InstancedPool.CastAsInstanced<SPSite, SimSPSite>(instance);
        }

        internal static void Initialize()
        {
            ShimSPSite.BehaveAsNotImplemented();
            ShimSPSite.StaticConstructor = (delegate
            {
            });
            ShimSPSite.ConstructorString = (delegate(SPSite me, string url)
            {
                new SimSPSite(me, url);
            });
        }

        private static void CollectAllWebs(SPWeb web, SimSPWebCollection allWebs)
        {
            allWebs.Add(web);
            foreach (SPWeb web2 in web.Webs)
            {
                SimSPSite.CollectAllWebs(web2, allWebs);
            }
        }
    }
}
