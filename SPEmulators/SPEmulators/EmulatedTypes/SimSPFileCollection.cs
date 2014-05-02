namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPFileCollection : CollectionIsolator<SPFile, SPFileCollection, ShimSPFileCollection>
    {
        internal SimSPFolder Folder
        {
            get;
            set;
        }

        public SimSPFileCollection()
            : this(ShimRuntime.CreateUninitializedInstance<SPFileCollection>())
        {
        }

        public SimSPFileCollection(SPFileCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this);
            base.Fake.AddStringByteArray = (string url, byte[] file) => this.Add(url, file).Instance;
            base.Fake.DeleteString = new FakesDelegates.Action<string>(this.Delete);
        }

        public SimSPFile Add(string name, byte[] contents)
        {
            var simSPFile = new SimSPFile
            {
                ParentCollection = this,
                Name = name
            };

            base.Add(simSPFile.Instance);
            if (this.Folder != null && this.Folder.ParentList != null)
            {
                var simSPListItem = this.Folder.ParentList.Items.Add();
                simSPListItem.Name = name;
                simSPListItem.FileSystemObjectType = 0;
                simSPListItem.File = simSPFile;
                simSPListItem.Update();
                simSPFile.Item = simSPListItem;
            }

            return simSPFile;
        }

        public void Delete(string fileName)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].Name == fileName)
                {
                    base.RemoveAt(i);
                    return;
                }
            }
            throw new SPException();
        }
        internal static void Initialize()
        {
            ShimSPFileCollection.BehaveAsNotImplemented();
        }
    }
}
