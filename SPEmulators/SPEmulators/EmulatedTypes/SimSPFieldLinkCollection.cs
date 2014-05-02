namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Linq;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPFieldLinkCollection : CollectionIsolator<SPFieldLink, SPFieldLinkCollection, ShimSPFieldLinkCollection>
    {
        public SimSPFieldLinkCollection()
            : this(ShimRuntime.CreateUninitializedInstance<SPFieldLinkCollection>())
        {
        }

        public SimSPFieldLinkCollection(SPFieldLinkCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this);
            base.Fake.AddSPFieldLink = new FakesDelegates.Action<SPFieldLink>(base.Add);
            base.Fake.ItemGetString = (string name) =>
            {
                SPFieldLink fieldLink = this.FirstOrDefault((SPFieldLink f) => f.Name == name);
                if (fieldLink != null)
                {
                    return fieldLink;
                }
                throw new ArgumentException();
            };
            base.Fake.ItemGetGuid = (Guid id) =>
            {
                SPFieldLink fieldLink = this.FirstOrDefault((SPFieldLink f) => f.Id == id);
                if (fieldLink != null)
                {
                    return fieldLink;
                }
                throw new ArgumentException();
            };
        }

        internal static void Initialize()
        {
            ShimSPFieldLinkCollection.BehaveAsNotImplemented();
        }
    }
}
