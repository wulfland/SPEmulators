namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPFolder : Isolator<SPFolder, ShimSPFolder>
    {
        private SimSPFileCollection files;
        private SimSPFolderCollection subFolders;

        public SimSPFileCollection Files
        {
            get
            {
                if (this.files == null)
                {
                    this.files = new SimSPFileCollection
                    {
                        Folder = this
                    };
                }

                return this.files;
            }
        }

        public SimSPListItem Item
        {
            get;
            internal set;
        }

        public SimSPFolderCollection SubFolders
        {
            get
            {
                if (this.subFolders == null)
                {
                    this.subFolders = new SimSPFolderCollection
                    {
                        Folder = this
                    };
                }

                return this.subFolders;
            }
        }

        public string Name
        {
            get;
            set;
        }

        internal SimSPFolderCollection ParentCollection
        {
            get;
            set;
        }

        internal SimSPList ParentList
        {
            get;
            set;
        }

        public SimSPFolder()
            : this(null)
        {
        }

        public SimSPFolder(SPFolder instance)
            : base(instance)
        {
            base.Fake.Delete = new FakesDelegates.Action(this.Delete);
            base.Fake.FilesGet = () => this.Files.Instance;
            base.Fake.ItemGet = () => this.Item.Instance;
            base.Fake.SubFoldersGet = () => this.SubFolders.Instance;
            base.Fake.NameGet = () => this.Name;
        }

        public void Delete()
        {
            this.ParentCollection.Delete(this.Name);
        }

        public static SimSPFolder FromInstance(SPFolder instance)
        {
            return InstancedPool.CastAsInstanced<SPFolder, SimSPFolder>(instance);
        }

        internal static void Initialize()
        {
            ShimSPFolder.BehaveAsNotImplemented();
        }
    }
}
