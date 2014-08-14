namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPListCollection : CollectionIsolator<SPList, SPListCollection, ShimSPListCollection>
    {
        private Guid assetListId;
        private Guid pagesListId;

        public SimSPList this[Guid listId]
        {
            get
            {
                SPList sPList = this.FirstOrDefault((SPList list) => list.ID == listId);
                if (sPList != null)
                {
                    return SimSPList.FromInstance(sPList);
                }
                throw new SPException("listId");
            }
        }

        public SPWeb Web
        {
            get;
            set;
        }

        public SimSPListCollection()
            : this(ShimRuntime.CreateUninitializedInstance<SPListCollection>())
        {
        }

        public SimSPListCollection(SPListCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this);
            base.Fake.ItemGetInt32 = ((int index) => base[index]);
            base.Fake.ItemAtIndexInt32 = ((int index) => base[index]);
            base.Fake.ItemGetGuid = ((Guid id) => this[id].Instance);
            base.Fake.ItemGetString = ((string title) => this.GetListName(title, true));
            base.Fake.WebGet = (() => this.Web);
            base.Fake.GetListGuidBoolean = ((Guid listId, bool fetchMetadata) => this.GetList(listId, fetchMetadata).Instance);
            base.Fake.GetListByNameStringBoolean = (new FakesDelegates.Func<string, bool, SPList>(this.GetListName));
            base.Fake.AddStringStringSPListTemplate = (delegate(string title, string description, SPListTemplate template)
            {
                return this.AddItem(title, description);
            });
            base.Fake.AddStringStringSPListTemplateSPDocTemplate = (delegate(string title, string description, SPListTemplate template, SPDocTemplate docTemplate)
            {
                return this.AddItem(title, description);
            });
            base.Fake.AddStringStringSPListTemplateType = (new FakesDelegates.Func<string, string, SPListTemplateType, Guid>(this.Add));
            base.Fake.TryGetListString = ((string title) => this.GetListName(title, false));
            base.Fake.DeleteGuid = (new FakesDelegates.Action<Guid>(this.Delete));
            base.Fake.EnsureSiteAssetsLibrary = (delegate
            {
                if (this.assetListId == Guid.Empty)
                {
                    this.assetListId = this.AddItem("Assets", "List designed as a default asset location for images.");
                }
                foreach (SPList current in this)
                {
                    if (current.ID == this.assetListId)
                    {
                        return current;
                    }
                }
                throw new InvalidOperationException("Unable to find list.");
            });
            base.Fake.EnsureSitePagesLibrary = (delegate
            {
                if (this.pagesListId == Guid.Empty)
                {
                    this.pagesListId = this.AddItem("Pages", "List designed as a default asset location for wiki pages.");
                }
                foreach (SPList current in this)
                {
                    if (current.ID == this.pagesListId)
                    {
                        return current;
                    }
                }
                throw new InvalidOperationException("Unbale to find list.");
            });
        }

        public Guid AddItem(string title, string description)
        {
            return this.Add(title, description, SPListTemplateType.GenericList);
        }

        public Guid Add(string title, string description, SPListTemplateType templateType)
        {
            var simList = (templateType == SPListTemplateType.DocumentLibrary) ? new SimSPDocumentLibrary() : new SimSPList();
            simList.Title = title;
            this.Initialize(simList);
            base.Add(simList.Instance);

            return simList.ID;
        }

        public void Delete(Guid uniqueId)
        {
            foreach (SPList current in this)
            {
                if (current.ID == uniqueId)
                {
                    base.Remove(current);
                    return;
                }
            }

            throw new InvalidOperationException("Unbale to find list.");
        }

        public SimSPList GetList(Guid listId, bool fetchMetadata)
        {
            return this[listId];
        }

        private SPList GetListName(string title, bool @throw)
        {
            SPList result;
            foreach (SPList current in this)
            {
                if (string.Equals(current.Title, title, StringComparison.Ordinal))
                {
                    result = current;
                    return result;
                }
            }
            if (@throw)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "List '{0}' was not found.", new object[]
                {
                    title
                }));
            }
            result = null;

            return result;
        }

        public bool TryGetWeb(out SPWeb outWeb)
        {
            outWeb = this.Web;
            return outWeb != null;
        }

        public SimSPList SetOne()
        {
            var simList = this.CreateList();
            base.Clear();
            base.Add(simList.Instance);
            return simList;
        }

        public SimSPList SetNext()
        {
            var simList = this.CreateList();
            base.Add(simList.Instance);

            return simList;
        }

        protected override SPList CreateItem()
        {
            return this.CreateList().Instance;
        }

        private SimSPList CreateList()
        {
            var simList = new SimSPList();
            this.Initialize(simList);

            return simList;
        }

        private void Initialize(SimSPList list)
        {
            list.ID = Guid.NewGuid();
            list.Lists = base.Instance;
            list.Fields.Add(new SimSPField
            {
                Id = SPBuiltInFieldId.ID,
                InternalName = "ID",
                Title = "ID", 
                Type = SPFieldType.Counter
            }.Instance);
            list.Fields.Add(new SimSPField
            {
                Id = SPBuiltInFieldId.Title,
                InternalName = "Title",
                Title = "Title",
                TypeDisplayName = "Title"
            }.Instance);
            list.Fields.Add(new SimSPField
            {
                Id = SPBuiltInFieldId.Modified,
                Type = (SPFieldType)4,
                InternalName = "Modified",
                Title = "Modified"
            }.Instance);
            list.Fields.Add(new SimSPField
            {
                Id = SPBuiltInFieldId.URL,
                Type = (SPFieldType)11,
                InternalName = "URL",
                Title = "URL"
            }.Instance);
        }

        public static SimSPListCollection FromInstance(SPListCollection instance)
        {
            return InstancedPool.CastAsInstanced<SPListCollection, SimSPListCollection>(instance);
        }

        internal static void Initialize()
        {
            ShimSPListCollection.BehaveAsNotImplemented();
        }
    }
}
