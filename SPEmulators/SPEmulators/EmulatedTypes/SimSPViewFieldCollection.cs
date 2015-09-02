using System.Collections.Specialized;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Fakes;

namespace SPEmulators.EmulatedTypes
{
    internal class SimSPViewFieldCollection :
        CollectionIsolator<SPField, SPViewFieldCollection, ShimSPViewFieldCollection>
    {
        public SPView View { get; set; }

        public SimSPViewFieldCollection()
            : this(null) {}

        public SimSPViewFieldCollection(SPViewFieldCollection instance)
            : base(instance)
        {
            Fake.Bind(this);
            Fake.AddSPField = Add;
            Fake.AddString = name => Add(View.ParentList.Fields[name]);
            Fake.CountGet = () => Count;
            Fake.DeleteSPField = field => Remove(field);
            Fake.DeleteString = name => Remove(View.ParentList.Fields[name]);
            Fake.DeleteAll = Clear;
            Fake.ExistsString = name => this.Any(field => field.InternalName == name);
            Fake.ItemGetInt32 = index => this[index].InternalName;
            Fake.ToStringCollection = ToStringCollection;
        }

        private StringCollection ToStringCollection()
        {
            var result = new StringCollection();
            foreach (var field in this)
                result.Add(field.InternalName);

            return result;
        }
    }
}