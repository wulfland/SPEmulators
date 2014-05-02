namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPListItem : SimSPItem, ICanIsolate<SPListItem, ShimSPListItem>, IInstanced<SPListItem>, IInstanced
    {
        private readonly Dictionary<Guid, object> fieldValues = new Dictionary<Guid, object>();

        public int ID
        {
            get;
            set;
        }

        public SPFileSystemObjectType FileSystemObjectType
        {
            get;
            set;
        }

        public SimSPListItemCollection ListItems
        {
            get;
            set;
        }
        public SPList ParentList
        {
            get
            {
                return this.ListItems.List;
            }
        }

        public object this[Guid fieldId]
        {
            get
            {
                object result;
                this.fieldValues.TryGetValue(fieldId, out result);
                return result;
            }
            set
            {
                this.fieldValues[fieldId] = value;
            }
        }

        public object this[int fieldIndex]
        {
            get
            {
                return this[this.ParentList.Fields[fieldIndex].Id];
            }
            set
            {
                this[this.ParentList.Fields[fieldIndex].Id] = value;
            }
        }

        public object this[string fieldName]
        {
            get
            {
                return this[this.GetFieldId(fieldName)];
            }
            set
            {
                this[this.GetFieldId(fieldName)] = value;
            }
        }

        public string Name
        {
            get
            {
                return this.Title;
            }
            set
            {
                this.Title = value;
            }
        }

        public string Title
        {
            get
            {
                return ((string)this[SPBuiltInFieldId.Title]) ?? string.Empty;
            }
            set
            {
                this[SPBuiltInFieldId.Title] = value;
            }
        }

        public SimSPFolder Folder
        {
            get;
            internal set;
        }

        public SimSPFile File
        {
            get;
            internal set;
        }

        public Guid UniqueId
        {
            get;
            set;
        }

        public new ShimSPListItem Fake
        {
            get;
            private set;
        }

        public new SPListItem Instance
        {
            get
            {
                return (SPListItem)base.Instance;
            }
        }
        public SimSPListItem()
            : this(ShimRuntime.CreateUninitializedInstance<SPListItem>())
        {
        }

        public SimSPListItem(SPListItem instance)
            : base(instance)
        {
            ShimSPListItem shimSPListItem = new ShimSPListItem(instance);
            shimSPListItem.IDGet = () => this.ID;
            shimSPListItem.FileSystemObjectTypeGet = () => this.FileSystemObjectType;
            shimSPListItem.FileSystemObjectTypeSetSPFileSystemObjectType = (SPFileSystemObjectType value) =>
            {
                this.FileSystemObjectType = value;
            };
            shimSPListItem.SystemUpdate = () =>
            {
                this.SystemUpdate();
            };
            shimSPListItem.SystemUpdateBoolean = (bool incrementListVersion) =>
            {
                this.SystemUpdate(incrementListVersion);
            };
            shimSPListItem.Delete = new FakesDelegates.Action(this.Delete);
            shimSPListItem.Update = new FakesDelegates.Action(this.Update);
            shimSPListItem.FolderGet = () => this.Folder.Instance;
            shimSPListItem.FileGet = () => this.File.Instance;
            shimSPListItem.ItemGetInt32 = (int fieldIndex) => this[fieldIndex];
            shimSPListItem.ItemSetInt32Object = delegate(int fieldIndex, object fieldValue)
            {
                this[fieldIndex] = fieldValue;
            };
            shimSPListItem.ItemGetGuid = (Guid fieldId) => this[fieldId];
            shimSPListItem.ItemSetGuidObject = delegate(Guid fieldId, object fieldValue)
            {
                this[fieldId] = fieldValue;
            };
            shimSPListItem.ItemGetString = (string fieldName) => this[fieldName];
            shimSPListItem.ItemSetStringObject = delegate(string fieldName, object fieldValue)
            {
                this[fieldName] = fieldValue;
            };
            shimSPListItem.NameGet = () => this.Name;
            shimSPListItem.NameSetString = delegate(string value)
            {
                this.Name = value;
            };
            shimSPListItem.ParentListGet = () => this.ParentList;
            shimSPListItem.ListItemsGet = () => this.ListItems.Instance;
            shimSPListItem.TitleGet = () => this.Title;
            shimSPListItem.UniqueIdGet = () => this.UniqueId;
            this.Fake = shimSPListItem;
        }
        private Guid GetFieldId(string fieldName)
        {
            using (IEnumerator<SPField> enumerator = (
                from SPField field in this.ParentList.Fields
                where field.Title == fieldName || field.InternalName == fieldName
                select field).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    SPField current = enumerator.Current;
                    return current.Id;
                }
            }
            throw new ArgumentException();
        }

        public SimSPFile SetFile()
        {
            var simSPFile = new SimSPFile();
            this.File = simSPFile;

            return simSPFile;
        }

        public static SimSPListItem FromInstance(SPListItem instance)
        {
            return InstancedPool.CastAsInstanced<SPListItem, SimSPListItem>(instance);
        }

        public void Delete()
        {
            this.ListItems.Remove(this.Instance);
        }

        public void SystemUpdate()
        {
            this.SystemUpdate(true);
        }

        public void SystemUpdate(bool incrementListVersion)
        {
            if (!this.ListItems.Contains(this.Instance))
            {
                this.ListItems.Add(this.Instance);
            }
            if (this.ID == 0)
            {
                this.ID = this.ListItems.Max((SPListItem item) => item.ID) + 1;
            }
            if (this.UniqueId == Guid.Empty)
            {
                this.UniqueId = Guid.NewGuid();
            }
            var fieldUrlValue = (SPFieldUrlValue)this[SPBuiltInFieldId.URL];
            if (fieldUrlValue != null && !fieldUrlValue.Url.StartsWith(this.ListItems.List.Lists.Web.Url))
            {
                fieldUrlValue.Url = fieldUrlValue.Url.Insert(0, this.ListItems.List.Lists.Web.Url);
                this[SPBuiltInFieldId.URL] = fieldUrlValue;
            }
        }
        public void Update()
        {
            this.SystemUpdate();
            this[SPBuiltInFieldId.Modified] = DateTime.Now;
        }

        internal new static void Initialize()
        {
            SimSPItem.Initialize();
            ShimSPListItem.BehaveAsNotImplemented();
        }
    }
}
