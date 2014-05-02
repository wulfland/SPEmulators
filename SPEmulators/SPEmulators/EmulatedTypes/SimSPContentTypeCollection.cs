namespace SPEmulators.EmulatedTypes
{
    using System;
    using System.Linq;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Fakes;

    internal class SimSPContentTypeCollection : CollectionIsolator<SPContentType, SPContentTypeCollection, ShimSPContentTypeCollection>
    {
        private SPWeb web;

        public SPList List
        {
            get;
            set;
        }
        public SPWeb Web
        {
            get
            {
                SPWeb result;
                if (this.web != null)
                {
                    result = this.web;
                }
                else
                {
                    if (this.List != null)
                    {
                        result = this.List.Lists.Web;
                    }
                    else
                    {
                        result = this.web;
                    }
                }
                return result;
            }
            set
            {
                this.web = value;
            }
        }

        public bool ReadOnly
        {
            get;
            set;
        }

        public SimSPContentTypeCollection()
            : this(ShimRuntime.CreateUninitializedInstance<SPContentTypeCollection>())
        {
        }

        public SimSPContentTypeCollection(SPContentTypeCollection instance)
            : base(instance)
        {
            base.Fake.Bind(this);
            base.Fake.BestMatchSPContentTypeId = (SPContentTypeId id) =>
            {
                SPContentTypeId result;
                foreach (SPContentType current in this)
                {
                    if (current.Id == id)
                    {
                        result = current.Id;
                        return result;
                    }
                }
                result = SPContentTypeId.Empty;
                return result;
            };
            base.Fake.AddSPContentType = (SPContentType contentType) =>
            {
                base.Add(contentType);
                return contentType;
            };
            base.Fake.DeleteSPContentTypeId = (SPContentTypeId id) =>
            {
                if (this.ReadOnly)
                {
                    throw new InvalidOperationException("The collection is readonly and cannot be modified.");
                }
                for (int i = 0; i < base.Count; i++)
                {
                    SPContentType contentType = base[i];
                    if (contentType.Id == id)
                    {
                        base.RemoveAt(i);
                        return;
                    }
                }
                throw new ArgumentOutOfRangeException("A content type with this id does not exist.");
            };

            base.Fake.ItemGetInt32 = (int index) => base[index];
            base.Fake.ItemGetString = ((string name) => this.FirstOrDefault((SPContentType contentType) => contentType.Name == name));
            base.Fake.ItemGetSPContentTypeId = ((SPContentTypeId id) => this.FirstOrDefault((SPContentType contentType) => contentType.Id == id));
            base.Fake.ReadOnlyGet = () => this.ReadOnly;
            base.Fake.ListGet = () => this.List;
            base.Fake.WebGet = () => this.Web;
            SimSPContentType.Initialize();
        }
    }
}
