namespace SPEmulators.EmulatedTypes
{
    using System.Collections.Generic;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPItemEventDataCollection : Isolator<SPItemEventDataCollection, ShimSPItemEventDataCollection>
    {
        private readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                object result;
                this.Properties.TryGetValue(key, out result);
                return result;
            }
            set
            {
                this.Properties[key] = value;
            }
        }

        public IDictionary<string, object> Properties
        {
            get
            {
                return this.properties;
            }
        }

        public SimSPItemEventDataCollection()
            : this(ShimRuntime.CreateUninitializedInstance<SPItemEventDataCollection>())
        {
        }

        public SimSPItemEventDataCollection(SPItemEventDataCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this.Properties);
            base.Fake.ItemGetString = (string key) => this[key];
            base.Fake.ItemSetStringObject = delegate(string key, object value)
            {
                this[key] = value;
            };
        }

        public static SimSPItemEventDataCollection FromInstance(SPItemEventDataCollection instance)
        {
            return InstancedPool.CastAsInstanced<SPItemEventDataCollection, SimSPItemEventDataCollection>(instance);
        }

        internal static void Initialize()
        {
            ShimSPItemEventDataCollection.BehaveAsNotImplemented();
        }
    }
}
