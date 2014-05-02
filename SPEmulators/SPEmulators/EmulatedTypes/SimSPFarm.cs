namespace SPEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Administration.Fakes;

    internal class SimSPFarm : SimSPPersistedUpgradableObject, ICanIsolate<SPFarm, ShimSPFarm>, IInstanced<SPFarm>, IInstanced
    {
        private static SPFarm local;

        public Guid TraceSessionGuid
        {
            get
            {
                return new Guid(2311637301u, 39423, 18638, 147, 118, 49, 218, 170, 243, 43, 133);
            }
        }

        public static SPFarm Local
        {
            get
            {
                return SimSPFarm.local;
            }
            set
            {
                SimSPFarm.local = value;
                ShimSPFarm.LocalGet = () => SimSPFarm.Local;
                ShimRuntime.RegisterStateCleaner(delegate
                {
                    SimSPFarm.local = null;
                });
            }
        }
        public new ShimSPFarm Fake
        {
            get;
            private set;
        }

        public new SPFarm Instance
        {
            get
            {
                return (SPFarm)base.Instance;
            }
        }

        public SimSPFarm()
            : this(ShimRuntime.CreateUninitializedInstance<SPFarm>())
        {
        }

        public SimSPFarm(SPFarm instance)
            : base(instance)
        {
            var shimSPFarm = new ShimSPFarm(instance);
            shimSPFarm.TraceSessionGuidGet = () => this.TraceSessionGuid;
            this.Fake = shimSPFarm;
        }

        public static void Initialize()
        {
            if (SimSPFarm.local == null)
            {
                SimSPFarm.SetLocal();
            }
        }

        public static SimSPFarm SetLocal()
        {
            var simFarm = new SimSPFarm();
            SimSPFarm.Local = simFarm.Instance;
            return simFarm;
        }

        public static SimSPFarm FromInstance(SPFarm instance)
        {
            return InstancedPool.CastAsInstanced<SPFarm, SimSPFarm>(instance);
        }
    }
}
