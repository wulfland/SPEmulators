namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPListCollection : CollectionIsolator<SPList, SPListCollection, ShimSPListCollection>
    {
        public SPWeb Web { get; set; }
    }
}
