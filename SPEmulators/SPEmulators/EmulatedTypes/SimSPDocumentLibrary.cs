namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPDocumentLibrary : SimSPList, ICanIsolate<SPDocumentLibrary, ShimSPList>, IInstanced<SPDocumentLibrary>, IInstanced
    {
        public new SPDocumentLibrary Instance
        {
            get
            {
                return (SPDocumentLibrary)base.Instance;
            }
        }

        public SimSPDocumentLibrary()
            : this(ShimRuntime.CreateUninitializedInstance<SPDocumentLibrary>())
        {
        }

        public SimSPDocumentLibrary(SPDocumentLibrary instance)
            : base(instance)
        {
        }

        public static SimSPDocumentLibrary FromInstance(SPDocumentLibrary instance)
        {
            return InstancedPool.CastAsInstanced<SPDocumentLibrary, SimSPDocumentLibrary>(instance);
        }
    }
}
