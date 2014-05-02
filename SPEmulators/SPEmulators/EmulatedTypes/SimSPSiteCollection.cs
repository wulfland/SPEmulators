namespace SPEmulators.EmulatedTypes
{
    using System.Collections.Generic;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Administration.Fakes;

    internal class SimSPSiteCollection : CollectionIsolator<SPSite, SPSiteCollection, ShimSPSiteCollection>
    {
        public SPWebApplication WebApplication
        {
            get;
            set;
        }

        public SimSPSiteCollection() : this(null)
        {
        }

        public SimSPSiteCollection(SPSiteCollection instance) : base(instance)
        {
            base.Fake.Bind((IEnumerable<SPSite>)this);
            base.Fake.ItemGetInt32 = ((int index) => base[index]);
            base.Fake.ItemAtIndexInt32 = ((int index) => base[index]);
            base.Fake.WebApplicationGet = (() => this.WebApplication);
        }
    }
}
