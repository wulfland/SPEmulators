namespace SPEmulators.EmulatedTypes
{
    using System.Collections.Generic;
    using System.Collections.Specialized.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.Utilities.Fakes;

    internal class SimSPPropertyBag : Isolator<SPPropertyBag, ShimSPPropertyBag>
    {
        private readonly Dictionary<string, string> values = new Dictionary<string, string>();

        public IDictionary<string, string> Values
        {
            get
            {
                return this.values;
            }
        }

        public bool Updated
        {
            get;
            private set;
        }

        public SimSPPropertyBag()
            : this(ShimRuntime.CreateUninitializedInstance<SPPropertyBag>())
        {
        }

        public SimSPPropertyBag(SPPropertyBag instance)
            : base(instance)
        {
            base.Fake.Update = (delegate
            {
                this.Updated = true;
            });
            var shimStringDictionary = new ShimStringDictionary(base.Instance);
            shimStringDictionary.AddStringString = (delegate(string key, string value)
            {
                this.values.Add(key, value);
            });
            shimStringDictionary.Clear = (delegate
            {
                this.values.Clear();
            });
            shimStringDictionary.ContainsKeyString = ((string key) => this.values.ContainsKey(key));
            shimStringDictionary.ContainsValueString = ((string value) => this.Values.Values.Contains(value));
            shimStringDictionary.CountGet = (() => this.values.Count);
            shimStringDictionary.GetEnumerator = (() => this.values.GetEnumerator());
            shimStringDictionary.IsSynchronizedGet = (() => false);
            shimStringDictionary.ItemGetString = ((string key) => this.values[key]);
            shimStringDictionary.ItemSetStringString = (delegate(string key, string value)
            {
                this.values[key] = value;
            });
            shimStringDictionary.KeysGet = (() => this.values.Keys);
            shimStringDictionary.RemoveString = (delegate(string key)
            {
                this.values.Remove(key);
            });
            shimStringDictionary.SyncRootGet = (() => this);
            shimStringDictionary.ValuesGet = (() => this.values.Values);
        }
    }
}
