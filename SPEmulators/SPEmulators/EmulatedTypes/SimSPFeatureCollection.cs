namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPFeatureCollection : CollectionIsolator<SPFeature, SPFeatureCollection, ShimSPFeatureCollection>
    {
        public object ScopeParent
        {
            get;
            set;
        }

        public SimSPFeatureCollection()
            : this(null)
        {
        }

        public SimSPFeatureCollection(SPFeatureCollection instance)
            : base(instance)
        {
            base.Fake.Bind((IEnumerable<SPFeature>)this);
            base.Fake.ItemAtIndexInt32 = ((int index) => base[index]);
            base.Fake.ItemGetGuid = ((Guid id) => this.FirstOrDefault((SPFeature feature) => feature.DefinitionId == id));
        }
    }
}
