namespace SPEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPList : Isolator<SPList, ShimSPList>
    {
        private string title;
        private readonly SimSPContentTypeCollection contentTypes;
        private readonly SimSPFieldIndexCollection fieldIndexes;
        private readonly SimSPFieldCollection fields;
        private readonly SimSPListItemCollection items;
        private readonly SimSPViewCollection views;
        private SPFolder rootFolder;

        public Guid ID
        {
            get;
            set;
        }
        public bool EnableAttachments
        {
            get;
            set;
        }

        public bool Hidden
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public bool ContentTypesEnabled
        {
            get;
            set;
        }
        public SPWeb ParentWeb
        {
            get
            {
                return this.Lists.Web;
            }
        }
        public SPListCollection Lists
        {
            get;
            set;
        }

        internal SimSPContentTypeCollection ContentTypes
        {
            get
            {
                return this.contentTypes;
            }
        }

        public SimSPListItemCollection Items
        {
            get
            {
                return this.items;
            }
        }

        public SimSPFieldCollection Fields
        {
            get
            {
                return this.fields;
            }
        }

        public SimSPFieldIndexCollection FieldIndexes
        {
            get
            {
                return this.fieldIndexes;
            }
        }
 
        internal SimSPViewCollection Views
        {
            get
            {
                return this.views;
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
                bool flag = this.title == null;
                this.title = value;
                if (flag)
                {
                    var simView = SimSPView.FromInstance(this.views.DefaultView);
                    if (value != null && simView != null)
                    {
                        string arg = this.TrimOffExcessString(this.Title.Replace(".", string.Empty));
                        simView.ServerRelativeUrl = string.Format("/Lists/{0}/AllItems.aspx", arg);
                    }
                }
            }
        }
 
        public SPFolder RootFolder
        {
            get
            {
                if (this.rootFolder == null)
                {
                    this.rootFolder = new SimSPFolder
                    {
                        ParentList = this
                    }.Instance;
                }

                return this.rootFolder;
            }
        }

        public string RootFolderUrl
        {
            get
            {
                return this.RootFolder.Url;
            }
        }
 
        public bool? Updated
        {
            get;
            set;
        }

        public new ShimSPList Fake
        {
            get;
            private set;
        }

        public new SPList Instance
        {
            get
            {
                return (SPList)base.Instance;
            }
        }

        public SimSPList()
            : this(ShimRuntime.CreateUninitializedInstance<SPList>())
        {
        }

        protected SimSPList(SPList instance)
            : base(instance)
        {
            this.contentTypes = new SimSPContentTypeCollection();
            this.fields = new SimSPFieldCollection();
            this.fieldIndexes = new SimSPFieldIndexCollection();
            this.items = new SimSPListItemCollection();
            this.views = new SimSPViewCollection();
            this.contentTypes.List = this.Instance;
            this.fields.List = this.Instance;
            this.fieldIndexes.List = this.Instance;
            this.items.List = this.Instance;
            this.views.List = this.Instance;
            this.GetOrCreateDefaultView();
            
            var shimSPList = new ShimSPList(instance);
            shimSPList.IDGet = () => this.ID;
            shimSPList.HiddenGet = () => this.Hidden;
            shimSPList.HiddenSetBoolean = (bool value) => 
            {
                this.Hidden = value;
            };
            shimSPList.EnableAttachmentsGet = () => this.EnableAttachments;
            shimSPList.EnableAttachmentsSetBoolean = (bool value) =>
            {
                this.EnableAttachments = value;
            };
            shimSPList.TitleGet = () => this.Title;
            shimSPList.TitleSetString = (string newTitle) =>
            {
                this.Title = newTitle;
            };
            shimSPList.RootFolderGet = () => this.RootFolder;
            shimSPList.RootFolderUrlGet = () => this.RootFolderUrl;
            shimSPList.DefaultViewGet= () => this.GetOrCreateDefaultView().Instance;
            shimSPList.DefaultViewUrlGet = () =>
            {
                SimSPView orCreateDefaultView = this.GetOrCreateDefaultView();
                return orCreateDefaultView.ServerRelativeUrl;
            };
            shimSPList.ParentWebGet= () => this.ParentWeb;
            shimSPList.Delete= new FakesDelegates.Action(this.Delete);
            shimSPList.ListsGet= () => this.Lists;
            shimSPList.ItemsGet= () => this.items.Instance;
            shimSPList.FieldsGet= () => this.fields.Instance;
            shimSPList.FieldIndexesGet= () => this.fieldIndexes.Instance;
            shimSPList.ContentTypesGet= () => this.contentTypes.Instance;
            shimSPList.ViewsGet= () => this.views.Instance;
            shimSPList.GetItemByIdInt32 = (int id) =>
            {
                foreach (SPListItem current in this.items)
                {
                    if (current.ID == id)
                    {
                        return current;
                    }
                }
                throw new ArgumentException();
            };
            shimSPList.GetItemsSPView = (SPView view) => this.GetItems().Instance;
            shimSPList.ItemCountGet = () => this.Items.Count;
            shimSPList.Update = () =>
            {
                this.Updated = new bool?(false);
            };
            shimSPList.UpdateBoolean = (bool fromMigration) =>
            {
                this.Updated = new bool?(fromMigration);
            };
            shimSPList.AddItem = () => this.AddItem();
            shimSPList.ContentTypesEnabledGet = () => this.ContentTypesEnabled;
            shimSPList.ContentTypesEnabledSetBoolean = (bool value) =>
            {
                this.ContentTypesEnabled = value;
            };
            this.Fake = shimSPList;
        }

        private SPListItem AddItem()
        {
            return new SimSPListItem
            {
                ListItems = this.items
            }.Instance;
        }

        public void Delete()
        {
            this.Lists.Delete(this.ID);
        }

        private SimSPListItemCollection GetItems()
        {
            var listItemCollection = new SimSPListItemCollection
            {
                List = this.Instance
            };

            foreach (SPListItem current in this.Items)
            {
                listItemCollection.Add(current);
            }

            return listItemCollection;
        }

        private string TrimOffExcessString(string input)
        {
            int num = input.Length;
            if (num > 50)
            {
                num = 50;
            }
            return input.Substring(0, num);
        }

        public static SimSPList FromInstance(SPList instance)
        {
            return InstancedPool.CastAsInstanced<SPList, SimSPList>(instance);
        }

        internal static void Initialize()
        {
            SimSPSecurableObject.Initialize();
            ShimSPList.BehaveAsNotImplemented();
        }

        private SimSPView GetOrCreateDefaultView()
        {
            var view = SimSPView.FromInstance(this.views.DefaultView);
            if (view == null)
            {
                view = this.views.SetOne();
                this.views.DefaultViewIndex = 0;
                view.Title = "Default View";
            }

            return view;
        }
    }
}
