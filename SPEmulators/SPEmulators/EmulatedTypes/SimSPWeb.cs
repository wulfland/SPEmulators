namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPWeb : SimSPSecurableObject, ICanIsolate<SPWeb, ShimSPWeb>, IInstanced<SPWeb>, IInstanced
    {
        private readonly Hashtable allProperties;
        private readonly SimSPContentTypeCollection availableContentTypes;
        private readonly SimSPFieldCollection availableFields;
        private readonly SimSPContentTypeCollection contentTypes;
        private readonly SimSPFeatureCollection features;
        private readonly SimSPFieldCollection fields;
        private readonly SimSPFolderCollection folders;
        private readonly SimSPListCollection lists;
        private readonly SimSPPropertyBag properties;
        private readonly SimSPUserCollection users;
        private readonly SimSPWebCollection webs;
        private Guid? id;
        private CultureInfo locale;
        private string name;
        private SimSPFolder rootFolder;
        private string serverRelativeUrl;
        private string title;
        private string url;

        public bool AllowUnsafeUpdates
        {
            get;
            set;
        }

        public bool Exists
        {
            get;
            set;
        }

        protected SimSPFolder RootFolder
        {
            get
            {
                if (this.rootFolder == null)
                {
                    this.rootFolder = new SimSPFolder();
                }

                return this.rootFolder;
            }
        }

        public SPSite Site
        {
            get;
            set;
        }

        public Hashtable AllProperties
        {
            get
            {
                return this.allProperties;
            }
        }

        public SPUser CurrentUser
        {
            get;
            set;
        }

        public SPWeb ParentWeb
        {
            get;
            set;
        }

        public Guid ParentWebId
        {
            get
            {
                SPWeb parentWeb = this.ParentWeb;
                return (parentWeb != null) ? parentWeb.ID : Guid.Empty;
            }
        }

        public SimSPFieldCollection Fields
        {
            get
            {
                return this.fields;
            }
        }

        public SimSPFieldCollection AvailableFields
        {
            get
            {
                return this.availableFields;
            }
        }

        public SimSPListCollection Lists
        {
            get
            {
                return this.lists;
            }
        }

        internal SimSPUserCollection Users
        {
            get
            {
                return this.users;
            }
        }

        internal SimSPFeatureCollection Features
        {
            get
            {
                return this.features;
            }
        }

        internal SimSPContentTypeCollection ContentTypes
        {
            get
            {
                return this.contentTypes;
            }
        }

        internal SimSPContentTypeCollection AvailableContentTypes
        {
            get
            {
                return this.availableContentTypes;
            }
        }

        public SimSPFolderCollection Folders
        {
            get
            {
                return this.folders;
            }
        }

        public SimSPPropertyBag Properties
        {
            get
            {
                return this.properties;
            }
        }

        public SimSPWebCollection Webs
        {
            get
            {
                return this.webs;
            }
        }

        public int DisposeCount
        {
            get;
            private set;
        }

        public CultureInfo Locale
        {
            get
            {
                return this.locale;
            }
            set
            {
                this.locale = value;
            }
        }

        public string ServerRelativeUrl
        {
            get
            {
                return this.serverRelativeUrl;
            }
            set
            {
                this.serverRelativeUrl = value;
            }
        }

        public string Url
        {
            get
            {
                return this.url;
            }
            private set
            {
                this.url = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.UpdateUrls();
            }
        }

        public Guid ID
        {
            get
            {
                if (!this.id.HasValue)
                {
                    this.id = new Guid?(Guid.NewGuid());
                }
                return this.id.Value;
            }
            set
            {
                this.id = new Guid?(value);
            }
        }

        public bool Updated
        {
            get;
            set;
        }

        internal SimSPContext SPContext
        {
            get;
            set;
        }

        public new ShimSPWeb Fake
        {
            get;
            private set;
        }

        public new SPWeb Instance
        {
            get
            {
                return (SPWeb)base.Instance;
            }
        }

        public SimSPWeb()
            : this(ShimRuntime.CreateUninitializedInstance<SPWeb>())
        {
        }

        public SimSPWeb(SPWeb instance)
            : base(instance)
        {
            this.fields = new SimSPFieldCollection();
            this.availableFields = new SimSPFieldCollection();
            this.lists = new SimSPListCollection();
            this.users = new SimSPUserCollection();
            this.features = new SimSPFeatureCollection();
            this.contentTypes = new SimSPContentTypeCollection();
            this.availableContentTypes = new SimSPContentTypeCollection();
            this.folders = new SimSPFolderCollection();
            this.properties = new SimSPPropertyBag();
            this.webs = new SimSPWebCollection(this);
            this.allProperties = new Hashtable();
            this.Exists = true;
            this.lists.Web = this.Instance;
            this.features.ScopeParent = this.Instance;
            this.contentTypes.Web = this.Instance;
            this.fields.Web = this.Instance;

            var shimSPWeb = new ShimSPWeb(instance);
            shimSPWeb.IDGet = (() => this.ID);
            shimSPWeb.UrlGet = (() => this.Url);
            shimSPWeb.TitleGet = (() => this.Title);
            shimSPWeb.NameGet = (() => this.Name);
            shimSPWeb.ParentWebGet = (() => this.ParentWeb);
            shimSPWeb.ParentWebIdGet = (() => this.ParentWebId);
            shimSPWeb.RootFolderGet = (() => this.RootFolder.Instance);
            shimSPWeb.ServerRelativeUrlGet = (() => this.ServerRelativeUrl);
            shimSPWeb.ServerRelativeUrlSetString = (delegate(string value)
            {
                this.ServerRelativeUrl = value;
            });
            shimSPWeb.SiteGet = (() => this.Site);
            shimSPWeb.CurrentUserGet = (() => this.CurrentUser);
            shimSPWeb.AllowUnsafeUpdatesGet = (() => this.AllowUnsafeUpdates);
            shimSPWeb.AllowUnsafeUpdatesSetBoolean = (delegate(bool value)
            {
                this.AllowUnsafeUpdates = value;
            });
            shimSPWeb.ExistsGet = (() => this.Exists);
            shimSPWeb.LocaleGet = (() => this.Locale);
            shimSPWeb.ListsGet = (() => this.lists.Instance);
            shimSPWeb.UsersGet = (() => this.users.Instance);
            shimSPWeb.FeaturesGet = (() => this.features.Instance);
            shimSPWeb.ContentTypesGet = (() => this.contentTypes.Instance);
            shimSPWeb.AvailableContentTypesGet = (() => this.availableContentTypes.Instance);
            shimSPWeb.FieldsGet = (() => this.fields.Instance);
            shimSPWeb.AvailableFieldsGet = (() => this.availableFields.Instance);
            shimSPWeb.PropertiesGet = (() => this.properties.Instance);
            shimSPWeb.WebsGet = (() => this.webs.Instance);
            shimSPWeb.AllPropertiesGet = (() => this.allProperties);
            shimSPWeb.FoldersGet = (() => this.folders.Instance);
            shimSPWeb.GetFolderString = (delegate(string url)
            {
                using (IEnumerator<SPFolder> enumerator = (
                    from folder in this.folders
                    where folder.Url == url
                    select folder).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        return enumerator.Current;
                    }
                }
                throw new ArgumentException(url);
            });
            shimSPWeb.GetFolderGuid = (delegate(Guid id)
            {
                using (IEnumerator<SPFolder> enumerator = (
                    from folder in this.folders
                    where folder.UniqueId == id
                    select folder).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        return enumerator.Current;
                    }
                }
                throw new ArgumentException(id.ToString());
            });
            shimSPWeb.Dispose = (delegate
            {
                this.DisposeCount++;
            });
            shimSPWeb.Update = (delegate
            {
                this.Updated = true;
            });
            shimSPWeb.SetPropertyObjectObject = (delegate(object name, object value)
            {
                this.AllProperties[name] = value;
            });
            shimSPWeb.GetPropertyObject = ((object name) => this.AllProperties[name]);
            shimSPWeb.GetListString = ((string url) => (
                from list in this.Lists
                let blist = SimSPList.FromInstance(list)
                where blist != null && blist.Url == url
                select list).FirstOrDefault<SPList>());
            shimSPWeb.Delete = (new FakesDelegates.Action(this.Delete));

            this.Fake = shimSPWeb;
        }

        public void Delete()
        {
            this.ParentWeb.Webs.Delete(this.Name);
        }

        public static SimSPWeb FromInstance(SPWeb instance)
        {
            return InstancedPool.CastAsInstanced<SPWeb, SimSPWeb>(instance);
        }

        internal new static void Initialize()
        {
            SimSPSecurableObject.Initialize();
            ShimSPWeb.BehaveAsNotImplemented();
        }

        internal void UpdateUrls()
        {
            if (this.ParentWeb != null)
            {
                this.ServerRelativeUrl = UrlHelper.ConstructRelative(this.ParentWeb.ServerRelativeUrl, this.Name);
                this.Url = UrlHelper.Construct(this.ParentWeb.Url, this.Name);
            }
            else
            {
                this.ServerRelativeUrl = UrlHelper.ConstructRelative(string.Empty, this.Name);
                if (this.Site != null)
                {
                    this.Url = UrlHelper.Construct(this.Site.Url, this.Name);
                }
            }
        }
    }
}
