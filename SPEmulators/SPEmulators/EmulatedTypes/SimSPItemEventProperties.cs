namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Globalization;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPItemEventProperties : SimSPEventPropertiesBase, ICanIsolate<SPItemEventProperties, ShimSPItemEventProperties>, IInstanced<SPItemEventProperties>, IInstanced
    {
        private readonly SimSPItemEventDataCollection afterProperties;
        private SPContext context;
        private Guid? listId;
        private SPListItem listItem;
        private int? listItemId;
        private string listTitle;
        private string webUrl;

        public string UserDisplayName
        {
            get;
            set;
        }

        public string UserLoginName
        {
            get;
            set;
        }

        public SimSPItemEventDataCollection AfterProperties
        {
            get
            {
                return this.afterProperties;
            }
        }

        public SPContext Context
        {
            get
            {
                if (this.context == null)
                {
                    this.context = SimSPContext.Current.Instance;
                }

                return this.context;
            }
            set
            {
                this.context = value;
            }
        }

        public SPListItem ListItem
        {
            get
            {
                return this.listItem;
            }
            set
            {
                if (this.listId.HasValue)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Field '{0}' is already initialized.", new object[]
                    {
                        "listId"
                    }));
                }
                if (this.listItemId.HasValue)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Field '{0}' is already initialized.", new object[]
                    {
                        "listItemId"
                    }));
                }
                if (this.listTitle != null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Field '{0}' is already initialized.", new object[]
                    {
                        "listTitle"
                    }));
                }
                if (this.webUrl != null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Field '{0}' is already initialized.", new object[]
                    {
                        "webUrl"
                    }));
                }
                this.listItem = value;
            }
        }

        public SPList List
        {
            get
            {
                return (this.listItem != null) ? this.listItem.ParentList : null;
            }
        }

        public SPWeb Web
        {
            get
            {
                return this.List.ParentWeb;
            }
        }

        public SPSite Site
        {
            get
            {
                return this.Web.Site;
            }
        }

        public string WebUrl
        {
            get
            {
                return (this.listItem != null) ? this.Web.Url : this.webUrl;
            }
            set
            {
                if (this.listItem != null)
                {
                    throw new InvalidOperationException("The listitem is already initialized.");
                }

                this.webUrl = value;
            }
        }

        public Guid ListId
        {
            get
            {
                return (this.listItem != null) ? this.List.ID : (this.listId.HasValue ? this.listId.Value : Guid.Empty);
            }
            set
            {
                if (this.listItem != null)
                {
                    throw new InvalidOperationException("The listitem is already initialized.");
                }
                this.listId = new Guid?(value);
            }
        }

        public string ListTitle
        {
            get
            {
                return (this.listItem != null) ? this.List.Title : this.listTitle;
            }
            set
            {
                if (this.listItem != null)
                {
                    throw new InvalidOperationException("The listitem is already initialized.");
                }
                this.listTitle = value;
            }
        }

        public int ListItemId
        {
            get
            {
                return (this.listItem != null) ? this.ListItem.ID : (this.listItemId.HasValue ? this.listItemId.Value : 0);
            }
            set
            {
                if (this.listItem != null)
                {
                    throw new InvalidOperationException("The listitem is already initialized.");
                }
                this.listItemId = new int?(value);
            }
        }

        public new ShimSPItemEventProperties Fake
        {
            get;
            private set;
        }


        public new SPItemEventProperties Instance
        {
            get
            {
                return (SPItemEventProperties)base.Instance;
            }
        }

        public SimSPItemEventProperties() : this(ShimRuntime.CreateUninitializedInstance<SPItemEventProperties>())
        {
        }

        public SimSPItemEventProperties(SPItemEventProperties instance) : base(instance)
        {
            this.afterProperties = new SimSPItemEventDataCollection();
            var itemEventProperties = new ShimSPItemEventProperties(instance);
            itemEventProperties.ListIdGet = (() => this.ListId);
            itemEventProperties.ListTitleGet = (() => this.ListTitle);
            itemEventProperties.ListItemGet = (() => this.ListItem);
            itemEventProperties.ListItemIdGet = (() => this.ListItemId);
            itemEventProperties.UserLoginNameGet = (() => this.UserLoginName);
            itemEventProperties.UserDisplayNameGet = (() => this.UserDisplayName);
            itemEventProperties.WebUrlGet = (() => this.WebUrl);
            itemEventProperties.OpenWeb = (() => this.Web);
            itemEventProperties.OpenSite = (() => this.Site);
            itemEventProperties.AfterPropertiesGet = (() => this.AfterProperties.Instance);
            itemEventProperties.ListGet = (() => this.List);
            itemEventProperties.WebGet = (() => this.Web);
            this.Fake = itemEventProperties;
        }

        public static SimSPItemEventProperties FromInstance(SPItemEventProperties instance)
        {
            return InstancedPool.CastAsInstanced<SPItemEventProperties, SimSPItemEventProperties>(instance);
        }

        internal new static void Initialize()
        {
            SimSPEventPropertiesBase.Initialize();
            ShimSPItemEventProperties.BehaveAsNotImplemented();
        }
    }
}
