namespace SPEmulators.EmulatedTypes
{
    using System.Web;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPContext : Isolator<SPContext, ShimSPContext>
    {
        static SimSPContext current;
        SPItem item;
        SPList list;
        SPWeb web;
        SPSite site;
        SPViewContext viewContext;

        public SPItem Item
        {
            get
            {
                if (item == null)
                {
                    item = this.CreateListItem().Instance;
                }

                return item;
            }
            set
            {
                this.item = value;
            }
        }

        public SPListItem ListItem
        {
            get
            {
                return this.Item as SPListItem;
            }
        }

        public SPList List
        {
            get
            {
                return this.list;
            }
            set
            {
                this.list = value;
            }
        }

        public SPWeb Web
        {
            get
            {
                if (this.web == null)
                {
                    this.web = SimSPContext.CreateWeb();
                }

                return this.web;
            }

            set
            {
                this.web = value;
            }
        }

        public SPSite Site
        {
            get
            {
                return this.Web.Site;
            }
        }

        public SPViewContext ViewContext
        {
            get
            {
                if (this.viewContext == null)
                {
                    this.viewContext = SimSPContext.CreateViewContext().Instance;
                }

                return this.viewContext;
            }
            set
            {
                this.viewContext = value;
            }
        }

        public SimSPContext()
            : this(null)
        {
        }

        public SimSPContext(SPContext instance)
            : base(instance)
        {
            this.Fake.WebGet = () => this.Web;
            this.Fake.SiteGet = () => this.Site;
            this.Fake.ItemGet = () => this.Item;
            this.Fake.ItemIdGet = () => this.Item.ID;
            this.Fake.ListItemGet = () => this.ListItem;
            this.Fake.ListGet = () => this.List;
            this.Fake.ListIdGet = () => this.List.ID;
            this.Fake.WebFeaturesGet = () => this.Web.Features;
            this.Fake.SiteFeaturesGet = () => this.Web.Site.Features;
        }

        internal static SimSPContext Current
        {
            get
            {
                if (current == null)
                {
                    SetCurrent();
                }

                return current;
            }
        }

        internal static void Initialize()
        {
            ShimSPContext.BehaveAsNotImplemented();
            SetCurrent();
            SetGetContextSPWeb();
            SetGetContextHttpContext();
        }

        private static void SetGetContextHttpContext()
        {
            ShimSPContext.GetContextHttpContext = (HttpContext httpContext) =>
            {
                var context = ((SPContext)httpContext.Items["DefaultSPContext"]) ?? new SimSPContext().Instance;
                httpContext.Items["DefaultSPContext"] = context;

                return context;
            };
        }

        private static void SetGetContextSPWeb()
        {
            ShimSPContext.GetContextSPWeb = (SPWeb web) =>
            {
                var httpContext = HttpContext.Current;

                if (httpContext != null)
                {
                    var simcontext = SimSPContext.FromInstance(SPContext.GetContext(httpContext));
                    simcontext.Web = web;

                    return simcontext.Instance;
                }
                else
                {
                    var simweb = SimSPWeb.FromInstance(web);
                    if (simweb.SPContext == null)
                    {
                        var spcontext = new SimSPContext { Web = web };
                        simweb.SPContext = spcontext;
                    }

                    return simweb.SPContext.Instance;
                }
            };
        }

        public static SimSPContext SetCurrent()
        {
            if (current == null)
            {
                current = new SimSPContext();
            }

            ShimSPContext.CurrentGet = () => current.Instance;
            ShimRuntime.RegisterStateCleaner(() => current = null);

            return current;
        }

        public SimSPListItem SetListItem()
        {
            var listItem = this.CreateListItem();
            this.Item = listItem.Instance;

            return listItem;
        }

        public SimSPList SetList()
        {
            var list = this.CreateList();
            this.List = list.Instance;

            return list;
        }

        public SimSPSite SetSite()
        {
            SimSPSite site;
            this.Web = CreateRootWeb(out site).Instance;
            return site;
        }

        private static SimSPWeb CreateRootWeb(out SimSPSite site)
        {
            site = new SimSPSite("http://localhost");
            return site.RootWeb;
        }

        private SimSPList CreateList()
        {
            return new SimSPList();
        }

        private static SimSPViewContext CreateViewContext()
        {
            return new SimSPViewContext();
        }

        private static SPWeb CreateWeb()
        {
            SimSPSite site;
            return CreateRootWeb(out site).Instance;
        }

        private SimSPListItem CreateListItem()
        {
            var list = this.SetList();
            return list.Items.SetNext();
        }

        internal SimSPViewContext SetViewContext()
        {
            var viewContext = CreateViewContext();
            this.viewContext = viewContext.Instance;

            return viewContext;
        }

        public static SimSPContext FromInstance(SPContext ctx)
        {
            return InstancedPool.CastAsInstanced<SPContext, SimSPContext>(ctx);
        }
    }
}
