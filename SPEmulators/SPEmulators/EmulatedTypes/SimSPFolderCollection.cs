namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPFolderCollection : CollectionIsolator<SPFolder, SPFolderCollection, ShimSPFolderCollection>
    {
        public SimSPFolder Folder
        {
            get;
            internal set;
        }

        public SimSPFolderCollection()
            : this(ShimRuntime.CreateUninitializedInstance<SPFolderCollection>())
        {
        }

        public SimSPFolderCollection(SPFolderCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this);
            base.Fake.AddString = (string url) => this.Add(url).Instance;
            base.Fake.DeleteString = new FakesDelegates.Action<string>(this.Delete);
            base.Fake.FolderGet = () => this.Folder.Instance;
        }

        public SimSPFolder Add(string url)
        {
            var simSPFolder = new SimSPFolder
            {
                Name = url,
                ParentCollection = this
            };

            base.Add(simSPFolder.Instance);
            if (this.Folder != null && this.Folder.ParentList != null)
            {
                var simSPListItem = this.Folder.ParentList.Items.Add();
                simSPListItem.Name = url;
                simSPListItem.FileSystemObjectType = SPFileSystemObjectType.Folder;
                simSPListItem.Folder = simSPFolder;
                simSPListItem.Update();
                simSPFolder.Item = simSPListItem;
            }

            return simSPFolder;
        }

        public void Delete(string url)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].Name == url)
                {
                    base.RemoveAt(i);
                    break;
                }
            }
        }
        internal static void Initialize()
        {
            ShimSPFolderCollection.BehaveAsNotImplemented();
        }
    }
}
