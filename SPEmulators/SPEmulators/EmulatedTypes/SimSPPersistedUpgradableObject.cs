namespace SPEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Administration.Fakes;

    internal class SimSPPersistedUpgradableObject : SimSPPersistedObject, ICanIsolate<SPPersistedUpgradableObject, ShimSPPersistedUpgradableObject>, IInstanced<SPPersistedUpgradableObject>, IInstanced
    {
        public new ShimSPPersistedUpgradableObject Fake
        {
            get;
            private set;
        }

        public new SPPersistedUpgradableObject Instance
        {
            get
            {
                return (SPPersistedUpgradableObject)base.Instance;
            }
        }

        public SimSPPersistedUpgradableObject()
            : this(ShimRuntime.CreateUninitializedInstance<SPPersistedUpgradableObject>())
        {
        }

        public SimSPPersistedUpgradableObject(SPPersistedUpgradableObject instance)
            : this(instance, null)
        {
        }

        public SimSPPersistedUpgradableObject(SPPersistedUpgradableObject instance, string defaultName)
            : base(instance, defaultName)
        {
            this.Fake = new ShimSPPersistedUpgradableObject(instance);
        }

        public static SimSPPersistedUpgradableObject FromInstance(SPPersistedUpgradableObject instance)
        {
            return InstancedPool.CastAsInstanced<SPPersistedUpgradableObject, SimSPPersistedUpgradableObject>(instance);
        }
    }
}
