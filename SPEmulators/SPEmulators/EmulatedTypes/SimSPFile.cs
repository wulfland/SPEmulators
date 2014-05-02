namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPFile : Isolator<SPFile, ShimSPFile>
    {
        public SimSPListItem Item
        {
            get;
            internal set;
        }

        public string Name
        {
            get;
            set;
        }

        internal SimSPFileCollection ParentCollection
        {
            get;
            set;
        }

        public SimSPFile()
            : this(ShimRuntime.CreateUninitializedInstance<SPFile>())
        {
        }

        public SimSPFile(SPFile instance)
            : base(instance)
        {
            base.Fake.Delete = new FakesDelegates.Action(this.Delete);
            base.Fake.ItemGet = () => this.Item.Instance;
            base.Fake.NameGet = () => this.Name;
        }

        public void Delete()
        {
            this.ParentCollection.Delete(this.Name);
        }

        public static SimSPFile FromInstance(SPFile instance)
        {
            return InstancedPool.CastAsInstanced<SPFile, SimSPFile>(instance);
        }

        internal static void Initialize()
        {
            ShimSPFile.BehaveAsNotImplemented();
        }
    }
}
