namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPItem : SimSPSecurableObject, ICanIsolate<SPItem, ShimSPItem>, IInstanced<SPItem>, IInstanced
    {
        public new ShimSPItem Fake
        {
            get;
            private set;
        }

        public new SPItem Instance
        {
            get
            {
                return (SPItem)base.Instance;
            }
        }

        protected SimSPItem(SPItem instance)
            : base(instance)
        {
            this.Fake = new ShimSPItem(instance);
        }

        public static SimSPItem FromInstance(SPItem instance)
        {
            return InstancedPool.CastAsInstanced<SPItem, SimSPItem>(instance);
        }
        internal new static void Initialize()
        {
            SimSPSecurableObject.Initialize();
            ShimSPItem.BehaveAsNotImplemented();
        }
    }
}
