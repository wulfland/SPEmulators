namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Globalization;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPListEventProperties : SimSPEventPropertiesBase, ICanIsolate<SPListEventProperties, ShimSPListEventProperties>, IInstanced<SPListEventProperties>, IInstanced
    {
        private SPList list;
        private Guid? listId;
        private string listTitle;
        private string userDisplayName;
        private string userLoginName;

        public Guid FeatureId
        {
            get;
            set;
        }

        public int TemplateId
        {
            get;
            set;
        }

        public string WebUrl
        {
            get;
            set;
        }

        public SPContext Context
        {
            get;
            set;
        }

        public SPField Field
        {
            get;
            set;
        }

        public SPList List
        {
            get
            {
                return this.list;
            }
            set
            {
                if (this.listId.HasValue)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Field '{0}' is already set.", new object[]
                    {
                        "listId"
                    }));
                }
                if (this.listTitle != null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Field '{0}' is already set.", new object[]
                    {
                        "listTitle"
                    }));
                }
                this.list = value;
            }
        }

        public string UserDisplayName
        {
            get
            {
                return this.userDisplayName;
            }
            set
            {
                this.userDisplayName = value;
            }
        }

        public string UserLoginName
        {
            get
            {
                return this.userLoginName;
            }
            set
            {
                this.userLoginName = value;
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

        public Guid ListId
        {
            get
            {
                return (this.list != null) ? this.List.ID : (this.listId.HasValue ? this.listId.Value : Guid.Empty);
            }
            set
            {
                if (this.list != null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Field '{0}' is already set.", new object[]
                    {
                        "ListItem"
                    }));
                }

                this.listId = new Guid?(value);
            }
        }

        public string ListTitle
        {
            get
            {
                return (this.list != null) ? this.List.Title : (this.listTitle ?? string.Empty);
            }
            set
            {
                if (this.list != null)
                {
                    throw new InvalidOperationException("The ListItem is already specified in SPItemEventProperties.");
                }

                this.listTitle = value;
            }
        }

        public new ShimSPListEventProperties Fake
        {
            get;
            private set;
        }

        public new SPListEventProperties Instance
        {
            get
            {
                return (SPListEventProperties)base.Instance;
            }
        }

        public SimSPListEventProperties()
            : this(ShimRuntime.CreateUninitializedInstance<SPListEventProperties>())
        {
        }

        public SimSPListEventProperties(SPListEventProperties instance)
            : base(instance)
        {
            var listEventProperties = new ShimSPListEventProperties(instance);
            listEventProperties.FieldGet = (() => this.Field);
            listEventProperties.FieldNameGet = (() => this.Field.InternalName);
            listEventProperties.ListGet = (() => this.List);
            listEventProperties.ListIdGet = (() => this.ListId);
            listEventProperties.ListTitleGet = (() => this.ListTitle);
            listEventProperties.WebGet = (() => this.Web);
            listEventProperties.WebUrlGet = (() => this.WebUrl);
            listEventProperties.FeatureIdGet = (() => this.FeatureId);
            listEventProperties.TemplateIdGet = (() => this.TemplateId);
            this.Fake = listEventProperties;
        }

        public static SimSPListEventProperties FromInstance(SPListEventProperties instance)
        {
            return InstancedPool.CastAsInstanced<SPListEventProperties, SimSPListEventProperties>(instance);
        }

        internal new static void Initialize()
        {
            SimSPEventPropertiesBase.Initialize();
            ShimSPListEventProperties.BehaveAsNotImplemented();
        }
    }
}
